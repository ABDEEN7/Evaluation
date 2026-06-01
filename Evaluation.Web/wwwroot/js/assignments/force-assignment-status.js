
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
        <table class="table table-bordered">
            <thead>
                <tr>
                    <th>${uiControlsSetup().GetUiControlText('lblMemberName')}</th>
                    <th>${uiControlsSetup().GetUiControlText('lblPartyType')}</th>
                    <th>${uiControlsSetup().GetUiControlText('lblNdaStatus')}</th>
                    <th>${uiControlsSetup().GetUiControlText('lblTeamLeader')}</th>
                    <th>${uiControlsSetup().GetUiControlText('lblNDADate')}</th>
                    <th>${uiControlsSetup().GetUiControlText('lblNotes')}</th>
                    <th>${uiControlsSetup().GetUiControlText('lblAction')}</th>
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
                <button class="btn btn-danger btn-sm"
                        onclick="forceAssignmentStatus('${x.ministryUserId}','${x.evaluationRequestId}')">
                    Force
                </button>
              `
            : "";

        const sendEmailButton =x.ndaStatusId?.toLowerCase() === NDA_STATUS.PENDING?.toLowerCase()
                                    ? `
                                <button class="btn btn-sm btn-outline-primary"
                                        onclick="sendAssignmentEmail(
                                            '${x.ministryUserId}',
                                            '${x.evaluationRequestId}',
                                            '${(x.ministryUser || '').replace(/'/g, "\\'")}'
                                        )">
                                    <i class="las la-envelope"></i>
                                </button>
                              `
                                    : "";

        html += `
            <tr id="assignment_row_${x.ministryUserId}">
                
                <td>${x.ministryUser || '-'}</td>

                <td>${x.partyType || '-'}</td>

                <td>${x.ndaStatus || '-'}</td>

                <td>${x.isLeader ? 'Yes' : 'No'}</td>

                <td>${ndaDate}</td>

                <td>${x.note || '-'}</td>
            </tr>
        `;
    });

    html += `
            </tbody>
        </table>
    `;

    container.html(html);
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
                .addClass("table-success");

            $("#assignment_row_" + ministryUserId)
                .find("button")
                .remove();
        }

    } catch (e) {
        console.error(e);
    }
};