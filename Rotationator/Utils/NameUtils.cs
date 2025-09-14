namespace Rotationator.Utils;

public static class NameUtils
{
    public static string GetStageName(int id)
    {
        return id switch
        {
            0 => "Urchin Underpass",
            1 => "Walleye Warehouse",
            2 => "Saltspray Rig",
            3 => "Arowana Mall",
            4 => "Blackbelly Skatepark",
            5 => "Camp Triggerfish",
            6 => "Port Mackerel",
            7 => "Kelp Dome",
            8 => "Moray Towers",
            9 => "Bluefin Depot",
            10 => "Hammerhead Bridge",
            11 => "Flounder Heights",
            12 => "Museum d'Alfonsino",
            13 => "Ancho-V Games",
            14 => "Piranha Pit",
            15 => "Mahi-Mahi Resort",
            _ => throw new Exception($"Unknown stage {id}")
        };
    }

    public static string GetRuleName(VersusRule rule)
    {
        return rule switch
        {
            VersusRule.None => "None",
            VersusRule.Paint => "Turf War",
            VersusRule.Goal => "Rainmaker",
            VersusRule.Area => "Splat Zones",
            VersusRule.Lift => "Tower Control",
            _ => throw new Exception($"Unknown rule {rule}")
        };
    }

}