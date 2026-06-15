using Evaluation.DAL.Dtos.Form;
using Evaluation.DAL.Models.FormsModules;
using Evaluation.SharedHelper.Dtos.EvalFormDto;
using System.Linq;


namespace Evaluation.DAL.Helper;
//public static class ScopeTreeBuilder
//{
//    public static List<ScopeTreeNode> BuildTree(
//        List<ScopeAcademicYear> academicYearScopes)
//    {
//        // all scope ids existing in academic year
//        var validScopeIds = academicYearScopes
//            .Select(x => x.ScopeId)
//            .ToHashSet();

//        // roots:
//        // parent not found OR empty
//        var roots = academicYearScopes
//            .Where(x =>
//                x.ScopeParentId == Guid.Empty ||
//                !validScopeIds.Contains(x.ScopeParentId??Guid.Empty))
//            .OrderBy(x => x.Scope?.OrderNo)
//            .ToList();

//        return roots
//            .Select(root => BuildNode(root, academicYearScopes))
//            .ToList();
//    }

//    private static ScopeTreeNode BuildNode(
//        ScopeAcademicYear current,
//        List<ScopeAcademicYear> allScopes)
//    {
//        var scope = current.Scope!;
//        var scopeType = scope.ScopeType!;

//        var node = new ScopeTreeNode
//        {
//            ScopeId = scope.Id,

//            ScopeNameAr = scope.NameAr,
//            ScopeNameEn = scope.NameEn,

//            ScopeTypeId = scopeType.Id,
//            ScopeTypeNameAr = scopeType.NameAr,
//            ScopeTypeNameEn = scopeType.NameEn,

//            OrderNo = scope.OrderNo,
//            ColorCode = scope.ColorCode
//        };

//        //// Attach Items
//        //node.Items = formItems
//        //    .Where(x => x.ScopeId == scope.Id)
//        //    .OrderBy(x => x.OrderNo)
//        //    .Select(x => new FormItemNode
//        //    {
//        //        Id = x.Id,
//        //        NameAr = x.NameAr,
//        //        NameEn = x.NameEn,
//        //        Min = x.Min,
//        //        Max = x.Max,
//        //        Weight = x.Weight,
//        //        OrderNo = x.OrderNo
//        //    })
//        //    .ToList();

//        // Find children
//        var children = allScopes
//            .Where(x => x.ScopeParentId == scope.Id)
//            .OrderBy(x => x.Scope!.OrderNo)
//            .ToList();

//        foreach (var child in children)
//        {
//            node.Children.Add(
//                BuildNode(child, allScopes));
//        }

//        return node;
//    }
//}

//public static class ScopeTreeBuilder
//{
//    public static List<ScopeTreeDto> Build(
//        List<ScopeAcademicYear> academicYearScopes,
//        List<FormItem> formItems)
//    {
//        // 1. Get all scopes from academicYearScopes
//        var scopes = academicYearScopes
//            .Select(x => x.Scope)
//            .Where(x => x != null)
//            .DistinctBy(x => x!.Id)
//            .ToDictionary(x => x!.Id, x => x!);

//        // 2. Build nodes
//        var nodes = scopes.Values.ToDictionary(
//            s => s.Id,
//            s => new ScopeTreeDto
//            {
//                Id = s.Id,
//                Name = s.NameEn,
//                ScopeTypeName = s.ScopeType?.NameEn ?? "",
//                OrderNo = s.OrderNo
//            });

//        // 3. Attach form items to scopes
//        foreach (var item in formItems)
//        {
//            if (!nodes.TryGetValue(item.ScopeId, out var node))
//                continue;

//            node.Items.Add(new FormItemDto
//            {
//                Id = item.Id,
//                Name = item.NameEn,
//                OrderNo = item.OrderNo,
//                HasNote = item.HasNote,
//                NoteRequired = item.NoteRequired,
//                HasMultiEvaluation = item.HasMuliEvaluation
//            });
//        }

//        // 4. Build tree using ScopeParentId (from ScopeAcademicYear)
//        var nodeByScopeId = nodes;
//        var childLookup = academicYearScopes
//            .Where(x => x.Scope != null)
//            .GroupBy(x => x.ScopeParentId??Guid.Empty)
//            .ToDictionary(g => g.Key, g => g.Select(x => x.Scope!).ToList());

//        var rootIds = academicYearScopes
//            .Where(x => x.Scope != null)
//            .Select(x => x.Scope!)
//            .Where(s =>
//                !academicYearScopes.Any(a =>
//                    a.Scope != null && a.Scope.Id == s.Id && nodeByScopeId.ContainsKey(a.ScopeParentId ?? Guid.Empty)))
//            .Select(x => x.Id)
//            .Distinct()
//            .ToList();

//        // safer build: parent-child linking
//        foreach (var sa in academicYearScopes)
//        {
//            if (sa.Scope == null)
//                continue;

//            if (sa.ScopeParentId == Guid.Empty)
//                continue;

//            if (!nodes.ContainsKey(sa.ScopeParentId ?? Guid.Empty) || !nodes.ContainsKey(sa.Scope.Id))
//                continue;

//            nodes[sa.ScopeParentId ?? Guid.Empty].Children.Add(nodes[sa.Scope.Id]);
//        }

//        // 5. Get roots
//        var hasParent = new HashSet<Guid>(
//            academicYearScopes
//                .Where(x => x.Scope != null && x.ScopeParentId != Guid.Empty)
//                .Select(x => x.Scope.Id));

