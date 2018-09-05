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

        //나는 아직 제너릭을 쓰기엔 부족한거같다
    }
}

public enum eTileState
{
    Shadow,
    BasicTile,
    Movable,
    Wall,
    Player,
    Enemy
}

public enum eTouchMode
{
    GamePlay,
    CloseInventory,
}

public enum eChooseItemMode
{
    RandomPotion,
    RandomStatus,
}

public enum eStatus
{
    Health,
    AttackPoint,
}

