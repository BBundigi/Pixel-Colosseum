using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileState
{
    Shadow,
    BasicTile,
    Wall,
    Player,
    Enemy
}


public class MapManager {

    public static TileState[,] MapData
    {
        get
        {
            return mapData;
        }
    }

    private static TextAsset MapDataText = Resources.Load<TextAsset>("MapDataText/MapDataText_Cave");
    private static TileState[,] mapData;

    private static int WIDTH;
    private static int HEIGH;
    private static int distance;
    private static int[] limits;
    private static bool[] fieldOfView;

    private static int[][] rounding;
    
    // MapData[x,y] = 0 -> Wall
    //              = 1 -> Tile
    //              = 2 -> Player
    //              = 3 -> Enemy
    //              = 4 -> Shadow
    // Use this for initialization

    private int SightDistance;

    private static GameObject Shadow = Resources.Load<GameObject>("Prefab/shadow");

    
    private static GameObject[,] Shadows;
    private static Transform ShadowParent;


    private static Obstacles Obs = new Obstacles();

     static MapManager() { 

        string StringMapData = MapDataText.ToString();

        string[] EachString = StringMapData.Split('\n');

        mapData = new TileState[EachString.Length, EachString[0].Length - 1];

        for (int i = 0; i < mapData.GetLength(0); i++)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                if (EachString[i][j] == '1')
                {
                    mapData[i,j] = TileState.BasicTile;
                }
                else

                {
                    mapData[i,j] = TileState.Wall;
                }
            }
        }

        HEIGH = mapData.GetLength(0);
        WIDTH = mapData.GetLength(1);

        fieldOfView = new bool[HEIGH * WIDTH];

        ShadowParent = GameObject.FindGameObjectWithTag("ShadowParent").transform;
        Shadows = new GameObject[HEIGH,WIDTH];

        for (int i = 0; i < mapData.GetLength(0); i++)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                GameObject TempShadow = Object.Instantiate(Shadow, ShadowParent);
                TempShadow.transform.position = GetPositionFromIndex(j, i);
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
    }

    private static void GetIndexsFromPosition(Vector2 Position, out int RowIndex, out int ColumnIndex)
    {
        RowIndex = (int)Mathf.Round(((Position.x + 0.08f) * 100)) / 16 + 9;
        ColumnIndex = (7 - (int)Mathf.Round((Position.y - 0.08f) * 100) / 16);
    }


    private static Vector2 GetPositionFromIndex(int Row, int Column)
    {
        return new Vector2(-1.52f + Row * 0.16f, 1.20f - Column * 0.16f);
    }

    public static bool[] SetShadowFlag(Vector2 PlayerPosition ,int Distance)
    {
        int x, y;
        GetIndexsFromPosition(PlayerPosition, out x, out y);

        distance = Distance;

        limits = rounding[distance];

        for(int i =0; i < fieldOfView.Length; i++)
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

    public static void ShadowCast(Vector3 PlayerPosition,int Sight)
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

                    if (mapData[y,x] == 0)
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
        RowIndex = TargetIndex & WIDTH;
    }

    public static int ConverIndexTo1D(int ColumnIndex, int RowIndex)
    {
        return ColumnIndex * WIDTH + RowIndex;
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