(function (window) {
    'use strict';
    window.getTeamMembers = function (tableId) {
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
                console.error('Table not founded', tableId);
                return null;
            }

            const $tbody = $table.find('tbody');
            const $rows = $tbody.find('tr[data-selected-id]');

            $rows.each(function () {
                const $row = $(this);
                const memberId = $row.data('selected-id');

                if (!memberId) return;

                const memberName = $row.find('td:eq(1) h6').text().trim();
                const memberPosition = $row.find('td:eq(2) h6').first().text().trim();

                const $scopeSelect = $row.find('.multiCheckSelect-dynamic');
                const selectedScopes = $scopeSelect.val() || [];
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
                    scopes: selectedScopes.map(s => parseInt(s)),
                    scopeNames: scopeNames,
                    isLeader: isLeader
                });
            });

            teamData.totalMembers = teamData.members.length;
            return teamData;

        } catch (error) {
            console.error('❌ خطأ في استخراج بيانات الفريق:', error);
            return null;
        }
    };

    window.getTeamMembers = function (tableId) {
        const data = window.getTeamMembers(tableId);
        return data ? data.members : [];
    };
    window.getTeamLeader = function (tableId) {
        const data = window.getTeamMembers(tableId);

        if (!data) return null;

        const leader = data.members.find(m => m.isLeader);
        return leader || null;
    };
})(window);