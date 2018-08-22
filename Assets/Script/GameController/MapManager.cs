using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTileState
{
    Shadow,
    BasicTile,
    Wall,
    Player,
    Enemy
}


public class MapManager {
    private static TextAsset MapDataText = Resources.Load<TextAsset>("MapDataText/MapDataText_Cave");

    private static eTileState[,] mapData;

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

        for (int i = 0; i < mapData.GetLength(0); i++)
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

        for (int i = 0; i < mapData.GetLength(0); i++)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                GameObject TempShadow = Object.Instantiate(Shadow, ShadowParent);
                TempShadow.transform.position = ConvertIndexsToPosition(j, i);
                Shadows[i, j] = TempShadow;
                TempShadow.SetActive(false);
            }
        }

        rounding = new int[20][];
        for (int i = 1; i <= 19; i++)
        {
            rounding[i] = new int[i + 1];
            for (int j = 1; j <= i; j++)
            {
                rounding[i][j] = (int)Mathf.Min(j, Mathf.Round(i * Mathf.Cos(Mathf.Asin(j / (i + 0.5f)))));
            }
        }
        SetEnemyDefaultDestination();
    }

    public static void ConvertPositionToIndexs(Vector2 Position, out int RowIndex, out int ColumnIndex)
    {   
        RowIndex = (int)((Mathf.Round((Position.x + 6.016f) * 1000)) / 1000 / 0.64f);
        ColumnIndex = (int)((Mathf.Round((5.4f - Position.y) * 1000)) / 1000 / 0.64f);
    }

    public static void ConvertPositionToIndexs(Vector2 Position, out Vector2 TargetPosition)
    {
        int RowIndex = (int)((Mathf.Round((Position.x + 6.016f) * 1000)) / 1000 / 0.64f);
        int ColumnIndex = (int)((Mathf.Round((5.4f - Position.y) * 1000)) / 1000 / 0.64f);

        TargetPosition = new Vector2(RowIndex, ColumnIndex);

    }
    public static Vector2 ConvertIndexsToPosition(int Row, int Column)
    {
        return new Vector2(-6.016f + Row * 0.64f, 5.4f - Column * 0.64f);
    }

    public static void ShadowCast(Vector2 PlayerPosition,int Sight)
    {
        bool[] FlagShadow = SetShadowFlag(PlayerPosition, Sight);

        for (int i = 0; i < mapData.GetLength(0); i++)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                if (FlagShadow[i * WIDTH + j])
                {
                    Shadows[i, j].SetActive(false);
                }
                else
                {
                    Shadows[i, j].SetActive(true);
                }
            }
        }
    }

    public static bool[] SetShadowFlag(Vector2 PlayerPosition ,int Distance)
    {
        int x, y;
        ConvertPositionToIndexs(PlayerPosition, out x, out y);

        distance = Distance;

        limits = rounding[distance];

        for (int i = 0; i < fieldOfView.Length; i++)
        {
            fieldOfView[i] = false;
        }
        fieldOfView[y * WIDTH + x] = true;

        scanSector(x, y, +1, +1, 0, 0);
        scanSector(x, y, -1, +1, 0, 0);
        scanSector(x, y, +1, -1, 0, 0);
        scanSector(x, y, -1, -1, 0, 0);
        scanSector(x, y, 0, 0, +1, +1);
        scanSector(x, y, 0, 0, -1, +1);
        scanSector(x, y, 0, 0, +1, -1);
        scanSector(x, y, 0, 0, -1, -1);

        return fieldOfView;
    }
    private static void scanSector(int cx, int cy, int m1, int m2, int m3, int m4)
    {
        Obs.reset();
        for (int p = 1; p <= distance; p++)
        {
            float dq2 = 0.5f / p;
            int pp = limits[p];
            for (int q = 0; q <= pp; q++)
            {
                int x = cx + q * m1 + p * m3;
                int y = cy + p * m2 + q * m4;

                if (y >= 0 && y < HEIGH && x >= 0 && x < WIDTH)
                {

                    float a0 = (float)q / p;
                    float a1 = a0 - dq2;
                    float a2 = a0 + dq2;

                    int pos = y * WIDTH + x;

                    if (Obs.isBlocked(a0) && Obs.isBlocked(a1) && Obs.isBlocked(a2))
                    {
                        // Do nothing					
                    }
                    else
                    {
                        fieldOfView[pos] = true;
                    }

                    if (mapData[y,x] == eTileState.Wall)
                    {
                        Obs.add(a1, a2);
                    }
                }
            }
            Obs.nextRow();
        }
    }
    public static void ConvertIndexTo2D(int TargetIndex, out int ColumnIndex, out int RowIndex)
    {
        ColumnIndex = TargetIndex / WIDTH;
        RowIndex = TargetIndex % WIDTH;
    }

    public static int ConvertIndexTo1D(int ColumnIndex, int RowIndex)
    {
        return ColumnIndex * WIDTH + RowIndex;
    }

    public static void SetTileState(Vector2 TargetPosition, eTileState TargetTileState)
    {
        int ColumnIndex;
        int RowIndex;
        ConvertPositionToIndexs(TargetPosition, out RowIndex, out ColumnIndex);
        
        mapData[ColumnIndex, RowIndex] = TargetTileState;
    }

    public static void SetTileState(int ColumnIndex, int RowIndex, eTileState TargetTileState)
    {
        mapData[ColumnIndex, RowIndex] = TargetTileState;
    }

    public static void SetTileState(int Index1D, eTileState TargetTileState)
    {
        int ColumnIndex;
        int RowIndex;
        ConvertIndexTo2D(Index1D, out ColumnIndex, out RowIndex);

        mapData[ColumnIndex, RowIndex] = TargetTileState;
    }

    public static eTileState GetTileState(Vector2 TargetPosition)
    {
        int ColumnIndex;
        int RowIndex;
        
        ConvertPositionToIndexs(TargetPosition, out RowIndex, out ColumnIndex);

        //Debug.Log(RowIndex + " " + ColumnIndex);
        //Debug.Log(mapData[ColumnIndex, RowIndex]);
        return mapData[ColumnIndex, RowIndex];
    }
    
    public static eTileState GetTileState(int Index1D)
    {
        int ColumnIndex;
        int RowIndex;

        ConvertIndexTo2D(Index1D, out ColumnIndex, out RowIndex);

        return mapData[ColumnIndex, RowIndex];
    }

    public static eTileState GetTileState(int ColumnIndex, int Rowindex)
    {
        return mapData[ColumnIndex, Rowindex];
    }

    private static void SetEnemyDefaultDestination()
    {
        Vector2 DefaultPosition0 = new Vector2(3, 2);
        Vector2 DefaultPosition1 = new Vector2(3, 7);
        Vector2 DefaultPosition2 = new Vector2(3, 12);
        Vector2 DefaultPosition3 = new Vector2(9, 7);
        Vector2 DefaultPosition4 = new Vector2(16, 2);
        Vector2 DefaultPosition5 = new Vector2(16, 7);
        Vector2 DefaultPosition6 = new Vector2(16, 12);

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

public class Obstacles
{
    private static int SIZE = 8 * 8 / 2;
    private float[] a1 = new float[SIZE];
    private float[] a2 = new float[SIZE];

    private int length;
    private int limit;

    public  void reset()
    {
        length = 0;
        limit = 0;
    }

    public  void add(float o1, float o2)
    {

        if (length > limit && o1 <= a2[length - 1])
        {
            // Merging several blocking cells
            a2[length - 1] = o2;
        }
        else
        {
            a1[length] = o1;
            a2[length++] = o2;
        }
    }

    public  bool isBlocked(float a)
    {
        for (int i = 0; i < limit; i++)
        {
            if (a >= a1[i] && a <= a2[i])
            {
                return true;
            }
        }
        return false;
    }

    public  void nextRow()
    {
        limit = length;
    }
}


// 2018 - 08 - 17 픽셀던전 깃헙에서 배껴옴!! 어케돌아가는 로직인지도 모름!! 
// 꼭 시간날때마다 공부해보자!!