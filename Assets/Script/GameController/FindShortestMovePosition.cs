using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindShortestMovePosition
{
    private int[,] MapData;
    private int RowLength;
    private int ColumnLength;

    public FindShortestMovePosition(int[,] TargetMapData)
    {
        MapData = TargetMapData;
        RowLength = TargetMapData.GetLength(0);
        ColumnLength = TargetMapData.GetLength(1);
    }

    public Vector3 GetShortestPositionVector(int FirstIndex, int SecondIndex)
    {
        int NodeNumber = GetNodeNumberFromIndexs(FirstIndex, SecondIndex);
        bool[] isInTheQueue= new bool[MapData.Length];
        int[] Origin = new int[MapData.Length];
        int PlayerNodeNumber;
        Queue Que = new Queue();

        Que.Enqueue(NodeNumber);

        isInTheQueue[NodeNumber] = true;

        while (Que.Peek() == null)
        {
            int TargetNodeNumber = (int)Que.Dequeue();
            int[] Indexs = GetIndexsFromNode(TargetNodeNumber);
            if (MapData[Indexs[0], Indexs[1]] != 2)
            {
                //Check Right - UpperRight - Top - UpperLeft - Left - DownLeft - Bottom -  DownRight
                if (Indexs[0] + 1 < ColumnLength)// Right
                {
                    int TempNode = GetNodeNumberFromIndexs(Indexs[0] + 1, Indexs[1]);
                    if (MapData[Indexs[0] + 1, Indexs[1]] != 0 && isInTheQueue[TempNode])
                    {
                        isInTheQueue[TempNode] = true;
                        Que.Enqueue(TempNode);
                        Origin[TempNode] = TargetNodeNumber;
                    }
                }
                if (Indexs[0] - 1 >= 0)// Left
                {
                    int TempNode = GetNodeNumberFromIndexs(Indexs[0] - 1, Indexs[1]);
                    if (MapData[Indexs[0] - 1, Indexs[1]] != 0 && isInTheQueue[TempNode])
                    {
                        Que.Enqueue(TempNode);
                        Origin[TempNode] = TargetNodeNumber;
                    }
                }
                if (Indexs[1] + 1 < RowLength) // Bottom
                {
                    int TempNode = GetNodeNumberFromIndexs(Indexs[0], Indexs[1] + 1);
                    if (MapData[Indexs[0], Indexs[1] + 1] != 0 && isInTheQueue[TempNode])
                    {
                        
                        Que.Enqueue(TempNode);
                        Origin[TempNode] = TargetNodeNumber;
                    }
                }
                if (Indexs[1] - 1 >= 0) // Top
                {
                    int TempNode = GetNodeNumberFromIndexs(Indexs[0], Indexs[1] - 1);
                    if (MapData[Indexs[0], Indexs[1] - 1] != 0 && isInTheQueue[TempNode])
                    {
                        
                        Que.Enqueue(TempNode);
                        Origin[TempNode] = TargetNodeNumber;
                    }
                }
                if (Indexs[0] - 1 >= 0 && Indexs[1] - 1 >= 0) // UpperLeft
                {
                    int TempNode = GetNodeNumberFromIndexs(Indexs[0] - 1, Indexs[1] - 1);
                    if (MapData[Indexs[0], Indexs[1] - 1] != 0 && isInTheQueue[TempNode])
                    {
                       
                        Que.Enqueue(TempNode);
                        Origin[TempNode] = TargetNodeNumber;
                    }
                }
                if (Indexs[0] + 1 >= ColumnLength && Indexs[1] - 1 >= 0)//UpperRight;
                {
                    int TempNode = GetNodeNumberFromIndexs(Indexs[0] + 1, Indexs[1] - 1);
                    if (MapData[Indexs[0], Indexs[1] - 1] != 0 && isInTheQueue[TempNode])
                    {

                        Que.Enqueue(TempNode);
                        Origin[TempNode] = TargetNodeNumber;
                    }
                }
                if (Indexs[0] + 1 >= ColumnLength && Indexs[1] + 1 >= RowLength)
                {
                    int TempNode = GetNodeNumberFromIndexs(Indexs[0] + 1, Indexs[1] + 1);
                    if (MapData[Indexs[0] + 1, Indexs[1] + 1] != 0 && isInTheQueue[TempNode])
                    {
                       
                        Que.Enqueue(TempNode);
                        Origin[TempNode] = TargetNodeNumber;
                    }
                }

                if (Indexs[0] - 1 >= 0 && Indexs[1] + 1 >= RowLength)
                {
                    int TempNode = GetNodeNumberFromIndexs(Indexs[0] - 1, Indexs[1] + 1);
                    if (MapData[Indexs[0] + 1, Indexs[1] + 1] != 0 && isInTheQueue[TempNode])
                    {
                        
                        Que.Enqueue(TempNode);
                        Origin[TempNode] = TargetNodeNumber;
                    }
                }
            }
            else
            {
                for(int i = 0; i < Que.Count; i ++)
                {
                    Que.Dequeue();
                }
                PlayerNodeNumber = NodeNumber;
                break;
            }
        }

        return Vector3.zero;
    }

    private int[] GetIndexsFromNode(int NodeNumber)
    {
        int iIndex = NodeNumber / ColumnLength;

        int jIndex = NodeNumber % ColumnLength;

        int[] Indexs = new int[2] { iIndex, jIndex };

        return Indexs;
    }

    private int GetNodeNumberFromIndexs(int iIndex, int jIndex)
    {
        return iIndex * ColumnLength + jIndex;
    }
}
