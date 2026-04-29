using ObservableCollections;
using R3;
using System;

public class LevelModelItem : IFileStreamObject
{
    public int level;

    public int ModelVersion => 1;

    public void ReadOrWrite(IFileStream stream, int version)
    {
        switch (version)
        {
            case 1: ReadOrWrite_v1(stream); break;
            default: throw new Exception($"model {nameof(LevelModelItem)} has invalid version {version}");
        }
    }

    private void ReadOrWrite_v1(IFileStream stream)
    {
        stream.ReadOrWriteInt(ref level, nameof(level));
    }
}

public class LevelModel : BasePlayerModel
{
    public ReactiveProperty<int> lLevel = new ReactiveProperty<int>(1);

    public override int ModelVersion => 1;

    public override void ReadOrWrite(IFileStream stream, int version)
    {
        switch (version)
        {
            case 1: ReadOrWrite_v1(stream); break;
            default: throw new Exception($"model {nameof(LevelModel)} has invalid version {version}");
        }
    }

    private void ReadOrWrite_v1(IFileStream stream)
    {
        stream.ReadOrWriteRxInt(ref lLevel, nameof(lLevel));
    }

    public override void OnModelInitializing()
    {
        base.OnModelInitializing();

        lLevel.Value = 1;
    }
}
