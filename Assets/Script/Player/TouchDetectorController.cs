using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetectorController : MonoBehaviour {
    void OnCollisionEnter2D(Collision2D Other)
    {
        if(Other.collider.CompareTag("EnemyBCForCheckTouch"))
        {
            gameObject.SetActive(false);
        }
    }
}
