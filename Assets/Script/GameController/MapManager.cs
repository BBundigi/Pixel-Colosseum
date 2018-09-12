using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapManager {
    private static TextAsset MapDataText = Resources.Load<TextAsset>("MapDataText/MapDataText_Cave");

    

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

    private static eTileState[,] mapData;

    private const float WORLDPOS_LOCAL_EnemyXPos_0 = -9;
    private const float WORLDPOS_LOCAL_localYPos_0 = -7;
    public const float TILE_GAP = 1.0f;
    //Key Value for set MapManager!!

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
        RowIndex = (int)((Mathf.Round((Position.x - WORLDPOS_LOCAL_EnemyXPos_0) * 1000)) / 1000 / TILE_GAP);
        ColumnIndex = (int)((Mathf.Round((Position.y - WORLDPOS_LOCAL_localYPos_0) * 1000)) / 1000 / TILE_GAP);
    }

    public static Vector2 ConvertIndexsToPosition(int Row, int Column)
    {
        return new Vector2(WORLDPOS_LOCAL_EnemyXPos_0 + Row * TILE_GAP, WORLDPOS_LOCAL_localYPos_0 + Column * TILE_GAP);
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

    public static bool AddTileState(Vector2 TargetPosition, eTileState TargetTileState)
    {
        int RowIndex;
        int ColumnIndex;
        ConvertPositionToIndexs(TargetPosition, out RowIndex, out ColumnIndex);

        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return true;
        }
        
        mapData[ColumnIndex, RowIndex] += (int)TargetTileState;
        return false;
    }

    public static bool AddTileState(int RowIndex, int ColumnIndex , eTileState TargetTileState)
    {
        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return false;
        }

        mapData[ColumnIndex, RowIndex] += (int)TargetTileState;
        return true;
    }

    public static bool AddTileState(int Index1D, eTileState TargetTileState)
    {
        int ColumnIndex;
        int RowIndex;
        ConvertIndexTo2D(Index1D, out RowIndex, out ColumnIndex);

        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return false;
        }

        mapData[ColumnIndex, RowIndex] += (int)TargetTileState;
        return true;
    }

    public static bool RemoveTileState(int RowIndex, int ColumnIndex, eTileState TargetTileState)
    {
        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return false;
        }
        mapData[ColumnIndex, RowIndex] -= (int)TargetTileState;
        return true;
    }

    public static bool RemoveTileState(int Index1D, eTileState TargetTileState)
    {
        int ColumnIndex;
        int RowIndex;
        ConvertIndexTo2D(Index1D, out RowIndex, out ColumnIndex);

        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return true;
        }

        mapData[ColumnIndex, RowIndex] -= (int)TargetTileState;

        return false;
    }

    public static bool SetMapObjects(int RowIndex, int ColumnIndex, Object TargetObject)
    {
        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return true;
        }
        int index = ConvertIndexTo1D(RowIndex, ColumnIndex);
        MapObjects[index] = TargetObject;

        return false;
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


        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return eTileState.None;
        }

        return mapData[ColumnIndex, RowIndex];
    }
    
    public static eTileState GetTileState(int Index1D)
    {
        int RowIndex;
        int ColumnIndex;

        ConvertIndexTo2D(Index1D, out RowIndex, out ColumnIndex);

        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return eTileState.None;
        }

        return mapData[ColumnIndex, RowIndex];
    }

    public static eTileState GetTileState(int RowIndex, int ColumnIndex)
    {
        if (RowIndex < 0 || RowIndex >= WIDTH || ColumnIndex < 0 || ColumnIndex >= HEIGH)
        {
            return eTileState.None;
        }

        return mapData[ColumnIndex, RowIndex];
    }

    public static bool CheckTileState(int Distance, eTileState TargetTileState, int RowIndex, int ColumnIndex)
    {
        for (int i = -Distance; i < Distance + 1; i++)
        {
            for (int j = -Distance; j < Distance + 1; j++)
            {
                if (ColumnIndex + i >= 0 && ColumnIndex + i < HEIGH && RowIndex + j >= 0 && RowIndex + j < WIDTH)
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
        //Use Secene number for set Defualt Destination In EnemyClass 
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
