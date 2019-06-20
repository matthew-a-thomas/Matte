namespace Matte
{
    using System;

    static class ConsoleHelpers
    {
        public static string Prompt(
            string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        
        public static bool TryPrompt(
            string prompt,
            out int i)
        {
            var line = Prompt(prompt);
            return int.TryParse(
                line,
                out i);
        }
    }
}