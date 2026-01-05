(function (window) {
    'use strict';

    // ================= HELPER FUNCTIONS =================
    function getTableIdFromFieldId(fieldId) {
        return `${fieldId}_selectedTeamTable`;
    }

    /**
     * استخراج بيانات NDA من الخلية
     */
    function extractNDAStatus($row, hasNDAColumn) {
        if (!hasNDAColumn) {
            return null;
        }

        const $ndaCell = $row.find('td').eq(2); // العمود الثالث
        const $statusElement = $ndaCell.find('h6').first();

        // إذا كان هناك محتوى NDA
        if ($statusElement.length && $statusElement.text().trim() !== '') {
            return null; // أو قيمة Guid إذا كانت متوفرة
        }

        return null;
    }

    /**
     * التحقق من وجود عمود NDA في الجدول
     */
    function hasNDAColumn($table) {
        const $headers = $table.find('thead th');
        let hasNDA = false;

        $headers.each(function () {
            if ($(this).text().trim() === 'NDA') {
                hasNDA = true;
                return false;
            }
        });

        return hasNDA;
    }
    // ================= VALIDATION =================
    function validateTeamData(teamData) {
        const errors = [];

        if (!teamData || teamData.length === 0) {
            errors.push('لا يوجد أعضاء في الفريق');
            return { isValid: false, errors };
        }

        // التحقق من وجود قائد واحد فقط
        const leaders = teamData.filter(m => m.IsLeader);
        if (leaders.length === 0) {
            errors.push('يجب تحديد قائد للفريق');
        } else if (leaders.length > 1) {
            errors.push('يجب تحديد قائد واحد فقط للفريق');
        }

        // التحقق من البيانات المطلوبة لكل عضو
        teamData.forEach((member, index) => {
            if (!member.UserId) {
                errors.push(`العضو رقم ${index + 1}: معرف المستخدم مطلوب`);
            }
            if (!member.PartyTypeId) {
                errors.push(`العضو رقم ${index + 1}: نوع الطرف مطلوب`);
            }
            if (!member.Scopes || member.Scopes.length === 0) {
                errors.push(`العضو رقم ${index + 1}: يجب تحديد مجال واحد على الأقل`);
            }
        });

        return {
            isValid: errors.length === 0,
            errors: errors
        };
    }

    // ================= PUBLIC API =================

    /**
     * الحصول على بيانات الفريق بصيغة API باستخدام fieldId
     */
    window.getTeamDataByFieldId = function (fieldId, evaluationRequestId = null) {
        const tableId = getTableIdFromFieldId(fieldId);
        try {
            const $table = $(`#${tableId}`);

            if (!$table.length) {
                console.error('❌ الجدول غير موجود:', tableId);
                return null;
            }

            // التحقق من وجود عمود NDA
            const hasNDA = hasNDAColumn($table);

            // الحصول على جميع صفوف الأعضاء
            const $rows = $table.find('tbody tr[data-selected-id]');

            if ($rows.length === 0) {
                console.warn('⚠️ لا يوجد أعضاء في الجدول');
                return [];
            }

            const teamMembers = [];

            // استخراج بيانات كل عضو
            $rows.each(function () {
                const $row = $(this);

                // استخراج البيانات الأساسية
                const userId = $row.data('selected-id');

                // استخراج PartyTypeId
                const $partyTypeSelect = $row.find('.party-type-select');
                const partyTypeId = $partyTypeSelect.val();

                if (!partyTypeId) {
                    console.warn('⚠️ عضو بدون نوع طرف:', userId);
                    return; // skip this member
                }

                // استخراج Scopes
                const $scopeSelect = $row.find('.multiCheckSelect-dynamic');
                const selectedScopes = ($scopeSelect.val() || []).map(scopeId => ({
                    Id: scopeId
                }));
                const $leaderRadio = $row.find('.team-leader-radio');
                const isLeader = $leaderRadio.is(':checked');

                const ndaStatusId = extractNDAStatus($row, hasNDA);

                const memberDto = {
                    Id: null,
                    UserId: userId,
                    EvaluationRequestId: evaluationRequestId,
                    PartyTypeId: partyTypeId,
                    IsLeader: isLeader,
                    IsNDA: hasNDA && ndaStatusId !== null,
                    Note: null,
                    NdaStatusId: ndaStatusId,
                    Scopes: selectedScopes.length > 0 ? selectedScopes : null
                };

                teamMembers.push(memberDto);
            });
            return teamMembers;

        } catch (error) {
            console.error('❌ خطأ في استخراج بيانات الفريق:', error);
            return null;
        }
    };

    /**
     * التحقق من صحة البيانات
     */
    window.validateTeamByFieldId = function (fieldId) {
        const data = window.getTeamDataByFieldId(fieldId);
        return validateTeamData(data);
    };

    /**
     * الحصول على البيانات مع التحقق
     */
    window.getValidatedTeamData = function (fieldId, evaluationRequestId = null) {
        const data = window.getTeamDataByFieldId(fieldId, evaluationRequestId);
        const validation = validateTeamData(data);

        if (!validation.isValid) {
            console.error('❌ أخطاء في بيانات الفريق:', validation.errors);
            return null;
        }

        return data;
    };
    async function submitEvalRequestAssignment(data) {
        const submitBtn = $('#btn-submit');
        submitBtn.prop('disabled', true).text('Saving...');
        let endpoint = API_ENDPOINTS.SUBMIT_EVALUATION_REQUEST_ASSIGNMENT;
        const result = await jqClient().Post(endpoint, data);
        if (result.success) {
            const modal = bootstrap.Modal.getInstance(document.getElementById('confirmation-modal'));
            if (modal) modal.hide();

            alert('Plan saved successfully');
        } else {
            throw new Error(result.message || 'Failed to save the plan');
        }
    }

})(window);