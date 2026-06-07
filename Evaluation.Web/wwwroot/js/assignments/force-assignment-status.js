
window.NDA_STATUS = {
    OBJECTION: "466D8F62-F8F9-4A3F-B364-82C77D7DE72D",
    APPROVE: "E679F265-2BDA-4A36-8C4C-8911EE26BB7B",
    SUSPENDED: "8F017EEF-84C1-46B9-9E89-9698BAB996CF",
    PENDING: "B08F866B-52E7-4B98-966B-B16F0C4FCCE7",
    APPROVED: "2E30F24C-A86D-48A5-B922-DB59859AB617",
    FORCED: "D3CCAB02-789D-4D44-A8BC-EBEFABCCF2C4"
};

window.renderForceAssignments = function (assignments, divId) {

    const container = $("#" + divId);

    if (!assignments || assignments.length === 0) {
        container.html("");
        return;
    }

    let html = `
                <table class="table table-bordered"
                  style="font-family:Tajawal,sans-serif; direction:rtl; font-size:13px;">
                    <thead style="background:#6B1E35;">
                        <tr>
                            <th style="color:#fff; font-weight:600; padding:11px 16px; white-space:nowrap; border:none; text-align:right;">
                                ${uiControlsSetup().GetUiControlText('lblMemberName')}</th>
                            <th style="color:#fff; font-weight:600; padding:11px 16px; white-space:nowrap; border:none; text-align:right;">
                                ${uiControlsSetup().GetUiControlText('lblPartyType')}</th>
                            <th style="color:#fff; font-weight:600; padding:11px 16px; white-space:nowrap; border:none; text-align:right;">
                                ${uiControlsSetup().GetUiControlText('lblNdaStatus')}</th>
                            <th style="color:#fff; font-weight:600; padding:11px 16px; white-space:nowrap; border:none; text-align:right;">
                                ${uiControlsSetup().GetUiControlText('lblTeamLeader')}</th>
                            <th style="color:#fff; font-weight:600; padding:11px 16px; white-space:nowrap; border:none; text-align:right;">
                                ${uiControlsSetup().GetUiControlText('lblNDADate')}</th>
                            <th style="color:#fff; font-weight:600; padding:11px 16px; white-space:nowrap; border:none; text-align:right;">
                                ${uiControlsSetup().GetUiControlText('lblNotes')}</th>
                            <th style="color:#fff; font-weight:600; padding:11px 16px; white-space:nowrap; border:none; text-align:right;">
                                ${uiControlsSetup().GetUiControlText('lblAction')}</th>
                        </tr>
                    </thead>
                    <tbody>
            `;


    assignments.forEach(x => {

        const ndaDate = x.ndaDate
            ? new Date(x.ndaDate).toLocaleString()
            : "-";
        const forceButton = x.ndaStatusId?.toLowerCase() === NDA_STATUS.OBJECTION?.toLowerCase()
            ? `
               <button class="btn btn-sm"
                        onclick="forceAssignmentStatus('${x.ministryUserId}','${x.evaluationRequestId}')"
                        style="background:#FEE2E2;color:#991B1B;border:0.5px solid #FECACA;
                               border-radius:7px;padding:5px 12px;font-size:12px;font-weight:600;
                               font-family:Tajawal,sans-serif;">
                    <i class="ti ti-shield-check"></i> Force
                </button>
              `
            : "";

        const sendEmailButton = x.ndaStatusId?.toLowerCase() === NDA_STATUS.PENDING?.toLowerCase()
            ? `
                               <button class="btn btn-sm"
                                        onclick="sendAssignmentEmail('${x.ministryUserId}','${x.evaluationRequestId}','${(x.ministryUser || '').replace(/'/g, "\\'")}')"
                                        style="background:var(--color-background-secondary);border:0.5px solid #ccc;
                                               border-radius:7px;width:34px;height:34px;display:inline-flex;
                                               align-items:center;justify-content:center;color:#6B1E35;font-size:16px;">
                                    <i class="las la-envelope"></i>
                                </button>
                              `
            : "";
        const ndaStatusBadge = (() => {
                                const id = x.ndaStatusId?.toLowerCase();
                                if (id === NDA_STATUS.PENDING?.toLowerCase())
                                    return `<span style="background:#FEF3C7;color:#92400E;border-radius:20px;padding:3px 10px;font-size:11px;font-weight:600;">
                                      <i class="ti ti-clock"></i> ${x.ndaStatus}
                                    </span>`;
                                if (id === NDA_STATUS.APPROVED?.toLowerCase() || id === NDA_STATUS.APPROVE?.toLowerCase())
                                    return `<span style="background:#D1FAE5;color:#065F46;border-radius:20px;padding:3px 10px;font-size:11px;font-weight:600;">
                                      <i class="ti ti-check"></i> ${x.ndaStatus}
                                    </span>`;
                                if (id === NDA_STATUS.OBJECTION?.toLowerCase())
                                    return `<span style="background:#FEE2E2;color:#991B1B;border-radius:20px;padding:3px 10px;font-size:11px;font-weight:600;">
                                      <i class="ti ti-x"></i> ${x.ndaStatus}
                                    </span>`;
                                if (id === NDA_STATUS.FORCED?.toLowerCase())
                                    return `<span style="background:#EDE9FE;color:#5B21B6;border-radius:20px;padding:3px 10px;font-size:11px;font-weight:600;">
                                      <i class="ti ti-lock"></i> ${x.ndaStatus}
                                    </span>`;
                                if (id === NDA_STATUS.SUSPENDED?.toLowerCase())
                                    return `<span style="background:#F3F4F6;color:#374151;border-radius:20px;padding:3px 10px;font-size:11px;font-weight:600;">
                                      <i class="ti ti-ban"></i> ${x.ndaStatus}
                                    </span>`;
                                return `<span style="color:#6c757d;">-</span>`;
                            })();
        html += `
            <tr id="assignment_row_${x.ministryUserId}">
                
                <td>${x.ministryUser || '-'}</td>

                <td>${x.partyType || '-'}</td>

                <td>${ndaStatusBadge}</td>

               <td>
                  ${x.isLeader
                                ? `<span style="background:#D1FAE5;color:#065F46;border-radius:20px;padding:2px 9px;font-size:11px;font-weight:600;"><i class="ti ti-check"></i> Yes</span>`
                                : `<span style="background:#FEE2E2;color:#991B1B;border-radius:20px;padding:2px 9px;font-size:11px;font-weight:600;"><i class="ti ti-x"></i> No</span>`
                  }
                </td>

                <td>${ndaDate}</td>

                <td>${x.note || '-'}</td>

                 <td>
                        ${sendEmailButton}
                        ${forceButton}
                </td>

            </tr>
        `;
    });

    html += `
            </tbody>
        </table>
    `;

    container.html(html);

    container.find('[data-bs-toggle="tooltip"]').each(function () {
        new bootstrap.Tooltip(this);
    });
};

window.forceAssignmentStatus = async function (ministryUserId, evaluationRequestId) {

    try {

        const response = await jqClient().Post(
            API_ENDPOINTS.FORCE_ASSIGNMENT_STATUS,
            {
                ministryUserId: ministryUserId,
                evaluationRequestId: evaluationRequestId
            }
        );

        if (response.isSuccess || response.success) {

            $("#assignment_row_" + ministryUserId)
                .css({ "background": "#D1FAE5", "transition": "background 0.3s" });

            $("#assignment_row_" + ministryUserId)
                .find("button")
                .remove();
        }

    } catch (e) {
        console.error(e);
    }
};