(function (window) {
    'use strict';

    function getTableIdFromFieldId(fieldId) {
        return `${fieldId}_selectedTeamTable`;
    }

    function extractTeamData(tableId) {
        const teamData = {
            members: [],
            leaderId: null,
            leaderName: null,
            totalMembers: 0,
            timestamp: new Date().toISOString(),
            dateFormatted: new Date().toLocaleString('ar-QA', {
                year: 'numeric',
                month: 'long',
                day: 'numeric',
                hour: '2-digit',
                minute: '2-digit'
            })
        };

        try {
            const $table = $(`#${tableId}`);

            if (!$table.length) {
                console.error('❌ Table not found:', tableId);
                return null;
            }

            const $rows = $table.find('tbody tr[data-selected-id]');

            $rows.each(function () {
                const $row = $(this);
                const memberId = $row.data('selected-id');
                if (!memberId) return;

                const memberName = $row.find('td:eq(1) h6').text().trim();
                const memberPosition = $row.find('td:eq(2) h6').first().text().trim();

                const $scopeSelect = $row.find('.multiCheckSelect-dynamic');
                const selectedScopes = ($scopeSelect.val() || []).map(Number);
                const scopeNames = [];

                $scopeSelect.find('option:selected').each(function () {
                    scopeNames.push($(this).text().trim());
                });

                const isLeader = $row.find('.team-leader-radio').is(':checked');

                if (isLeader) {
                    teamData.leaderId = memberId;
                    teamData.leaderName = memberName;
                }

                teamData.members.push({
                    id: memberId,
                    name: memberName,
                    position: memberPosition,
                    scopes: selectedScopes,
                    scopeNames,
                    isLeader
                });
            });

            teamData.totalMembers = teamData.members.length;
            return teamData;

        } catch (error) {
            console.error('❌ خطأ في استخراج بيانات الفريق:', error);
            return null;
        }
    }

    // ================= PUBLIC API =================

    window.getTeamDataByFieldId = function (fieldId) {
        const tableId = getTableIdFromFieldId(fieldId);
        return extractTeamData(tableId);
    };

    window.getTeamMembersByFieldId = function (fieldId) {
        const data = window.getTeamDataByFieldId(fieldId);
        return data ? data.members : [];
    };

    window.getTeamLeaderByFieldId = function (fieldId) {
        const data = window.getTeamDataByFieldId(fieldId);
        return data?.members.find(m => m.isLeader) || null;
    };

})(window);
