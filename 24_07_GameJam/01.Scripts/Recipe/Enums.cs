using System;

namespace karin
{

    [Flags]
    public enum ItemNames : int
    {
        Cockatrice = 1,
        Grizzly = 2,
        Wolf = 4,
        DragonTail = 8,
        FireFlower = 16,
        GoldenRice = 32,
        Mandragora = 64,
        GitHub = 128,
        GoodHub = 256,
    }

    public enum Food
    {
        BraisedDragonTail = 1,
        AnotherWorldMeatLaunch,
        CockatriceSkewer,
        GrizzlyCurry,
        HealthySoup,
        MandragoraSteak,
        HotMarinatedMeat,
        ColdAndHotPot

    }

}