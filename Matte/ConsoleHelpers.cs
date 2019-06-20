namespace Matte
{
    using System;
    using System.Text;

    static class ConsoleHelpers
    {
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