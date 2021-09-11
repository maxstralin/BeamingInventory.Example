using System;
using System.Threading.Tasks;
using BeamingInventory.Example.Presentation.Entities;
using BeamingInventory.Example.Presentation.Entities.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace BeamingInventory.Example.Presentation.App
{
    internal class Program
    {
        private static async Task Main()
        {
            var services = CreateServiceProvider();
            var commandsProvider = services.GetRequiredService<ICommandsProvider>();
            var inputHandler = services.GetRequiredService<InputHandler>();

            //Fetch commands and display their description, as a help to the user.
            //We assume we know that it's a reasonable number of commands (e.g. not 9000) for this example
            var commands = await commandsProvider.GetAsync();
            foreach (var commandType in commands)
            {
                Console.WriteLine($"{commandType.CommandChar}: {commandType.Description ?? "No description available"}");
            }
            Console.WriteLine("Please enter a command:");

            while (true)
            {
                var input = Console.ReadLine();
                await inputHandler.ProcessInputAsync(input);
            }
        }

        private static IServiceProvider CreateServiceProvider()
        {
            //We know that this is a console app running locally somewhat ephemerally.
            //Presumably, different lifetimes would be relevant for other scenarios (e.g. ASP.NET Core web app)
            var collection = new ServiceCollection();
            collection.AddSingleton<ICommandsProvider, CommandsProvider>()
                .AddSingleton<InputHandler>()
                .AddSingleton<ICommandService, CommandService>()
                .AddHttpClient()
                .AddSingleton<IConfiguration, Configuration>() //Could have been IOptions pattern but that's more for ASP.NET
                .AddLogging();
            return collection.BuildServiceProvider();
        }
    }
}
