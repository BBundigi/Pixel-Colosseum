using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileState
{
    Shadow,
    BasicTile,
    Movable,
    Wall,
    Player,
    Enemy
}


public static class MapManager {
    private static TextAsset MapDataText = Resources.Load<TextAsset>("MapDataText/MapDataText_Cave");

    private static eTileState[,] mapData;

    public static Object[] MapObjects;

    public static int WIDTH;
    public static int HEIGH;

    private static Vector2[][] enemyDefaultDestination;

    public static Vector2[][] EnemyDefaultDestination
    {
        get
        {
            return enemyDefaultDestination;
        }
    }

    static MapManager() {
        string StringMapData = MapDataText.ToString();

        string[] EachString = StringMapData.Split('\n');

        mapData = new eTileState[EachString.Length, EachString[0].Length - 1];
        MapObjects = new Object[mapData.Length];

        for (int i = mapData.GetLength(0) - 1; i >= 0; i--)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                if (EachString[i][j] == '1')
                {
                    mapData[i,j] = eTileState.BasicTile;
                }
                else
                {
                    mapData[i,j] = eTileState.Wall;
                }
            }
        }

        HEIGH = mapData.GetLength(0);
        WIDTH = mapData.GetLength(1);

        SetEnemyDefaultDestination();
    }

    public static void ConvertPositionToIndexs(Vector2 Position, out int RowIndex, out int ColumnIndex)
    {
        RowIndex = (int)((Mathf.Round((Position.x + 6.016f) * 1000)) / 1000 / 0.64f);
        ColumnIndex = (int)((Mathf.Round((Position.y + 4.12f) * 1000)) / 1000 / 0.64f);
    }

    public static Vector2 ConvertIndexsToPosition(int Row, int Column)
    {
        return new Vector2(-6.016f + Row * 0.64f, -4.12f + Column * 0.64f);
    }

    public static void ConvertIndexTo2D(int TargetIndex, out int RowIndex, out int ColumnIndex)
    {
        RowIndex = TargetIndex % WIDTH;
        ColumnIndex = TargetIndex / WIDTH;
    }

    public static int ConvertIndexTo1D(int RowIndex, int ColumnIndex)
    {
        return ColumnIndex * WIDTH + RowIndex;
    }

    public static void SetTileState(Vector2 TargetPosition, eTileState TargetTileState)
    {
        int RowIndex;
        int ColumnIndex;
        ConvertPositionToIndexs(TargetPosition, out RowIndex, out ColumnIndex);
        
        mapData[ColumnIndex, RowIndex] = TargetTileState;
    }

    public static void SetTileState(int RowIndex, int ColumnIndex , eTileState TargetTileState)
    {
        mapData[ColumnIndex, RowIndex] = TargetTileState;
    }

    public static void SetTileState(int Index1D, eTileState TargetTileState)
    {
        int ColumnIndex;
        int RowIndex;
        ConvertIndexTo2D(Index1D, out RowIndex, out ColumnIndex);

        mapData[ColumnIndex, RowIndex] = TargetTileState;
    }

    public static void SetMapObjects(int RowIndex, int ColumnIndex, Object TargetObject)
    {
        int index = ConvertIndexTo1D(RowIndex, ColumnIndex);
        MapObjects[index] = TargetObject;
    }

    public static Object GetMapObjects(int RowIndex, int ColumnIndex)
    {
        int index = ConvertIndexTo1D(RowIndex, ColumnIndex);

        return MapObjects[index];
    }

    public static eTileState GetTileState(Vector2 TargetPosition)
    {
        int RowIndex;
        int ColumnIndex;

        ConvertPositionToIndexs(TargetPosition, out RowIndex, out ColumnIndex);

        return mapData[ColumnIndex, RowIndex];
    }
    
    public static eTileState GetTileState(int Index1D)
    {
        int RowIndex;
        int ColumnIndex;

        ConvertIndexTo2D(Index1D, out RowIndex, out ColumnIndex);

        return mapData[ColumnIndex, RowIndex];
    }

    public static eTileState GetTileState(int RowIndex, int ColumnIndex)
    { 
        return mapData[ColumnIndex, RowIndex];
    }

    public static bool CheckTileState(int Distance, eTileState TargetTileState, int RowIndex, int ColumnIndex)
    {
        for (int i = -Distance; i < Distance + 1; i++)
        {
            for (int j = -Distance; j < Distance + 1; j++)
            {
                if (ColumnIndex + i > 0 && ColumnIndex + i < HEIGH && RowIndex + j > 0 && RowIndex + j < WIDTH)
                {
                    if(mapData[ColumnIndex + i, RowIndex + j] == TargetTileState)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private static void SetEnemyDefaultDestination()
    {
        Vector2 DefaultPosition0 = new Vector2(3, 12);
        Vector2 DefaultPosition1 = new Vector2(3, 7);
        Vector2 DefaultPosition2 = new Vector2(3, 2);
        Vector2 DefaultPosition3 = new Vector2(9, 7);
        Vector2 DefaultPosition4 = new Vector2(16, 12);
        Vector2 DefaultPosition5 = new Vector2(16, 7);
        Vector2 DefaultPosition6 = new Vector2(16, 2);

        enemyDefaultDestination = new Vector2[7][];

        enemyDefaultDestination[0] = new Vector2[2];

        enemyDefaultDestination[0][0] = DefaultPosition0;
        enemyDefaultDestination[0][1] = DefaultPosition1;

        enemyDefaultDestination[1] = new Vector2[4];

        enemyDefaultDestination[1][0] = DefaultPosition1;
        enemyDefaultDestination[1][1] = DefaultPosition0;
        enemyDefaultDestination[1][2] = DefaultPosition2;
        enemyDefaultDestination[1][3] = DefaultPosition3;

        enemyDefaultDestination[2] = new Vector2[2];

        enemyDefaultDestination[2][0] = DefaultPosition2;
        enemyDefaultDestination[2][1] = DefaultPosition1;

        enemyDefaultDestination[3] = new Vector2[3];

        enemyDefaultDestination[3][0] = DefaultPosition3;
        enemyDefaultDestination[3][1] = DefaultPosition1;
        enemyDefaultDestination[3][2] = DefaultPosition5;

        enemyDefaultDestination[4] = new Vector2[2];

        enemyDefaultDestination[4][0] = DefaultPosition4;
        enemyDefaultDestination[4][1] = DefaultPosition5;

        enemyDefaultDestination[5] = new Vector2[4];

        enemyDefaultDestination[5][0] = DefaultPosition5;
        enemyDefaultDestination[5][1] = DefaultPosition4;
        enemyDefaultDestination[5][2] = DefaultPosition3;
        enemyDefaultDestination[5][3] = DefaultPosition6;

        enemyDefaultDestination[6] = new Vector2[2];

        enemyDefaultDestination[6][0] = DefaultPosition6;
        enemyDefaultDestination[6][1] = DefaultPosition5;
    }
}
