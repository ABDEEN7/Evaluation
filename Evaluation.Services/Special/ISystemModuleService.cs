namespace Evaluation.Services.Special;

public interface ISystemModuleService
{
    Task<string?> GetBackendNameOfSystemModule(Guid systemModuleId);
}
