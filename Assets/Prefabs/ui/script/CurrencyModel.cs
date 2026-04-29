
using R3;
using System;

public class CurrencyModel : BasePlayerModel
{
    public ReactiveProperty<long> gold = new(0);//v1
    public ReactiveProperty<long> lives = new(0);//v1
    
    public ReactiveProperty<long> nextLifeTime = new(0);

    public override int ModelVersion => 1;

    public override void ReadOrWrite(IFileStream stream, int version)
    {
        switch (version)
        {
            case 1: ReadOrWrite_v1(stream); break;
            default: throw new Exception($"model {nameof(CurrencyModel)} has invalid version {version}");
        }
    }

    private void ReadOrWrite_v1(IFileStream stream)
    {
        stream.ReadOrWriteRxLong(ref gold, nameof(gold));
        stream.ReadOrWriteRxLong(ref lives, nameof(lives));
        stream.ReadOrWriteRxLong(ref nextLifeTime, nameof(nextLifeTime));
    }

    public override void OnModelInitializing()
    {
        base.OnModelInitializing();

        gold.Value = 5000;
        lives.Value = 5;
        nextLifeTime.Value = 0;
    }
}