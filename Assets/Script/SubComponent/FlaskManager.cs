using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskManager : MonoBehaviour {
    private eItemID PotionID;
    private int DestinationX;
    private int DestinationY;
    private int TurnCount;

    private SpriteRenderer MainSprite;

    private void Awake()
    {
        MainSprite = GetComponent<SpriteRenderer>();
    }

    private void CountTurn()
    {
        TurnCount++;
        UseAbillity();
    }

    public void SetFlask(int _DestinationX,int _DestinationY, eItemID _PotionID)
    {
        DestinationX = _DestinationX;
        DestinationY = _DestinationY;


        VoidCallBack TempCounter = CountTurn;

        TurnManager.Instance.PlayerUpdateLogicAndCount += TempCounter;

        PotionID = _PotionID;
        MainSprite.sprite = ItemInfoManager.Instance.GetItemSprite((PotionID));
        StartCoroutine(ThrowFlask());
    }

    private void UseAbillity()
    {
        switch(PotionID)
        {
            case eItemID.FireFlask:
                {
                    if (TurnCount == 0)
                    {
                        for (int i =  -2; i < 3; i++)
                        {
                            for (int j = -2; j < 3; j++)
                            {
                                eTileState TargetTileState = MapManager.GetTileState(DestinationX + i, DestinationY + j);

                                if (TargetTileState != eTileState.None)
                                {
                                    if ((TargetTileState & eTileState.Wall) != eTileState.Wall)
                                    {
                                        MapManager.AddTileState(DestinationX + i, DestinationY + j, eTileState.OnFire);

                                    }
                                }
                            }
                        }
                    }
                    else if(TurnCount ==4)
                    {
                        for (int i = DestinationX - 2; i < DestinationX + 3; i++)
                        {
                            for (int j = DestinationY - 2; j < DestinationY + 3; j++)
                            {
                                if ((MapManager.GetTileState(DestinationX + i, DestinationY +j) & eTileState.OnFire) == eTileState.OnFire)
                                {
                                    MapManager.RemoveTileState(DestinationX + i, DestinationY + j, eTileState.OnFire);
                                }
                            }
                        }
                    }
                    break;
                }
            case eItemID.PoisonFlask:
                {
                    if (TurnCount <= 5)
                    {
                        Spread(TurnCount,eTileState.OnPoison);
                    }
                    else
                    {
                        RemoveSpraed(5 - (TurnCount - 6), eTileState.OnPoison);
                        if(TurnCount == 9)
                        {
                            Destroy(gameObject);
                        }
                    }
                    break;
                }
        }
    }

    private IEnumerator ThrowFlask()
    {
        transform.position = MapManager.ConvertIndexsToPosition(PlayerManager.Instance.PlayerXPos, PlayerManager.Instance.PlayerYPos);

        Vector3 m_TargetPosition = (Vector3)MapManager.ConvertIndexsToPosition(DestinationX, DestinationY);

        Vector3 m_MovePerSecond = (m_TargetPosition - transform.position) / 20;
        while (Mathf.Round(10 *(transform.position - m_TargetPosition).magnitude) != 0)
        {
            transform.Rotate(new Vector3(0.0f, 0.0f,720.0f) * Time.deltaTime);
            transform.position += m_MovePerSecond;
            yield return null;
        }
        UseAbillity();
        TurnManager.Instance.PlayerTurnEnd();
        transform.position = m_TargetPosition;
        MainSprite.color = Color.clear;
    }

    private void Spread(int Distance,eTileState TargetTileState)
    {
        for(int i =0; i < Distance + 1; i++)
        {
            int LocEnemyXPos = Distance - i + DestinationX;
            int LoclocalYPos = i+ DestinationY;

            MapManager.AddTileState(LocEnemyXPos, LoclocalYPos, TargetTileState);

            if(LocEnemyXPos == 0 && LoclocalYPos == 0)
            {
                return;
            }
            else if(LocEnemyXPos == 0)
            {
                MapManager.AddTileState(LocEnemyXPos, -LoclocalYPos, TargetTileState);
            }
            else if (LoclocalYPos == 0)
            {
                MapManager.AddTileState(-LocEnemyXPos, LoclocalYPos, TargetTileState);
            }
            else
            {
                MapManager.AddTileState(LocEnemyXPos, -LoclocalYPos, TargetTileState);
                MapManager.AddTileState(-LocEnemyXPos, LoclocalYPos, TargetTileState);
                MapManager.AddTileState(-LocEnemyXPos, -LoclocalYPos, TargetTileState);
            }
        }
    }

    private void RemoveSpraed(int Distance,eTileState TargetTileState)
    {
        for (int i = 0; i < Distance + 1; i++)
        {
            int LocEnemyXPos = Distance - i;
            int LoclocalYPos = i;

            MapManager.RemoveTileState(LocEnemyXPos, LoclocalYPos, TargetTileState);

            if (LocEnemyXPos == 0 && LoclocalYPos == 0)
            {
                return;
            }
            else if (LocEnemyXPos == 0)
            {
                MapManager.RemoveTileState(LocEnemyXPos, -LoclocalYPos, TargetTileState);
            }
            else if (LoclocalYPos == 0)
            {
                MapManager.RemoveTileState(-LocEnemyXPos, LoclocalYPos, TargetTileState);
            }
            else
            {
                MapManager.RemoveTileState(LocEnemyXPos, -LoclocalYPos, TargetTileState);
                MapManager.RemoveTileState(-LocEnemyXPos, LoclocalYPos, TargetTileState);
                MapManager.RemoveTileState(-LocEnemyXPos, -LoclocalYPos, TargetTileState);
            }
        }
    }
}
