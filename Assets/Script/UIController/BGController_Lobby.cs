using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGController_Lobby : MonoBehaviour {
    private Rigidbody2D Rb;
    [SerializeField]
    private Vector2 ResetPoint;
    [SerializeField]
    private float Speed;
    [SerializeField]
    private string Tag;
	// Use this for initialization
	void Start () {
        Rb = GetComponent<Rigidbody2D>();
        Rb.velocity = Speed * Vector2.down;
	}

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.CompareTag(Tag))
        {
            Rb.position = ResetPoint;
        }
    }
}
