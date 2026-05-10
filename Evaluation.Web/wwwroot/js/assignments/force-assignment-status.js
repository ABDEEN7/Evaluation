

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
                    <th>${uiControlsSetup().GetUiControlText('lblNDA')}</th>
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

        const forceButton = !x.isNDA
            ? `
                <button class="btn btn-danger btn-sm"
                        onclick="forceAssignmentStatus('${x.ministryUserId}','${x.evaluationRequestId}')">
                    Force
                </button>
              `
            : "";

        html += `
            <tr id="assignment_row_${x.ministryUserId}">
                
                <td>${x.ministryUser || '-'}</td>

                <td>${x.partyType || '-'}</td>

                <td>${x.ndaStatusId ? 'Yes' : 'No'}</td>

                <td>${x.isNDA ? 'Yes' : 'No'}</td>

                <td>${x.isLeader ? 'Yes' : 'No'}</td>

                <td>${ndaDate}</td>

                <td>${x.note || '-'}</td>

                <td>${forceButton}</td>

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