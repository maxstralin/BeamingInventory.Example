using System;

namespace BeamingInventory.Example.Presentation.App
{
    public static class PromptService
    {
        public static bool BooleanPrompt(string prompt)
        {
            Console.WriteLine($"{prompt} Yes (Press Y), No (Press N)");
            var response = char.ToUpper(Console.ReadKey().KeyChar);
            return response == 'Y';
        }
    }
}
