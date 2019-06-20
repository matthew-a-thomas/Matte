namespace Matte
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    static class ConsoleHelpers
    {
        public static int Choose(
            string prompt,
            IReadOnlyCollection<string> choices)
        {
            if (choices.Count == 0)
                return -1;
            prompt = prompt ?? "Choose one:";
            while (true)
            {
                Console.WriteLine(prompt);
                foreach (var (label, index) in choices.Select((x, i) => (x, i)))
                {
                    Console.WriteLine($"\t{index + 1}. {label}");
                }

                if (!TryPrompt(
                    $"[1-{choices.Count}]? ",
                    out var choice))
                    continue;
                if (choice < 1 || choice > choices.Count)
                    continue;
                return choice - 1;
            }
        }
            
        public static string PromptParagraph(
            string prompt,
            string magicWord)
        {
            Console.Write(prompt);
            var stringBuilder = new StringBuilder();
            while (true)
            {
                var line = Console.ReadLine();
                if (line == magicWord)
                    break;
                stringBuilder.AppendLine(line);
            }

            return stringBuilder.ToString();
        }
        
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