using System.Linq;
using System.Threading.Tasks;
using BeamingInventory.Example.Presentation.Entities;
using BeamingInventory.Example.Presentation.Entities.Interfaces;
using Microsoft.Extensions.Logging;

namespace BeamingInventory.Example.Presentation.App
{
    public class InputHandler : IInputHandler
    {
        private readonly ICommandsProvider _commandsProvider;
        private readonly ICommandService _commandService;
        private readonly ILogger _logger;

        public InputHandler(ICommandsProvider commandsProvider, ICommandService commandService, ILogger logger)
        {
            _commandsProvider = commandsProvider;
            _commandService = commandService;
            _logger = logger;
        }

        public async Task<string> ProcessInputAsync(string? input)
        {
            if (string.IsNullOrEmpty(input)) return "I'm afraid you didn't enter a command, please try again.";

            //In this case, we know that the implementation of _commandsProvider caches the commands.
            //Otherwise, it'd make sense to cache it on our end.
            var commands = (await _commandsProvider.GetAsync()).ToList();

            //This could be cached instead of selected on each input but OK for this scenario.
            var commandStrings = commands.Select(a => a.CommandChar);

            //First character of the string is the command, e.g. 'S'
            var commandString = input[0];
            //We'll find a command corresponding to the key input, regardless of case (see more in comments below)
            var command = commands.SingleOrDefault(a => a.CommandChar == char.ToUpper(commandString));

            //Let's check if it's a valid command but they seem to have entered it erroneously (i.e. lowercase)
            //This example assumes we only have uppercase commands and there's no risk for 'S' and 's' to be used together as commands.
            if (command != null && char.IsLower(commandString))
            {
                var uppercaseCommand = char.ToUpper(commandString);
                _logger.LogDebug($"User possibly entered {commandString} instead of {uppercaseCommand}");
                var wantedUppercase =
                    PromptService.BooleanPrompt($"You entered {commandString}, did you mean {uppercaseCommand}?");
                if (!wantedUppercase) return "Okay, please try again.";
            }
            //We couldn't find a valid command
            else if (command == null)
            {
                _logger.LogDebug($"Unknown command entered: {commandString}");
                return $"I'm afraid I don't know how to process {commandString}, try again";
            }

            //Remaining part of the input (if any) 
            var param = input[..1];

            var result = await _commandService.PerformAsync(command, param);

            return $"{(result.Successful ? "Successful" : "Unsuccessful")}: {result.Message ?? "No details provided"}";
        }

    }
}
