using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDetectorManager : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D Other)
    {
        if(Other.CompareTag("Wall") || Other.CompareTag("EnemyBCForCheckTouch"))
        {
            gameObject.SetActive(false);
        }
    }
}
