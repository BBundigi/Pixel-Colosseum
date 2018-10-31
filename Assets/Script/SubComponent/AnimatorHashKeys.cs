using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorHashKeys
{
    public static int AnimIsMoveHash, AnimIsAttackHash, AnimIsDeadHash;

    static AnimatorHashKeys()
    {
        AnimIsMoveHash = Animator.StringToHash("IsMove");
        AnimIsAttackHash = Animator.StringToHash("IsAttack");
        AnimIsDeadHash = Animator.StringToHash("IsDie");
    }
}

