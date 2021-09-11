using System.Collections.Generic;
using System.Threading.Tasks;

namespace BeamingInventory.Example.Presentation.Entities.Interfaces
{
    public interface ICommandsProvider
    {
        /// <summary>
        /// Returns the available input commands
        /// </summary>
        /// <returns>List of <see cref="CommandType"/></returns>
        Task<IEnumerable<CommandType>> GetAsync();
    }
}