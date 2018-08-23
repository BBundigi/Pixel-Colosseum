using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ShadowCaster {
    private static GameObject Shadow = Resources.Load<GameObject>("Prefab/shadow");
    private static GameObject[,] Shadows;
    private static int distance;
    private static int[] limits;
    private static bool[] fieldOfView;
    private static int[][] rounding;
    private static int HEIGH;
    private static int WIDTH;

    private int SightDistance;

    private static Obstacles Obs = new Obstacles();


    private static Transform ShadowParent;
    public static void SetShadowCaster()
    {
        HEIGH = MapManager.HEIGH;
        WIDTH = MapManager.WIDTH;
        fieldOfView = new bool[HEIGH * WIDTH];

        ShadowParent = GameObject.FindGameObjectWithTag("ShadowParent").transform;
        Shadows = new GameObject[HEIGH, WIDTH];

        for (int i = 0; i < HEIGH; i++)
        {
            for (int j = 0; j < WIDTH; j++)
            {
                GameObject TempShadow = Object.Instantiate(Shadow, ShadowParent);
                TempShadow.transform.position = MapManager.ConvertIndexsToPosition(j, i);
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

    public static void ShadowCast(Vector2 PlayerPosition, int Sight)
    {
        bool[] FlagShadow = SetShadowFlag(PlayerPosition, Sight);

        for (int i = 0; i < HEIGH; i++)
        {
            for (int j = 0; j < WIDTH; j++)
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

    public static bool[] SetShadowFlag(Vector2 PlayerPosition, int Distance)
    {
        int x, y;
        MapManager.ConvertPositionToIndexs(PlayerPosition, out x, out y);

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

                    if (MapManager.GetTileState(x,y) == eTileState.Wall)
                    {
                        Obs.add(a1, a2);
                    }
                }
            }
            Obs.nextRow();
        }
    }

    public class Obstacles
    {
        private static int SIZE = 8 * 8 / 2;
        private float[] a1 = new float[SIZE];
        private float[] a2 = new float[SIZE];

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
}
