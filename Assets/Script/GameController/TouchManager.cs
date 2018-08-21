using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
    private Collider2D[] TouchDetectors;

    public static TouchManager Instance;

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
	void Start () {
        TouchDetectors = GameObject.FindWithTag("Player").GetComponentsInChildren<BoxCollider2D>();
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
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                if (hit.collider.CompareTag("MoveDetector")
                            && MapManager.GetTileState(hit.transform.position) != eTileState.Wall)
                {
                    StartCoroutine(PlayerManager.Instance.MovePosition(hit.transform.position));
                    enabled = false;
                }
                else if (hit.collider.CompareTag("EnemyBCForCheckTouch"))
                {
                    if (MapManager.GetTileState(hit.transform.position) == eTileState.Enemy)
                    {
                        EnemyClass Target = hit.collider.GetComponentInParent<EnemyClass>();
                        PlayerManager.Instance.PlayerAttack(Target);
                        enabled = false;
                    }
                }
            }
        }
    }
}
