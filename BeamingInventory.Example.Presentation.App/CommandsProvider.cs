using System.Collections.Generic;
using System.Threading.Tasks;
using BeamingInventory.Example.Presentation.Entities;
using BeamingInventory.Example.Presentation.Entities.Interfaces;

namespace BeamingInventory.Example.Presentation.App
{
    //This could fetch from db or somewhere other, hardcoded for now. We might imagine it to cache them as well
    public class CommandsProvider : ICommandsProvider
    {
        private readonly IEnumerable<CommandType> _commands = new List<CommandType>
        {
            new CommandType('I', typeof(int))
                {Description = "Add to inventory. I followed by a number, e.g I3: add three to inventory"},
            new CommandType('S', typeof(int))
            {
                Description = "S (e.g. S3): Sell from inventory, S followed by a number. E.g. sell three from inventory"
            },
            new CommandType('L') {Description = "L: Show how many are in inventory."}
        };

        public Task<IEnumerable<CommandType>> GetAsync() => Task.FromResult(_commands);
    }
}
