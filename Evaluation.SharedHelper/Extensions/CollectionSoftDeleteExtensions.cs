using Evaluation.DAL.Models.Generic;

namespace Evaluation.SharedHelper.Extensions;

public static class CollectionSoftDeleteExtensions
{
    public static void SoftDeleteRange<T>(this ICollection<T> entities, List<T> todeletes) where T : class, IEntity
    {
        foreach (var entity in entities)
        {
            if (todeletes.Contains(entity))
                entity.IsDeleted = true;
        }
    }
}
