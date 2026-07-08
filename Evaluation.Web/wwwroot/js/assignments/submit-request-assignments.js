(function (window) {
    'use strict';

    /**
     * Localization helper - resolves UI control text with fallback
     * @param {string} key - UI control key
     * @param {string} [fallback=''] - Fallback text if key not found
     * @returns {string} Resolved text or the key itself if not found
     */
    function t(key, fallback = '') {
        const text = uiControlsSetup()?.GetUiControlText(key);
        return text || key;
    }

    // ================= HELPER FUNCTIONS =================
    function getTableIdFromFieldId(fieldId) {
        return `${fieldId}_selectedTeamTable`;
    }


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
            errors.push(t('lblNoTeamMembers'));
            return { isValid: false, errors };
        }

        const leaders = teamData.filter(m => m.IsLeader);
        if (leaders.length === 0) {
            errors.push(t('lblLeaderRequired'));
        } else if (leaders.length > 1) {
            errors.push(t('lblOnlyOneLeaderAllowed'));
        }

        teamData.forEach((member, index) => {
            
            if (!member.UserId) {
                errors.push(`${t('lblMember')} ${index + 1}: ${t('lblUserIdRequired')}`);
            }
            if (!member.PartyTypeId) {
                errors.push(`${t('lblMember')} ${index + 1}: ${t('lblPartyTypeRequired')}`);
            }
            
            if (!member.Scopes || member.Scopes.length === 0) {
                errors.push(`${t('lblMember')} ${index + 1}: ${t('lblAtLeastOneScopeRequired')}`);
            }
        });

        return {
            isValid: errors.length === 0,
            errors: errors
        };
    }

    // ================= PUBLIC API =================

    /**
     * fieldId
     * Works with Select2 multi-select
     */
    window.getAssignmentsDataByFieldId = function (fieldId) {
        const tableId = getTableIdFromFieldId(fieldId);
        try {
            const $table = $(`#${tableId}`);

            if (!$table.length) {
                console.error('❌ الجدول غير موجود:', tableId);
                return null;
            }

            //checkNDA
            const hasNDA = hasNDAColumn($table);

            //get all rows
            const $rows = $table.find('tbody tr[data-selected-id]');

            if ($rows.length === 0) {
                console.warn('⚠️ لا يوجد أعضاء في الجدول');
                return [];
            }

            const assignments = [];

            //extract every users
            $rows.each(function () {
                const $row = $(this);

                //extract data
                const ministryUserId = $row.data('selected-id');

                // extract PartyTypeId
                const $partyTypeSelect = $row.find('.party-type-select');
                const partyTypeId = $partyTypeSelect.val();

                if (!partyTypeId) {
                    console.warn('⚠️ عضو بدون نوع طرف:', ministryUserId);
                    return; // skip this member
                }

                // استخراج Scopes من Select2
                const $scopeSelect = $row.find('.multiCheckSelect-dynamic');
                const selectedScopes = $scopeSelect.val() || [];

                const evalRequestAssignmentScopies = selectedScopes.map(scopeId => ({
                    Id: String(scopeId)
                }));

                // استخراج قائد الفريق
                const $leaderRadio = $row.find('.team-leader-radio');
                const isLeader = $leaderRadio.is(':checked');
                const isNDA = hasNDA;

                const memberDto = {
                    UserId: ministryUserId,
                    PartyTypeId: partyTypeId,
                    IsLeader: isLeader,
                    //IsNDA: isNDA,
                    Scopes: evalRequestAssignmentScopies.length > 0 ? evalRequestAssignmentScopies : null
                };

                assignments.push(memberDto);
            });

            return assignments;

        } catch (error) {
            console.error('❌ خطأ في استخراج بيانات الفريق:', error);
            return null;
        }
    };

    
    window.validateTeamByFieldId = function (fieldId) {
        const data = window.getAssignmentsDataByFieldId(fieldId);
        return validateTeamData(data);
    };

    
    window.getValidatedTeamData = function (fieldId) {
        const data = window.getAssignmentsDataByFieldId(fieldId);
        const validation = validateTeamData(data);

        if (!validation.isValid) {
            console.error('❌ أخطاء في بيانات الفريق:', validation.errors);
            alert(t('lblFixFollowingErrors') + '\n' + validation.errors.join('\n'));
            return null;
        }

        return data;
    };

    
    async function submitEvalRequestAssignment(data) {
        const submitBtn = $('#btn-submit');
        submitBtn.prop('disabled', true).text(t('lblSaving'));

        try {
            let endpoint = API_ENDPOINTS.SUBMIT_EVALUATION_REQUEST_ASSIGNMENT;
            const result = await jqClient().Post(endpoint, data);

            if (result.success || result.isSuccess) {
                const modal = bootstrap.Modal.getInstance(document.getElementById('confirmation-modal'));
                if (modal) modal.hide();
                return result;
            }
            else {
                throw new Error(result.message || t('lblFailedToSaveTeam'));
            }
        } catch (error) {
            console.error('❌ خطأ في حفظ الفريق:', error);
            alert(t('lblSaveTeamError'));
            throw error;
        } finally {
            submitBtn.prop('disabled', false).text(t('lblSave'));
        }
    }

    
    window.saveTeamByFieldId = async function (fieldId, evaluationRequestId = null) {
        try {
            const teamData = window.getValidatedTeamData(fieldId, evaluationRequestId);

            if (!teamData) {
                return false;
            }

            const result = await submitEvalRequestAssignment(teamData);

            return result.success || result.isSuccess;
        } catch (error) {
            console.error('❌ خطأ في حفظ الفريق:', error);
            return false;
        }
    };

    
    window.deleteEvalRequestAssignment = async function (fieldId, evalRequestAssignmentId) {
        if (!evalRequestAssignmentId) {
            console.error('❌ معرف التعيين مطلوب');
            return false;
        }

        if (!confirm(t('lblConfirmDeleteAssignment'))) {
            return false;
        }

        try {
            const endpoint = `${API_ENDPOINTS.DELETE_EVALUATION_REQUEST_ASSIGNMENT}/${evalRequestAssignmentId}`;
            const result = await jqClient().Delete(endpoint);

            if (result.success || result.isSuccess) {
                alert(t('lblAssignmentDeletedSuccessfully'));
                return true;
            } else {
                throw new Error(result.message || t('lblFailedToDeleteAssignment'));
            }
        } catch (error) {
            console.error('❌ خطأ في حذف التعيين:', error);
            alert(t('lblDeleteAssignmentError'));
            return false;
        }
    };

    // ================= UTILITY FUNCTIONS =================

    window.hasTeamChanges = function (fieldId) {
        if (!window.assignmentsLogic) {
            return false;
        }

        const state = window.assignmentsLogic.getState();
        if (!state) {
            return false;
        }

        const currentData = window.getAssignmentsDataByFieldId(fieldId);
        const originalData = state.existingAssignments;

        if (!originalData || originalData.length === 0) {
            return currentData && currentData.length > 0;
        }

        return JSON.stringify(currentData) !== JSON.stringify(originalData);
    };

    window.getTeamChangeSummary = function (fieldId) {
        const state = window.assignmentsLogic?.getState();
        if (!state) {
            return null;
        }

        const currentData = window.getAssignmentsDataByFieldId(fieldId);
        const originalData = state.existingAssignments || [];

        const summary = {
            added: [],
            removed: [],
            modified: []
        };

        currentData.forEach(current => {
            const exists = originalData.find(orig =>
                orig.ministryUserId === current.MinistryUserId &&
                orig.partyTypeId === current.PartyTypeId
            );

            if (!exists) {
                summary.added.push(current);
            }
        });

        originalData.forEach(orig => {
            const exists = currentData.find(current =>
                current.MinistryUserId === orig.ministryUserId &&
                current.PartyTypeId === orig.partyTypeId
            );

            if (!exists) {
                summary.removed.push(orig);
            }
        });

        currentData.forEach(current => {
            const original = originalData.find(orig =>
                orig.ministryUserId === current.MinistryUserId &&
                orig.partyTypeId === current.PartyTypeId
            );

            if (original && JSON.stringify(original) !== JSON.stringify(current)) {
                summary.modified.push({ original, current });
            }
        });

        return summary;
    };

    // ================= DEBUGGING =================

    window.debugTeamData = function (fieldId) {
        const data = window.getAssignmentsDataByFieldId(fieldId);

        const validation = validateTeamData(data);
        console.log('Validation:', validation);

        return data;
    };

})(window);