using System.Text.Json;
using OatmealDome.NinLib.Byaml.Dynamic;

namespace Rotationator.Commands;

public static class InfoCommand
{
    public static void Run(string byamlPath)
    {
        dynamic lastByaml = ByamlFile.Load(byamlPath);

        DateTime lastBaseTime = DateTime.Parse(lastByaml["DateTime"]).ToUniversalTime();
        List<dynamic> lastPhases = lastByaml["Phases"];
        
        if (lastPhases.Count == 0)
        {
            Console.Error.WriteLine("No phases found in BYAML file.");
        }
        
        var phases = lastPhases.Select(p => new GambitVersusPhase(p)).ToList();

        var totalHours = phases.Sum(p => p.Length);
        
        var standardPhaseDuration = phases.First().Length;
        var lastPhaseEndTime = lastBaseTime.AddHours(totalHours);
        
        // The last phase has a longer duration than the rest, so we need to specifically calculate its start time.
        var lastPhaseDuration = phases.Last().Length;
        var lastPhaseStartTime = lastPhaseEndTime.AddHours(-lastPhaseDuration);

        var output = new
        {
            phaseCount = phases.Count,
            phaseDuration = standardPhaseDuration,
            baseDateTime = lastBaseTime.ToString("O"),
            lastPhaseStart = lastPhaseStartTime.ToString("O"),
            generationTime = lastByaml["ByamlInfo"]["GenerationTime"],
            scheduleLength = lastByaml["ByamlInfo"]["ScheduleLength"],
            randomSeed = lastByaml["ByamlInfo"]["RandomSeed"]
        };

        var outputJson = JsonSerializer.Serialize(output, new JsonSerializerOptions
        {
            WriteIndented = true
        });
        
        Console.WriteLine(outputJson);
    }
}