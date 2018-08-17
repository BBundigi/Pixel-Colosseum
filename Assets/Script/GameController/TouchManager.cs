using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchManager : MonoBehaviour {
    private Collider[] TouchDetectors;
    private int TouchLayer = 512;
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
        TouchDetectors = GameObject.FindWithTag("Player").GetComponentsInChildren<BoxCollider>();
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
            Vector3 MousePosition = Input.mousePosition;
            Vector3 StartPosition = Camera.main.ScreenToWorldPoint(new Vector3(MousePosition.x, MousePosition.y, Camera.main.nearClipPlane));
            Vector3 EndPosition = Camera.main.ScreenToWorldPoint(new Vector3(MousePosition.x, MousePosition.y, Camera.main.farClipPlane));
            Ray Vim = new Ray(StartPosition, EndPosition - StartPosition);
            RaycastHit hit;
            if (Physics.Raycast(Vim, out hit, 100, TouchLayer))
            {
                if (hit.collider.CompareTag("MoveDetector"))
                {
                    StartCoroutine(PlayerManager.Instance.MovePosition(hit.transform.position));
                    enabled = false;
                }
                else if (hit.collider.CompareTag("EnemyBCForCheckTouch"))
                {
                    if ((PlayerManager.Instance.transform.position - hit.transform.position).magnitude < PlayerManager.Instance.AttackRange)
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
