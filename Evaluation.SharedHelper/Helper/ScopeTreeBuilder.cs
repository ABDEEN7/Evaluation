using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Dtos.Form;

namespace Evaluation.DAL.Helper;
public static class ScopeTreeBuilder
{
    public static List<ScopeTreeDto> BuildTree(
        List<ScopeAcademicYear> academicYearScopes,
        List<FormItem> formItems, List<FormItemValue>? formItemsValues = null)
    {
        // 1. Scope lookup
        var scopeLookup = academicYearScopes
            .Where(x => x.Scope != null)
            .Select(x => x.Scope!)
            .DistinctBy(x => x.Id)
            .ToDictionary(x => x.Id);

        // 2. Children lookup (fast hierarchy)
        var childrenLookup = academicYearScopes
            .Where(x => x.Scope != null)
            .GroupBy(x => x.ScopeParentId ?? Guid.Empty)
            .ToDictionary(g => g.Key, g => g.Select(x => x.Scope!).ToList());

        // 3. Items lookup
        var itemsLookup = formItems
            .GroupBy(x => x.ScopeId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // 4. Configs lookup (INSIDE METHOD NOW)
        var formItemConfigs = formItems
            .Where(x => x.FormItemConfigs != null)
            .SelectMany(x => x.FormItemConfigs!)
            .ToList();

        var configLookup = formItemConfigs
            .Where(x => x.FormItemId.HasValue)
            .GroupBy(x => x.FormItemId!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        // 5. Root nodes
        var rootScopes = academicYearScopes
            .Where(x =>
                x.Scope != null &&
                (x.ScopeParentId == Guid.Empty ||
                 !scopeLookup.ContainsKey(x.ScopeParentId ?? Guid.Empty)))
            .Select(x => x.Scope!)
            .DistinctBy(x => x.Id)
            .OrderBy(x => x.OrderNo)
            .ToList();

        // 6. Build tree
        return rootScopes
            .Select(root => BuildNode(
                root.Id,
                scopeLookup,
                childrenLookup,
                itemsLookup,
                configLookup,
                formItemsValues))
            .ToList();
    }

    private static ScopeTreeDto BuildNode(
        Guid scopeId,
        Dictionary<Guid, Scope> scopeLookup,
        Dictionary<Guid, List<Scope>> childrenLookup,
        Dictionary<Guid, List<FormItem>> itemsLookup,
        Dictionary<Guid, List<FormItemConfig>> configLookup,
        List<FormItemValue>? formItemsValues)
    {
        var scope = scopeLookup[scopeId];
        var scopeType = scope.ScopeType!;

        return new ScopeTreeDto
        {
            Id = scope.Id,
            Name = scope.NameEn,
            ScopeTypeId = scopeType.Id,
            ScopeTypeName = scopeType.NameEn,
            OrderNo = scope.OrderNo,
            ColorCode = scope.ColorCode,

            Items = itemsLookup.TryGetValue(scope.Id, out var items)
                ? items.OrderBy(x => x.OrderNo)
                    .Select(x => new FormItemDto
                    {
                        Id = x.Id,
                        Name = x.NameEn,
                        OrderNo = x.OrderNo,
                        HasNote = x.HasNote,
                        NoteRequired = x.NoteRequired,
                        HasMultiEvaluation = x.HasMuliEvaluation,
                        Value = formItemsValues?.Where(iv=>iv.FormItemId == x.Id).Select(iv=>iv.ActualValue).FirstOrDefault(),// Just For analysis
                        SubFormItems = x.SubFormItems?.Select(c => new SubFormItemDto
                        {
                            Id = c.Id,
                            Name = c.NameEn,//TODO: need to fix for selected language
                            hasNote = c.HasNote,
                            SubItemLists = c.DropDownType != null ? c.DropDownType.FieldDropDownValues.Select(d=> new SubItemList {
                            Id = d.Id,
                            NameAr = d.TitleAr,
                            NameEn = d.TitleEn,
                            OrderNo = d.OrderNo
                            }).ToList() : new List<SubItemList>()

                        }).ToList(),

                        FormItemConfigs = configLookup.TryGetValue(x.Id, out var configs)
                            ? configs.Select(c => new FormItemConfigDto
                            {
                                Id = c.Id,
                                FormItemConfig_EvalFormId = c.EvalFormId,
                                FormItemConfig_FormItemIds = c.FormItemId.HasValue
                                    ? new List<Guid> { c.FormItemId.Value }
                                    : null,
                                FormItemConfig_PartyTypeId = c.PartyTypeId,
                                FormItemConfig_NameAr = c.NameAr,
                                FormItemConfig_NameEn = c.NameEn,
                                FormItemConfig_CalcMethodId = c.CalcMethodId,
                                FormItemConfig_Percentage = c.WeightPercentage
                            }).ToList()
                            : new List<FormItemConfigDto>()
                    })
                    .ToList()
                : new List<FormItemDto>(),

            Children = childrenLookup.TryGetValue(scopeId, out var children)
                ? children.OrderBy(x => x.OrderNo)
                    .Select(x => BuildNode(
                        x.Id,
                        scopeLookup,
                        childrenLookup,
                        itemsLookup,
                        configLookup,
                        formItemsValues))
                    .ToList()
                : new List<ScopeTreeDto>()
        };
    }
}