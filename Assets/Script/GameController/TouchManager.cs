using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour
{
    public static TouchManager Instance;

    private Collider2D[] TouchDetectors;

    private  Vector2 StartPos;

    private  bool isMoved;

    private float ZoomSpeed;
    private float CameraMoveSpeed;
    private Transform CameraPosition; 

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
        CameraMoveSpeed = 1;
        CameraPosition = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
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
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    {
                        Vector2 deltaTouchPos = touch.deltaPosition;
                        isMoved = false;
                        CameraPosition.position = deltaTouchPos * CameraMoveSpeed;
                        break;
                    }
                case TouchPhase.Ended:
                    {
                        if (!isMoved)
                        {
                            Vector2 WorldPosition = Camera.main.ScreenToWorldPoint(touch.position);

                            int x;
                            int y;
                            ConvertTouchPositionToIndexs(WorldPosition, out x, out y);

                            switch(MapManager.GetTileState(x,y))
                            {
                                case eTileState.Movable:
                                    {
                                        PlayerManager.Instance.MovePosition(MapManager.ConvertIndexsToPosition(x, y));
                                        break;
                                    }
                                case eTileState.Enemy:
                                    {
                                        //PlayerManager.Instance.PlayerAttack();//How to?
                                        break;
                                    }
                                default:
                                    {
                                        break;
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
    private void ConvertTouchPositionToIndexs(Vector2 TargetVector, out int x, out int y)
    {
         x = (int)((TargetVector.x + 6.016 + 0.32) / 0.64);
         y = (int)((TargetVector.x + 4.12 + 0.32) / 0.64);
    }
}
