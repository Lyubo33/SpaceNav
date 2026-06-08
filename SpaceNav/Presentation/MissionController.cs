using System.Security.Cryptography;
using SpaceNav.Domain.Abstractions;
using SpaceNav.Domain.Models;
using SpaceNav.Services;

namespace SpaceNav.Presentation;

/// <summary>
/// The main application class that controls the entire mission, handles inputs and outputs
/// </summary>
public class MissionController
{
    private CosmicMap? _currentMap;
    private List<Astronaut> _astronauts = new();
    private readonly string[] _menuOptions = new[]
    {
        " 󰌌 Process Map via Manual Console Input",
        "  Generate a Random Cosmic Map",
        "  Shutdown System"
    };
    /// <summary>
    /// Main application loop
    /// </summary>
    public void StartApplicationLoop()
    {
        bool running = true;
        while (running)
        {
            int selectionIndex = ConsoleUI.PromptMenuSelection(_menuOptions);
            switch (selectionIndex)
            {
                case 0:
                    HandleManualMapInput();
                    break;
                case 1: // Random Map Generator
                    HandleRandomMapInput();
                    break;
                case 2: // Exit
                    running = false;
                    ConsoleUI.DisplayHeader("󱎃 SPACE NAV V.2026 󱎃 ");
                    ConsoleUI.PrintMessage("\nShutting down navigation. Safe travels, Commander.", ConsoleColor.Cyan);
                    break;
            }
        }
    }
    
    /// <summary>
    /// Method to handle input of map via stdin
    /// </summary>
    private void HandleManualMapInput()
    {
        ConsoleUI.DisplayHeader("󰌌 Manual map input 󰌌 ");
        try
        {
            int rows = ConsoleUI.PromptInteger("[  ] Enter number of rows (M):", 2, 100);
            int columns = ConsoleUI.PromptInteger("[  ] Enter number of columns (N):", 2, 100);
            List<string> lines = ConsoleUI.PromptMapGridLines(rows);
            var (map, astronauts) = MapParser.ParseMapData(lines, rows, columns);
            _currentMap = map;
            _astronauts = astronauts;
            ConsoleUI.PrintMessage("\n[ SUCCESS] Matrix successfully loaded in navigation memory.", ConsoleColor.Green);
            ProcessAndPrintResults();
        }
        catch (FormatException e)
        {
            ConsoleUI.PrintMessage($"\n[ MAP FORMAT ERROR]: {e.Message}", ConsoleColor.Red);
        }
        catch (Exception e)
        {
            ConsoleUI.PrintMessage($"\n[ SYSTEM ANOMALY]: { e.Message }", ConsoleColor.Red);
        }
        ConsoleUI.PromptKeyPressToContinue();
    }
    
    /// <summary>
    /// Method to handle input of randomly generated map
    /// </summary>
    private void HandleRandomMapInput()
    {
        RandomGenerator randomMap = new RandomGenerator();
        ConsoleUI.DisplayHeader(" Random map input  ");
        try
        {
            int rows = ConsoleUI.PromptInteger("[  ] Enter number of rows (M):", 2, 100);
            int columns = ConsoleUI.PromptInteger("[  ] Enter number of columns (N):", 2, 100);
            int maxAsteroids = Math.Max(0,(columns * rows) - 3 - 1);
            int asteroids = ConsoleUI.PromptInteger("Enter asteroid count:",0,maxAsteroids);
            List<string> lines = randomMap.GenerateRandomMapLines(rows, columns,asteroids);
            var (map,astronauts) = MapParser.ParseMapData(lines, rows, columns);
            _currentMap = map;
            _astronauts = astronauts;
            ConsoleUI.PrintMessage("\n[ SUCCESS] Matrix successfully loaded in navigation memory.", ConsoleColor.Green);
            ProcessAndPrintResults();
            
        }catch (FormatException e)
        {
            ConsoleUI.PrintMessage($"\n[ MAP FORMAT ERROR]: {e.Message}", ConsoleColor.Red);
        }
        catch (Exception e)
        {
            ConsoleUI.PrintMessage($"\n[ SYSTEM ANOMALY]: { e.Message }", ConsoleColor.Red);
        }
        ConsoleUI.PromptKeyPressToContinue();
    }
    
    /// <summary>
    /// Proceses results when <see cref="CosmicMap"/> and <see cref="List"/> of <see cref="Astronaut"/>s
    /// have been already parsed and generated, then it prints the result to the stdout
    /// </summary>
    private void ProcessAndPrintResults()
    {
        IPathfinder pathfinder = _currentMap.HasDebris() ? new DijkstraPathfinder() : new BfsPathfinder();

        foreach (Astronaut a in _astronauts)
        {
            PathResult result = pathfinder.FindPath(_currentMap, a.StartPosition);
            if (result.Success)
            {
                a.UpdateCalculatedPath(result.Steps,result.TotalCost);
            }
            else
            {
                a.SetAsLost();
            }
        }
        List<Astronaut> sortedAstronauts = _astronauts
            .OrderBy(a => a.IsPathFound)
            .ThenBy(a => a.PathCost)
            .ToList();
        string reportText = ConsoleUI.RenderResult(sortedAstronauts,_currentMap);
        ConsoleUI.PrintMessage("\nWould you like to send report via mail(y/n):", ConsoleColor.Cyan);
        string choice = Console.ReadLine().Trim().ToLower();
        if (choice == "y")
        {
            HandleEmailReport(reportText);
        }
    }
    
    /// <summary>
    /// Handles the sending of the report to mission control via email
    /// </summary>
    /// <param name="missionReport"> The mission report passed as a simple <see cref="string"/></param>
    private void HandleEmailReport(string missionReport)
    {
        ConsoleUI.DisplayHeader("    󰇮 SMTP Email report 󰇮    ");
        try
        {
            string sender = ConsoleUI.PromptString("Enter your email address:");
            string password = ConsoleUI.PromptPassword("Enter your password:");
            string recipient = ConsoleUI.PromptString("Send report to:");
            ConsoleUI.PrintMessage("\n[󱘖 SYSTEM] Connecting to mission control...", ConsoleColor.DarkCyan);
            EmailService.SendMailReport(sender, password, recipient, missionReport);
            ConsoleUI.PrintMessage($"\n[ SUCCESS] Mission report send to {recipient}",ConsoleColor.Green);
        }
        catch (Exception e)
        {
            ConsoleUI.PrintMessage($"\n[ SMTP ROUTINE ERROR]: {e.Message}", ConsoleColor.Red);
            if (e.InnerException != null)
            {
                ConsoleUI.PrintMessage($"[ NETWORK CORE REASON]: {e.InnerException.Message}", ConsoleColor.Yellow);
            }
        }
    }

}