using System;
using System.Collections;

public static class EnumManager
{
    public static T RandomEnumValue<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        //return Array that Contain value of constance enum
        int random = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(random);
    }

    public static void ConvertItemIDToTileState(eItemID TargetItem)
    {

    }
}

public enum eTileState
{
    None = 0,
    Shadow = 1 << 0,
    BasicTile = 1 << 1,
    Wall = 1 << 2,
    Movable = 1 << 3,
    Player = 1 << 4,
    Enemy = 1 << 5,
    OnFire = 1<< 6,
    OnPoison = 1<<7,
}

public enum eBuffType
{
    None = 0,
    OnFire = 1,
    OnPoison = 2,
}

public enum eTouchMode
{
    None,
    GamePlay,
    CloseInventory,
    ChooseItem,
    EnemyTurn,
    ThrowItem,
}

public enum eChooseItemMode
{
    RandomPotion,
    //RandomStatus,
}

public enum eStatus
{
    Health,
    AttackPoint,
}

public enum eItemID
{
    MirrorScroll = 40,
    FireFlask = 57,
    PoisonFlask = 59,
}

public enum eEnemyState
{
    Move,
    Attack,
};

public enum ePlayerState
{
    Move,
    Attack,
};



