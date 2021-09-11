using System.Threading.Tasks;

namespace BeamingInventory.Example.Presentation.Entities.Interfaces
{
    public interface ICommandService
    {
        Task<ApiResponse> PerformAsync(CommandType commandType, string? param);
    }
}