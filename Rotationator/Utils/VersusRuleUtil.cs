namespace Rotationator.Utils;

public static class VersusRuleUtil
{
    private static readonly List<VersusRule> DefaultGachiRulePool =
    [
        VersusRule.Goal,
        VersusRule.Area,
        VersusRule.Lift
    ];
    
    public static VersusRule FromEnumString(string str)
    {
        return str switch
        {
            "cPnt" => VersusRule.Paint,
            "cVgl" => VersusRule.Goal,
            "cVar" => VersusRule.Area,
            "cVlf" => VersusRule.Lift,
            _ => VersusRule.None
        };
    }

    public static string ToEnumString(this VersusRule rule)
    {
        return rule switch
        {
            VersusRule.None => "cNone",
            VersusRule.Paint => "cPnt",
            VersusRule.Goal => "cVgl",
            VersusRule.Area => "cVar",
            VersusRule.Lift => "cVlf",
            _ => throw new ArgumentOutOfRangeException(nameof(rule), "VersusRule not supported")
        };
    }
    
    public static VersusRule PickGachiRule(GambitStageInfo stageInfo, GambitStageInfo lastStageInfo, OverridePhase? nextPhaseOverride,
        List<VersusRule> pool, SeadRandom random)
    {
        if (pool.Count == 0)
        {
            pool.AddRange(DefaultGachiRulePool);
        }

        bool IsRuleValid(VersusRule rule)
        {
            if (nextPhaseOverride != null)
            {
                if (nextPhaseOverride.GachiRule == rule)
                {
                    return false;
                }
            }
        
            return rule != lastStageInfo.Rule;
        }

        if (pool.All(r => !IsRuleValid(r)))
        {
            List<VersusRule> forbiddenRules = new List<VersusRule>()
            {
                lastStageInfo.Rule
            };

            if (nextPhaseOverride != null)
            {
                forbiddenRules.Add(nextPhaseOverride.GachiRule);
            }

            pool = DefaultGachiRulePool.Except(forbiddenRules).ToList();
        }

        return RandomUtils.GetRandomElementFromPool(pool, IsRuleValid, random);
    }
}