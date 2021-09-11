using System.Threading.Tasks;

namespace BeamingInventory.Example.Presentation.Entities.Interfaces
{
    public interface IInputHandler
    {
        Task<string> ProcessInputAsync(string? input);
    }
}