using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;

    private Collider2D[] TouchDetectors;

    private Vector2 StartPos;

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
        TouchDetectors = GameObject.FindWithTag("Player").GetComponentsInChildren<BoxCollider2D>();
        ZoomSpeed = 1;
        CameraMoveSpeed = 0.1f;
    }
    void OnEnable()
    {
        if (TouchDetectors != null)
        {
            for (int i = 0; i < TouchDetectors.Length; i++)
            {
                TouchDetectors[i].gameObject.SetActive(true);
            }
        }
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 TargetVector = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } 
        if (Input.touchCount == 1)
        {

            Touch touch = Input.GetTouch(0);
            isDrag = false;

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    {
                        isDrag = true;
                        Vector2 deltaTouchPos = touch.deltaPosition;

                        Camera.main.transform.position += (Vector3)deltaTouchPos * CameraMoveSpeed;
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        if (!isDrag)
                        {
                            Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(touch.position);

                            int x;
                            int y;
                            ConvertTouchPositionToIndexs(WorldPosition, out x, out y);

                            if (x > 0 || y > 0 || x < MapManager.WIDTH || y < MapManager.HEIGH)
                            {
                                switch (MapManager.GetTileState(x, y))
                                {
                                    case eTileState.Movable:
                                        {
                                            PlayerManager.Instance.MovePosition(MapManager.ConvertIndexsToPosition(x, y));
                                            break;
                                        }
                                    case eTileState.Enemy:
                                        {
                                            break;
                                        }
                                    default:
                                        {
                                            break;
                                        }
                                }
                            }
                        }
                        break;
                    }
            }
        }
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 prevTouchPos1 = touch1.position - touch1.deltaPosition;
            Vector2 prevTouchPos2 = touch2.position - touch2.deltaPosition;

            float prevTouchesMag = (prevTouchPos1 - prevTouchPos2).magnitude;

            float TouchesMag = (touch1.position - touch2.position).magnitude;

            float deltaTouchesMag = TouchesMag - prevTouchesMag;

            if (Camera.main.orthographic)
            {
                Camera.main.orthographicSize += deltaTouchesMag * ZoomSpeed;
            }
        }
    }

    private void OnGUI()
    {
        Event m_Event = Event.current;

        if (m_Event.type == EventType.MouseDrag)
        {
            isDrag = true;
            Vector2 deltaMousePos = m_Event.delta;

            Camera.main.transform.position -= (Vector3)deltaMousePos * CameraMoveSpeed;

        }

        if (m_Event.type == EventType.MouseUp)
        {
            if (!isDrag)
            {
                Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                int x;
                int y;
                ConvertTouchPositionToIndexs(WorldPosition, out x, out y);
                if (x > 0 || y > 0 || x < MapManager.WIDTH || y < MapManager.HEIGH)           
                {
                    switch (MapManager.GetTileState(x, y))
                    {
                        case eTileState.Movable:
                            {
                                StartCoroutine(PlayerManager.Instance.MovePosition(MapManager.ConvertIndexsToPosition(x, y)));
                                break;
                            }
                        case eTileState.Enemy:
                            {
                                PlayerManager.Instance.PlayerAttack((EnemyClass)MapManager.GetMapObjects(x, y));
                                break;
                            }
                        default:
                            {
                                break;
                            }
                    }
                }
            }
            isDrag = false;
        }
    }
    private void ConvertTouchPositionToIndexs(Vector2 TargetVector, out int x, out int y)
    {
        x = (int)((TargetVector.x + 6.016f + .32f) / .64f);
        y = (int)((TargetVector.y + 4.12f + .32f) / .64f);
    }
}

//이유는 모르겠는데 Event.mousePosition이 잘 작동을 안함

//알아보기
