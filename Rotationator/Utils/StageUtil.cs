namespace Rotationator.Utils;

public static class StageUtil
{
    private static readonly Dictionary<VersusRule, List<int>> BannedStages = new()
    {
        {
            VersusRule.Paint,
            [] // nothing banned
        },
        {
            VersusRule.Goal,
            [
                2, // Saltspray Rig
                4, // Blackbelly Skatepark
                14
            ]
        },
        {
            VersusRule.Area,
            [] // nothing banned
        },
        {
            VersusRule.Lift,
            [
                2, // Saltspray Rig
                6 // Port Mackerel
            ]
        }
    };

    private static readonly List<int> DefaultStagePool = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15];
    
    public static int PickStage(GambitVersusPhase phase, GambitVersusPhase lastPhase, OverridePhase? nextPhaseOverride, VersusRule rule,
        List<int> pool, SeadRandom random)
    {
        var bannedStagesForRule = BannedStages[rule];

        if (pool.Count == 0)
        {
            pool.AddRange(DefaultStagePool.Except(bannedStagesForRule));
        }

        // If the current pool has any valid stages, pick from there.
        if (pool.Any(i => IsStageValid(lastPhase, phase, i)))
            return RandomUtils.GetRandomElementFromPool(pool, stageId1 => IsStageValid(lastPhase, phase, stageId1),
                random);
        
        // Otherwise, pick a random stage from the default pool, excluding:
        // - the current phase's stages (in both Regular and Gachi)
        // - the last phase's stages (in both Regular and Gachi)
        // - the next phase's stages (in both Regular and Gachi, if known)
        // - all banned stages for this rule
        var newPool = DefaultStagePool.Except(phase.RegularInfo.Stages)
            .Except(phase.GachiInfo.Stages)
            .Except(lastPhase.RegularInfo.Stages)
            .Except(lastPhase.GachiInfo.Stages)
            .Except(bannedStagesForRule);

        if (nextPhaseOverride != null)
        {
            newPool = newPool.Except(nextPhaseOverride.RegularStages)
                .Except(nextPhaseOverride.GachiStages);
        }

        pool = newPool.ToList();

        return RandomUtils.GetRandomElementFromPool(pool, stageId1 => IsStageValid(lastPhase, phase, stageId1), random);
    }

    private static bool IsStageValid(GambitVersusPhase gambitVersusPhase1, GambitVersusPhase gambitVersusPhase, int stageId)
    {
        // Don't pick this stage if it's already used in this phase.
        if (gambitVersusPhase.RegularInfo.Stages.Contains(stageId) || gambitVersusPhase.GachiInfo.Stages.Contains(stageId))
        {
            return false;
        }

        // Don't pick this stage if it's present in the last phase.
        if (gambitVersusPhase1.RegularInfo.Stages.Contains(stageId) || gambitVersusPhase1.GachiInfo.Stages.Contains(stageId))
        {
            return false;
        }

        return true;
    }
}