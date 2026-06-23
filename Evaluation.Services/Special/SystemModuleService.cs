using Evaluation.DAL.Models.DepartementEntites;
using Evaluation.Services.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Evaluation.Services.Special;

public class SystemModuleService: ISystemModuleService
{
    private readonly IServiceScopeFactory serviceScopeFactory;

    public SystemModuleService(IServiceScopeFactory serviceScopeFactory)
    {
        this.serviceScopeFactory = serviceScopeFactory;
    }

    public async Task<string?> GetBackendNameOfSystemModule(Guid systemModuleId)
    {
        using var scope = serviceScopeFactory.CreateScope();
        var uow = scope.ServiceProvider.CreateScopedUow();
        return await uow.GetRepository<SystemModule>()
                        .GetAllActiveNonDeleted(x => x.Id == systemModuleId)
                        .Select(x => x.BackendName)
                        .FirstOrDefaultAsync();
    }
}
