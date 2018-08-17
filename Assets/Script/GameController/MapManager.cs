using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MapManager : MonoBehaviour {
    public static MapManager Instance;
    [SerializeField]
    private TextAsset MapDataText;
    private int[,] mapData;

    private int WIDTH;
    private int HEIGH;
    private int distance;
    private int[] limits;
    private bool[] fieldOfView;

    private int[][] rounding;

    public int[,] MapData
    {
        get
        {
            return mapData;
        }
    }    
    // MapData[x,y] = 0 -> Wall
    //              = 1 -> Tile
    //              = 2 -> Player
    //              = 3 -> Enemy
    //              = 4 -> Shadow
    // Use this for initialization

    private int SightDistance;

    [SerializeField]
    private GameObject Shadow;

    private GameObject[,] Shadows;
    private Transform ShadowParent;


    private  Obstacles obs = new Obstacles();
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start() { 

        string StringMapData = MapDataText.ToString();

        string[] EachString = StringMapData.Split('\n');

        mapData = new int[EachString.Length, EachString[0].Length - 1];

        for (int i = 0; i < mapData.GetLength(0); i++)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                mapData[i, j] = (int)char.GetNumericValue(EachString[i][j]);
            }
        }

        //for(int i = 0; i < mapData.GetLength(0); i++)
        //{
        //    string TargetString = string.Empty;
        //    for(int j = 0; j < mapData.GetLength(1); j++)
        //    {
        //        if(mapData[i,j] == 1)
        //        {
        //            TargetString += "0";
        //        }
        //        else
        //        {
        //            TargetString += "X";
        //        }
        //    }
        //}
        HEIGH = mapData.GetLength(0);
        WIDTH = mapData.GetLength(1);

        fieldOfView = new bool[HEIGH * WIDTH];

        ShadowParent = GameObject.FindGameObjectWithTag("ShadowParent").transform;
        Shadows = new GameObject[HEIGH,WIDTH];

        for (int i = 0; i < mapData.GetLength(0); i++)
        {
            for (int j = 0; j < mapData.GetLength(1); j++)
            {
                GameObject TempShadow = Instantiate(Shadow, ShadowParent);
                TempShadow.transform.position = GetPositionFromIndex(j, i);
                Shadows[i, j] = TempShadow;
                TempShadow.SetActive(false);
            }
        }

        castShadow(PlayerManager.Instance.transform.position, 12);
    }

    private void GetIndexsFromPosition(Vector2 Position, out int RowIndex, out int ColumnIndex)
    {
        RowIndex = (int)Mathf.Round(((Position.x + 0.08f) * 100)) / 16 + 9;
        ColumnIndex = (7 - (int)Mathf.Round((Position.y - 0.08f) * 100) / 16);
    }


    private Vector2 GetPositionFromIndex(int Row, int Column)
    {
        return new Vector2(-1.52f + Row * 0.16f, 1.20f - Column * 0.16f);
    }

    public void castShadow(Vector2 PlayerPosition, int Distance)
    {
        int x, y;
        GetIndexsFromPosition(PlayerPosition, out x, out y);

        rounding = new int[Distance + 1][];
        for (int i = 1; i <= Distance; i++)
        {
            rounding[i] = new int[i + 1];
            for (int j = 1; j <= i; j++)
            {
                rounding[i][j] = (int)Mathf.Min(j, Mathf.Round(i * Mathf.Cos(Mathf.Asin(j / (i + 0.5f)))));
            }
        }
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

        for(int i = 0; i < mapData.GetLength(0); i++)
        {
            for(int j =0; j< mapData.GetLength(1); j++)
            {
                if(fieldOfView[i * WIDTH + j])
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

    private void scanSector(int cx, int cy, int m1, int m2, int m3, int m4)
    {
        obs.reset();
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

                    if (obs.isBlocked(a0) && obs.isBlocked(a1) && obs.isBlocked(a2))
                    {
                        // Do nothing					
                    }
                    else
                    {
                        fieldOfView[pos] = true;
                    }

                    if (mapData[y,x] == 0)
                    {
                        obs.add(a1, a2);
                    }

                }
            }
            obs.nextRow();
        }
    }
}

public class Obstacles
{

    private static int SIZE = 8 * 8 / 2;
    private static float[] a1 = new float[SIZE];
    private static float[] a2 = new float[SIZE];

    private int length;
    private int limit;

    public void reset()
    {
        length = 0;
        limit = 0;
    }

    public void add(float o1, float o2)
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

    public bool isBlocked(float a)
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

    public void nextRow()
    {
        limit = length;
    }
}


// 2018 - 08 - 17 픽셀던전 깃헙에서 배껴옴!! 어케돌아가는 로직인지도 모름!! 
// 꼭 시간날때마다 공부해보자!!