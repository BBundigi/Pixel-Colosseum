using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHashKeys : MonoBehaviour {

    public static AnimatorHashKeys Instance;
    [HideInInspector]
    public int AnimIsMoveHash, AnimIsAttackHash, AnimIsDeadHash;
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
        AnimIsMoveHash = Animator.StringToHash("IsMove");
        AnimIsAttackHash = Animator.StringToHash("IsAttack");
        AnimIsDeadHash = Animator.StringToHash("IsDie");
    }
}
