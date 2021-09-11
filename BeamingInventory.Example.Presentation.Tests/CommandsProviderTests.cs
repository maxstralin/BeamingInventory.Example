using System.Linq;
using System.Threading.Tasks;
using BeamingInventory.Example.Presentation.App;
using Xunit;

namespace BeamingInventory.Example.Presentation.Tests
{
    public class CommandsProviderTests
    {
        [Fact]
        public async Task AllCommandsDefined()
        {
            var expected = new[] {'S', 'I', 'L'}.OrderBy(a => a);
            var provider = new CommandsProvider();

            var commands = await provider.GetAsync();
            var actual = commands.Select(a => a.CommandChar).OrderBy(a => a);

            Assert.Equal(expected, actual);
        }
        
    }
}
