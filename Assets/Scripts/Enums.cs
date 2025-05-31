public enum GameState
{
    MENU,
    WEAPONSELECTION,
    GAME,
    STAGECOMPLETE,
    WAVETRANSITION,
    SHOP,
    GAMEOVER
}

public enum Stat
{
    Attack,
    AttackSpeed,
    CriticalChance,
    CriticalDamage,
    MoveSpeed,
    MaxHealth,
    Range,
    HealthRecoverySpeed,
    Armor,
    Luck,
    Dodge,
    Lifesteal,
}

public static class Enums
{
    public static string FormatStatName(Stat stat)
    {
        string formated = "";
        string unformattedString = stat.ToString();

        if (unformattedString.Length <= 0)
            return "Unvalid Stat name";

        formated += unformattedString[0];

        for (int i = 1; i < unformattedString.Length; i++)
        {
            if (char.IsUpper(unformattedString[i]))
                formated += " ";

            formated += unformattedString[i];
        }

        return formated;
    }
}