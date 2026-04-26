using UnityEngine;

public class DatabaseManager : Singleton<DatabaseManager>
{
    #region Player Data
    private int playerScore;
    public int PlayerScore
    {
        get { return playerScore; }
        set { playerScore = value; }
    }

    private int playerLevel;
    public int PlayerLevel
    {
        get { return playerLevel; }
        set { playerLevel = value; }
    }

    private int playerLives;
    public int PlayerLives
    {
        get { return playerLives; }
        set { playerLives = value; }
    }


    private int playerCoins;
    public int PlayerCoins
    {
        get { return playerCoins; }
        set { playerCoins = value; }
    }

    private int stageNumber;
    public int StageNumber
    {
        get { return stageNumber; }
        set { stageNumber = value; }
    }

    private bool noAdsPurchased;
    public bool NoAdsPurchased
    {
        get { return noAdsPurchased; }
        set { noAdsPurchased = value; }
    }

    #endregion
    

}
