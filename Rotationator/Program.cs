using System.CommandLine;
using Rotationator.Commands;

//
// Constants
//

const int defaultPhaseLength = 4;
const int defaultScheduleLength = 30;


//
// Command handling
//

var lastByamlArg = new Argument<string>("lastByaml", "The last VSSetting BYAML file.");
var outputByamlArg = new Argument<string>("outputByaml", "The output VSSetting BYAML file.");
var phaseLengthOption =
    new Option<int>("--phaseLength", () => defaultPhaseLength, "The length of each phase in hours.");
var scheduleLengthOption = new Option<int>("--scheduleLength", () => defaultScheduleLength,
    "How long the schedule should be in days.");
var overridePhasesOption =
    new Option<string?>("--overridePhases", () => null, "The override phases file.");
var seedOption = new Option<uint?>("--randomSeed", () => null, "The seed for the random number generator.");

var byamlArgument = new Argument<string>("byaml", "The BYAML file to process.");

RootCommand rootCommand = new();

Command generateCommand = new("generate", "Generates a new VSSetting BYAMl file.")
{
    lastByamlArg,
    outputByamlArg,
    phaseLengthOption,
    scheduleLengthOption,
    overridePhasesOption,
    seedOption
};
generateCommand.SetHandler(context =>
{
    string lastByamlPath = context.ParseResult.GetValueForArgument(lastByamlArg);
    string outputByamlPath = context.ParseResult.GetValueForArgument(outputByamlArg);
    int phaseLength = context.ParseResult.GetValueForOption(phaseLengthOption);
    int scheduleLength = context.ParseResult.GetValueForOption(scheduleLengthOption);
    string? overridePhasesPath = context.ParseResult.GetValueForOption(overridePhasesOption);
    uint? specifiedSeed = context.ParseResult.GetValueForOption(seedOption);
    
    GenerateCommand.Run(lastByamlPath, outputByamlPath, phaseLength, scheduleLength, overridePhasesPath, specifiedSeed);
});
rootCommand.AddCommand(generateCommand);

Command lastPhase = new("info", "Outputs information about the last VSSetting BYAML file in JSON format.")
{
    byamlArgument
};
lastPhase.SetHandler(context =>
{
    string byamlPath = context.ParseResult.GetValueForArgument(byamlArgument);
    InfoCommand.Run(byamlPath);
});
rootCommand.AddCommand(lastPhase);

rootCommand.Invoke(args);