//        var roots = nodes.Values
//            .Where(x => !hasParent.Contains(x.Id))
//            .ToList();

//        // 6. Sort recursively
//        Sort(roots);

//        return roots;
//    }

//    private static void Sort(List<ScopeTreeDto> nodes)
//    {
//        nodes.Sort((a, b) => a.OrderNo.CompareTo(b.OrderNo));

//        foreach (var node in nodes)
//            Sort(node.Children);
//    }
//}


//public static class ScopeTreeBuilder
//{
//    public static List<ScopeTreeDto> BuildTree(
//        List<ScopeAcademicYear> academicYearScopes,
//        List<FormItem> formItems)
//    {
//        // 1. Scope lookup
//        var scopeLookup = academicYearScopes
//            .Where(x => x.Scope != null)
//            .Select(x => x.Scope!)
//            .DistinctBy(x => x.Id)
//            .ToDictionary(x => x.Id);

//        // 2. Items grouped by scope
//        var itemsLookup = formItems
//            .GroupBy(x => x.ScopeId)
//            .ToDictionary(g => g.Key, g => g.ToList());

//        // 3. Root scopes (no valid parent)
//        var rootScopes = academicYearScopes
//            .Where(x =>
//                x.Scope != null &&
//                (x.ScopeParentId == Guid.Empty ||
//                 !scopeLookup.ContainsKey(x.ScopeParentId ?? Guid.Empty)))
//            .Select(x => x.Scope!)
//            .DistinctBy(x => x.Id)
//            .OrderBy(x => x.OrderNo)
//            .ToList();

//        // 4. Build tree
//        return rootScopes
//            .Select(root => BuildNode(
//                root.Id,
//                academicYearScopes,
//                scopeLookup,
//                itemsLookup))
//            .ToList();
//    }

//    private static ScopeTreeDto BuildNode(
//        Guid scopeId,
//        List<ScopeAcademicYear> academicYearScopes,
//        Dictionary<Guid, Scope> scopeLookup,
//        Dictionary<Guid, List<FormItem>> itemsLookup)
//    {
//        var scope = scopeLookup[scopeId];
//        var scopeType = scope.ScopeType!;

//        var node = new ScopeTreeDto
//        {
//            Id = scope.Id,
//            Name = scope.NameEn,
//            ScopeTypeId = scopeType.Id,
//            ScopeTypeName = scopeType.NameEn,
//            OrderNo = scope.OrderNo,
//            ColorCode = scope.ColorCode,

//            // ✅ attach items
//            Items = itemsLookup.TryGetValue(scope.Id, out var items)
//                ? items
//                    .OrderBy(x => x.OrderNo)
//                    .Select(x => new FormItemDto
//                    {
//                        Id = x.Id,
//                        Name = x.NameEn,
//                        OrderNo = x.OrderNo,
//                        HasNote = x.HasNote,
//                        NoteRequired = x.NoteRequired,
//                        HasMultiEvaluation = x.HasMuliEvaluation,
//                        FormItemConfigs

//                    })
//                    .ToList()
//                : new List<FormItemDto>()
//        };

//        // 5. children (ScopeParentId -> Scope.Id)
//        var children = academicYearScopes
//            .Where(x =>
//                x.Scope != null &&
//                x.ScopeParentId == scopeId)
//            .Select(x => x.Scope!)
//            .DistinctBy(x => x.Id)
//            .OrderBy(x => x.OrderNo)
//            .ToList();

//        foreach (var child in children)
//        {
//            node.Children.Add(
//                BuildNode(child.Id, academicYearScopes, scopeLookup, itemsLookup));
//        }

//        return node;
//    }
//}

public static class ScopeTreeBuilder
{
    public static List<ScopeTreeDto> BuildTree(
        List<ScopeAcademicYear> academicYearScopes,
        List<FormItem> formItems)
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
                configLookup))
            .ToList();
    }

    private static ScopeTreeDto BuildNode(
        Guid scopeId,
        Dictionary<Guid, Scope> scopeLookup,
        Dictionary<Guid, List<Scope>> childrenLookup,
        Dictionary<Guid, List<FormItem>> itemsLookup,
        Dictionary<Guid, List<FormItemConfig>> configLookup)
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
                        SubFormItems = x.SubFormItems?.Select(c => new SubFormItemDto
                        {
                            Id = c.Id,
                            Name = c.NameEn,//TODO: need to fix for selected language
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
                        configLookup))
                    .ToList()
                : new List<ScopeTreeDto>()
        };
    }
}
public class ScopeTreeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string ScopeTypeName { get; set; } = null!;
    public int OrderNo { get; set; }
    public Guid ScopeTypeId { get; set; }

    public string? ColorCode { get; set; }
    public List<ScopeTreeDto> Children { get; set; } = new();
    public List<FormItemDto> Items { get; set; } = new();
}
//public class ScopeTreeNode
//{
//    public Guid ScopeId { get; set; }

//    public string ScopeNameAr { get; set; } = "";
//    public string ScopeNameEn { get; set; } = "";

//    public Guid ScopeTypeId { get; set; }
//    public string ScopeTypeNameAr { get; set; } = "";
//    public string ScopeTypeNameEn { get; set; } = "";

//    public int OrderNo { get; set; }
//    public string? ColorCode { get; set; }


//    public List<FormItemDto> Items { get; set; } = new();


//    public List<ScopeTreeNode> Children { get; set; } = new();
//}
