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
    Shadow = 1,
    BasicTile = 2,
    Wall = 4,
    Movable = 8,
    Player = 16,
    Enemy = 32,
    OnFire = 64,
    OnPoison = 128,
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

public enum eItemType
{
    Potion,
    Scroll,
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



