using Evaluation.SharedHelper.Dtos.TeamMemberDto;
using Evaluation.SharedHelper.Enums;
using Evaluation.SharedHelper.Models;
using Microsoft.AspNetCore.Mvc;

namespace Evaluation.SharedHelper.Validations;

public class CreateAssignmentsValidator
{
    public static ValidationResult Validate(Guid evaluationRequestId, List<EvalTeamRequestDto> model)
    {
        // ===================== Request Level =====================
        var result = new ValidationResult();

        if (evaluationRequestId == Guid.Empty)
            result.Add(ConstantKeys.ExceptionMessage.Requiredfield);

        if (model == null || !model.Any())
        {
            result.Add(ConstantKeys.ExceptionMessage.InvalidAssignment);
            return result;
        }
        // ===================== One Leader Only =====================
        if (model.Count(x => x.IsLeader) > 1)
            result.Add(ConstantKeys.ExceptionMessage.OneLeader);
        // ===================== Team Members =====================

        for (int i = 0; i < model.Count; i++)
        {
            ValidateTeamMember(model[i], result, i);
        }

        return result;
    }
    private static void ValidateTeamMember(
      EvalTeamRequestDto member,
      ValidationResult result,
      int index)
    {
        var prefix = $"TeamMembers[{index}]";

        if (member.UserId == Guid.Empty)
            result.Add($"{prefix}: User is required.");
        // ===================== Scopes =====================
        if (member.Scopes == null || !member.Scopes.Any())
        {
            result.Add($"{prefix}: At least one scope is required.");
        }
        else
        {
            for (int j = 0; j < member.Scopes.Count; j++)
            {
                if (member.Scopes[j].Id == Guid.Empty)
                {
                    result.Add($"{prefix}: Scope[{j}] is required.");
                }
            }
        }
    }
}
