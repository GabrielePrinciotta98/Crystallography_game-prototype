public static class ScoreManager
{
    private static int score = 5000;
    private static int timeBonus;

    public static int TimeBonus
    {
        get => timeBonus;
        set => timeBonus = value;
    }

    public static int Score
    {
        get => score;
        set
        {
            score = value + timeBonus;
            timeBonus = 0;
        }
    }

    
    
}
