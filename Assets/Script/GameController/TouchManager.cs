using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;

    public eTouchMode NowMode
    {
        get
        {
            return nowMode;
        }
        set
        { 
            switch(value)
            {
                case eTouchMode.CloseInventory:
                    {
                        nowMode = value;
                        break;
                    }
                case eTouchMode.GamePlay:
                    {
                        nowMode = value;
                        break;
                    }
                case eTouchMode.None:
                    {
                        nowMode = value;
                        break;
                    }
                case eTouchMode.ChooseItem:
                    {
                        nowMode = value;
                        break;
                    }
                case eTouchMode.ThrowItem:
                    {
                        Debug.Log("Error! you Cant assinged NowMode To ThrowItem");
                        break;
                    }
            }
            isDrag = false;
        }
    }
    private eTouchMode nowMode;

    private eItemID targetItem;

    public eItemID TargetItem
    {
        set
        {
            targetItem = value;
            nowMode = eTouchMode.ThrowItem;
        }
    }

    private Collider2D[] TouchDetectors;

    private Vector2 StartPos;

    private Camera MainCamera;

    private bool isDrag;

    private float ZoomSpeed;

    private float CameraMoveSpeed;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        ZoomSpeed = 1;
        CameraMoveSpeed = 0.05f;
        MainCamera = Camera.main;
    }

    void Update()
    {

        if(Input.GetMouseButtonDown(1))
        {
            int x;
            int y;

            Vector3 MousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            ConvertTouchPositionToIndexs(MousePosition, out x, out y);
            Debug.Log(MapManager.GetTileState(x, y));
        }
    //    if (Input.touchCount == 1)
    //    {

    //        Touch touch = Input.GetTouch(0);
    //        isDrag = false;

    //        switch (touch.phase)
    //        {
    //            case TouchPhase.Moved:
    //                {
    //                    isDrag = true;
    //                    Vector2 deltaTouchPos = touch.deltaPosition;

    //                    MainCamera.transform.position += (Vector3)deltaTouchPos * CameraMoveSpeed;
    //                    break;
    //                }
    //            case TouchPhase.Ended:
    //                {
    //                    if (!isDrag)
    //                    {
    //                        Vector2 WorldPosition = MainCamera.ScreenToWorldPoint(touch.position);

    //                        int x;
    //                        int y;
    //                        ConvertTouchPositionToIndexs(WorldPosition, out x, out y);

    //                        if (x > 0 || y > 0 || x < MapManager.WIDTH || y < MapManager.HEIGH)
    //                        {
    //                            switch (MapManager.GetTileState(x, y))
    //                            {
    //                                case eTileState.Movable:
    //                                    {
    //                                        PlayerManager.Instance.PlayerMove(x,y);
    //                                        break;
    //                                    }
    //                                case eTileState.Enemy:
    //                                    {
    //                                        PlayerManager.Instance.PlayerAttack((EnemyClass)MapManager.GetMapObjects(x, y));
    //                                        break;
    //                                    }
    //                                default:
    //                                    {
    //                                        break;
    //                                    }
    //                            }
    //                        }
    //                    }
    //                    break;
    //                }
    //        }
    //    }
    //    if (Input.touchCount == 2)
    //    {
    //        Touch touch1 = Input.GetTouch(0);
    //        Touch touch2 = Input.GetTouch(1);

    //        Vector2 prevTouchPos1 = touch1.position - touch1.deltaPosition;
    //        Vector2 prevTouchPos2 = touch2.position - touch2.deltaPosition;

    //        float prevTouchesMag = (prevTouchPos1 - prevTouchPos2).magnitude;

    //        float TouchesMag = (touch1.position - touch2.position).magnitude;

    //        float deltaTouchesMag = TouchesMag - prevTouchesMag;

    //        if (MainCamera.orthographic)
    //        {
    //            MainCamera.orthographicSize += deltaTouchesMag * ZoomSpeed;
    //        }
    //    }
    }

    private void OnGUI()
    {
        switch(NowMode)
        {
            case eTouchMode.GamePlay:
                {
                    TouchInGamePlay();
                    break;
                }
            case eTouchMode.CloseInventory:
                {
                    TouchInCloseInventory();
                    break;
                }
            case eTouchMode.ThrowItem:
                {
                    TouchInThrowItem();
                    break;
                }
            case eTouchMode.None:
                {
                    break;
                }
        }
    }
    private void TouchInCloseInventory()
    {
        Event m_Event = Event.current;

        if (m_Event.type == EventType.MouseUp)
        {
            Vector2 mousePosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition,Vector2.zero);

            if(!hit)
            {
                InventoryController.Instance.CloseInventory();
            }
        }
    }
    private void TouchInGamePlay()
    {
        Event m_Event = Event.current;

        if (m_Event.type == EventType.MouseDrag)
        {
            isDrag = true;

            Vector2 MouseMovePositoin = m_Event.delta;

            MouseMovePositoin = new Vector2(-MouseMovePositoin.x, MouseMovePositoin.y);
            MainCamera.transform.position += (Vector3)MouseMovePositoin * CameraMoveSpeed;
        }

        if (m_Event.type == EventType.MouseUp)
        {
            if (!isDrag)
            {
                Vector2 WorldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                int x;
                int y;
                ConvertTouchPositionToIndexs(WorldPosition, out x, out y);
                eTileState tileState = MapManager.GetTileState(x, y);
                if (tileState != eTileState.None)
                {
                    if ((tileState & eTileState.Enemy) == eTileState.Enemy)
                    {
                        PlayerManager.Instance.PlayerAttack((EnemyClass)MapManager.GetMapObjects(x, y));
                        NowMode = eTouchMode.None;
                    }

                    if((tileState & eTileState.Movable) == eTileState.Movable)
                    {
                        PlayerManager.Instance.PlayerMove(x, y);
                        NowMode = eTouchMode.None;
                    }
                }
            }
            else
            {
                isDrag = false;
            }
        }
    }
    private void TouchInThrowItem()
    {
        Event m_Event = Event.current;

        if (m_Event.type == EventType.MouseDrag)
        {
            isDrag = true;
            Vector2 MouseMovePositoin = m_Event.delta;

            MouseMovePositoin = new Vector2(-MouseMovePositoin.x, MouseMovePositoin.y);
            MainCamera.transform.position += (Vector3)MouseMovePositoin * CameraMoveSpeed;
        }

        if (m_Event.type == EventType.MouseUp)
        {
            if (!isDrag)
            {
                Vector2 WorldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
                int x;
                int y;
                ConvertTouchPositionToIndexs(WorldPosition, out x, out y);
                eTileState tileState = MapManager.GetTileState(x, y);
                if ((tileState & eTileState.Wall) != eTileState.Wall && tileState != eTileState.None)
                {
                    ItemManager.UseItem(x, y, targetItem);
                    PopUpTextController.Instance.StopThrowing_OnClicked();
                    nowMode = eTouchMode.None;
                }
            }
            else
            {
                isDrag = false;
            }
        }
    }


    private void ConvertTouchPositionToIndexs(Vector2 TargetVector, out int x, out int y)
    {
        int tempX, tempY;
        float tileGap_Half = MapManager.TILE_GAP / 2;
        MapManager.ConvertPositionToIndexs(TargetVector + new Vector2(tileGap_Half, tileGap_Half), out tempX, out tempY);

        x = tempX;
        y = tempY;
    }
}

//이유는 모르겠는데 Event.mousePosition이 잘 작동을 안함

//알아보기
