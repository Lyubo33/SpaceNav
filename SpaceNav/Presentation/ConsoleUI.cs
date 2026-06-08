using System.Text;
using SpaceNav.Domain.Models;

namespace SpaceNav.Presentation;

/// <summary>
/// This class defines various ways to print messages or prompt the user as well as print out results
/// </summary>
public class ConsoleUI
{
    /// <summary>
    /// Displays a header text, usually used above menu or in certain sections
    /// </summary>
    /// <param name="text"> The text of the header</param>
    public static void DisplayHeader(string text)
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("======================================");
        Console.WriteLine($"     {text.ToUpper()}     ");
        Console.WriteLine("======================================");
        Console.ResetColor();
    }
    /// <summary>
    /// Defines the interaction with a menu by given entries
    /// </summary>
    /// <param name="options"> An array of entries representing the available choices in the menu</param>
    /// <returns>The selected entry</returns>
    public static int PromptMenuSelection(string[] options)
    {
        int currentSelection = 0;
        bool selecting = true;
        while (selecting)
        {
            DisplayHeader(" 󱎃 Space Nav V.2026 󱎃 ");
            Console.WriteLine("Select operational mode using Up/Down arrows, Press ENTER to select:\n");
            int menuStartTop = Console.CursorTop;
            for (int i = 0; i < options.Length; i++)
            {
                if (i == currentSelection)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"    {options[i]} ");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($" {options[i]}");
                }
            }
            Console.WriteLine("\n__________________________________________");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    currentSelection--;
                    if (currentSelection < 0) currentSelection = options.Length - 1;
                    break;
                case ConsoleKey.DownArrow:
                    currentSelection++;
                    if (currentSelection >= options.Length) currentSelection = 0;
                    break;
                case ConsoleKey.Enter:
                    Console.CursorVisible = false;
                    int targetLineTop = menuStartTop + currentSelection;
                    for (int speed = 0; speed < 30; speed++)
                    {
                        Console.SetCursorPosition(0,targetLineTop);
                        Console.Write(new string(' ', speed));
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write($"󱑹󱑹󱑹󰥛󱑼 ");
                        Console.ResetColor();
                        Console.Write(" ");
                        Console.Write(new string(' ', 15));
                        Thread.Sleep(25);
                    }
                    selecting = false;
                    break;
            }
        }
        return currentSelection;
    }
    /// <summary>
    /// Prompts the user to enter a whole number
    /// </summary>
    /// <param name="message">The message of the prompt</param>
    /// <param name="min"> Minimum possible value for the number</param>
    /// <param name="max">Maximum possible value for the number</param>
    /// <returns>The input number</returns>
    public static int PromptInteger(string message, int min, int max)
    {
        while (true)
        {
            Console.Write($"{message} ({min}-{max}): ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out int result) && result >= min && result <= max)
            {
                return result;
            }

            PrintMessage($"[!] Invalid input. Enter a whole number between {min} and {max}.", ConsoleColor.Yellow);
        }
    }
    
    /// <summary>
    /// Prompts the user to enter the map row by row
    /// </summary>
    /// <param name="expectedRows"> How many rows the map expects</param>
    /// <returns><see cref="List"/> of <see cref="string"/> representing the map</returns>
    public static List<string> PromptMapGridLines(int expectedRows)
    {
        Console.WriteLine($"\n[INPUT] Paste or type your entire cosmic map, ({expectedRows}) lines expected");
        Console.WriteLine("Press Enter on a blank line or when you are done finished pasting:");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("________________________________________________");
        Console.ResetColor();
        var lines = new List<string>();

        while (true)
        {
            string? line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }
            lines.Add(line.Trim());
        }
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("__________________________________________________");
        Console.ResetColor();
        return lines;
    }
    /// <summary>
    /// Prints a message
    /// </summary>
    /// <param name="message"> The message to print</param>
    /// <param name="color">The foreground color of the message as a <see cref="ConsoleColor"/></param>
    public static void PrintMessage(string message, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    
    /// <summary>
    /// Prompts the user to press any key to return to main menu
    /// </summary>
    public static void PromptKeyPressToContinue()
    {
        Console.WriteLine("\nPress any key to return to the command menu...");
        Console.ReadKey(true);
    }
    
    /// <summary>
    /// Prompts the user to input some text
    /// </summary>
    /// <param name="message"> The text to input</param>
    /// <returns>The input text itself</returns>
    public static string PromptString(string message)
    {
        while (true)
        {
            Console.Write(message);
            string? input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrEmpty(input))
            {
                return input;
            }
            PrintMessage("[!] Input cannot be empty.Operational params require validation.", ConsoleColor.Yellow);
        }
    }
    /// <summary>
    /// Prompts the user to input a password while it hides what is being input
    /// </summary>
    /// <param name="message">The password</param>
    /// <returns>The input password</returns>
    public static string PromptPassword(string message)
    {
        Console.Write(message);
        StringBuilder passwordBuilder = new StringBuilder();
        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                break;
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (passwordBuilder.Length > 0)
                {
                    passwordBuilder.Remove(passwordBuilder.Length - 1, 1);
                    Console.Write("\b \b");
                }
            }else if (!char.IsControl(keyInfo.KeyChar))
            {
                passwordBuilder.Append(keyInfo.KeyChar);
                Console.Write(" ");
            }
        }

        return passwordBuilder.ToString();
    }
    
    /// <summary>
    /// Outputs the result of a mission
    /// </summary>
    /// <param name="astronauts"> <see cref="List"/> of <see cref="Astronaut"/>s, usually sorted</param>
    /// <param name="map">A <see cref="CosmicMap"/></param>
    /// <returns>The resulting report as a string</returns>
    public static string RenderResult(List<Astronaut> astronauts, CosmicMap map)
    {
        StringBuilder emailBody = new StringBuilder();
        foreach (var a in astronauts)
        {
            if (!a.IsPathFound)
            {
                PrintMessage($"\nMission failed -- Astronaut {a.Id} lost in space!", ConsoleColor.Red);
                emailBody.Append($"\nMission failed -- Astronaut {a.Id} lost in space!");
                continue;
            }
            PrintMessage($"\nAstronaut {a.Id} - Shortest Path: {a.PathCost} steps ",ConsoleColor.Green);
            Console.WriteLine(" ");
            emailBody.Append($"\nAstronaut {a.Id} - Shortest Path: {a.PathCost} steps ");
            emailBody.AppendLine();
            var pathSteps = a.PathSteps?.ToHashSet() ?? new HashSet<Coordinate>();
            for (int r = 0; r < map.Rows; r++)
            {
                Console.Write(" ");
                emailBody.Append(" ");
                for (int c = 0; c < map.Columns; c++)
                {
                    Coordinate current =  new Coordinate(r, c);
                    if (pathSteps.Contains(current) && !map.SpaceStationPosition.Equals(current))
                    {
                        Console.ForegroundColor  = ConsoleColor.Green;
                        Console.Write("* ");
                        emailBody.Append("* ");
                    }
                    else
                    {
                        string symbol = map.Grid[r, c];
                        if (symbol == "X") Console.ForegroundColor = ConsoleColor.Red;
                        else if (symbol == "D") Console.ForegroundColor = ConsoleColor.Magenta;
                        else if (symbol == "F") Console.ForegroundColor = ConsoleColor.Cyan;
                        else if (symbol.StartsWith("S")) Console.ForegroundColor = ConsoleColor.Blue;
                        else Console.ForegroundColor = ConsoleColor.Gray;

                        Console.Write($"{symbol} ");
                        emailBody.Append($"{symbol} ");
                    }
                }
                Console.WriteLine();
                emailBody.AppendLine();
            }
            Console.ResetColor();
        }
        return emailBody.ToString();
    }
}