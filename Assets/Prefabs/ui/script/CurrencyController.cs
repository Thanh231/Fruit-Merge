using R3;

public static class CurrencyController
{
    static CurrencyModel currencyModel => PlayerModelManager.instance.GetPlayerModel<CurrencyModel>();

    public static ReactiveProperty<long> GetGoldRx()
    {
        return currencyModel.gold;
    }

    public static long GetGold()
    {
        return currencyModel.gold.Value;
    }

    public static void AddGold(long amount)
    {
        currencyModel.gold.Value += amount;

        currencyModel.Save();
    }

    public static void SubtractGold(long amount)
    {
        currencyModel.gold.Value -= amount;

        currencyModel.Save();
    }

    public static ReactiveProperty<long> GetLivesRx()
    {
        return currencyModel.lives;
    }

    public static long GetLives()
    {
        return currencyModel.lives.Value;
    }

    public static void AddLives(long amount)
    {
        currencyModel.lives.Value += amount;

        currencyModel.Save();
    }

    public static void SubtractLives(long amount)
    {
        currencyModel.lives.Value -= amount;

        currencyModel.Save();
    }

    public static ReactiveProperty<long> GetNextLifeTimeRx()
    {
        return currencyModel.nextLifeTime;
    }

    public static long GetNextLifeTime()
    {
        return currencyModel.nextLifeTime.Value;
    }

    public static void SetNextLifeTime(long timestamp)
    {
        currencyModel.nextLifeTime.Value = timestamp;
        currencyModel.Save();
    }
}
