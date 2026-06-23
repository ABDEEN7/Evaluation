using AutoMapper;
using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.FormBuilder;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using Evaluation.SharedHelper.Dtos.Form;

namespace Evaluation.DAL.Helper;
public static class ScopeTreeBuilder
{
    public static List<ScopeTreeDto> BuildTree(IMapper mapper,
        List<ScopeAcademicYear> academicYearScopes,
        List<FormItem> formItems,
        List<FormItemValue>? formItemsValues = null)
    {
        // 1. Scope lookup
        var scopeLookup = academicYearScopes
            .Where(x => x.Scope != null)
            .Select(x => x.Scope!)
            .DistinctBy(x => x.Id)
            .ToDictionary(x => x.Id);

        // 2. Children lookup
        var childrenLookup = academicYearScopes
            .Where(x => x.Scope != null)
            .GroupBy(x => x.ScopeParentId ?? Guid.Empty)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Scope!).ToList());

        // 3. Items lookup
        var itemsLookup = formItems
            .GroupBy(x => x.ScopeId)
            .ToDictionary(g => g.Key, g => g.ToList());

        // 4. Config lookup
        var formItemConfigs = formItems
            .Where(x => x.FormItemConfigs != null)
            .SelectMany(x => x.FormItemConfigs!)
            .ToList();

        var configLookup = formItemConfigs
            .Where(x => x.FormItemId.HasValue)
            .GroupBy(x => x.FormItemId!.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        // 5. Form item values lookup
        var valueLookup = formItemsValues?
            .GroupBy(x => x.FormItemId)
            .ToDictionary(g => g.Key, g => g.First());

        // 6. Root scopes
        var rootScopes = academicYearScopes
            .Where(x =>
                x.Scope != null &&
                (x.ScopeParentId == Guid.Empty ||
                 !scopeLookup.ContainsKey(x.ScopeParentId ?? Guid.Empty)))
            .Select(x => x.Scope!)
            .DistinctBy(x => x.Id)
            .OrderBy(x => x.OrderNo)
            .ToList();

        // 7. Build tree
        return rootScopes
            .Select(root => BuildNode(
                root.Id,
                scopeLookup,
                childrenLookup,
                itemsLookup,
                configLookup,
                valueLookup))
            .ToList();
    }

    private static ScopeTreeDto BuildNode(
        Guid scopeId,
        Dictionary<Guid, Scope> scopeLookup,
        Dictionary<Guid, List<Scope>> childrenLookup,
        Dictionary<Guid, List<FormItem>> itemsLookup,
        Dictionary<Guid, List<FormItemConfig>> configLookup,
        Dictionary<Guid, FormItemValue>? valueLookup)
    {
        var scope = scopeLookup[scopeId];
        var scopeType = scope.ScopeType!;

        return new ScopeTreeDto
        {
            Id = scope.Id,
            Name = scope.NameEn,//TODO: Need to translate
            ScopeTypeId = scopeType.Id,
            ScopeTypeName = scopeType.NameEn,//TODO: Need to translate
            OrderNo = scope.OrderNo,
            ColorCode = scope.ColorCode,

            Items = itemsLookup.TryGetValue(scope.Id, out var items)
                ? items
                    .OrderBy(x => x.OrderNo)
                    .Select(x =>
                    {
                        FormItemValue? currentItemValue = null;

                        valueLookup?.TryGetValue(
                            x.Id,
                            out currentItemValue);

                        return new FormItemDto
                        {
                            Id = x.Id,
                            Name = x.NameEn,//TODO: Need to translate
                            OrderNo = x.OrderNo,
                            HasNote = x.HasNote,
                            NoteRequired = x.NoteRequired,
                            HasMultiEvaluation = x.HasMuliEvaluation,

                            // Current item value
                            Value = currentItemValue?.ActualValue,

                            // Related items
                            RelatedItems = x.RelatedFrom?
                                .Where(r => r.RelatedItemId != Guid.Empty)
                                .Select(r =>
                                {
                                    FormItemValue? relatedValue = null;
                                    valueLookup?.TryGetValue(
                                        r.RelatedItemId,
                                        out relatedValue);

                                    return new RelatedItemDto
                                    {
                                        Id = r.RelatedItemId,
                                        Name = r.RelatedItem?.NameEn,//TODO: Need to translate
                                        Note = relatedValue?.Note,
                                        Value = relatedValue?.ActualValue?.ToString()
                                    };
                                })
                                .ToList()
                                ?? new List<RelatedItemDto>(),

                            // Sub items
                            SubFormItems = x.SubFormItems?
                                .Select(c => new SubFormItemDto
                                {
                                    Id = c.Id,
                                    Name = c.NameEn,//TODO: Need to translate
                                    hasNote = c.HasNote,

                                    SubItemLists = c.DropDownType != null
                                        ? c.DropDownType.FieldDropDownValues
                                            .Select(d => new SubItemList
                                            {
                                                Id = d.Id,
                                                NameAr = d.TitleAr,
                                                NameEn = d.TitleEn,
                                                OrderNo = d.OrderNo
                                            })
                                            .ToList()
                                        : new List<SubItemList>()
                                })
                                .ToList(),

                            // Configs
                            FormItemConfigs = configLookup.TryGetValue(
                                x.Id,
                                out var configs)
                                ? configs.Select(c => new FormItemConfigDto
                                {
                                    Id = c.Id,
                                    FormItemConfig_EvalFormId = c.EvalFormId,
                                    FormItemConfig_FormItemIds =
                                        c.FormItemId.HasValue
                                            ? new List<Guid>
                                            {
                                                c.FormItemId.Value
                                            }
                                            : null,
                                    FormItemConfig_PartyTypeId = c.PartyTypeId,
                                    FormItemConfig_NameAr = c.NameAr,
                                    FormItemConfig_NameEn = c.NameEn,
                                    FormItemConfig_CalcMethodId =
                                        c.CalcMethodId,
                                    FormItemConfig_Percentage =
                                        c.WeightPercentage
                                })
                                .ToList()
                                : new List<FormItemConfigDto>()
                        };
                    })
                    .ToList()
                : new List<FormItemDto>(),

            Children = childrenLookup.TryGetValue(scopeId, out var children)
                ? children
                    .OrderBy(x => x.OrderNo)
                    .Select(x => BuildNode(
                        x.Id,
                        scopeLookup,
                        childrenLookup,
                        itemsLookup,
                        configLookup,
                        valueLookup))
                    .ToList()
                : new List<ScopeTreeDto>()
        };
    }
}