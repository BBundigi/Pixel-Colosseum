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
    }

}
