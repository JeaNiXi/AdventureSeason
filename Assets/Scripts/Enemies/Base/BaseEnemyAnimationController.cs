using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemyAnimationController : MonoBehaviour
{
    BaseEnemy BaseEnemyScript;

    private void Start()
    {
        BaseEnemyScript = gameObject.GetComponentInParent<BaseEnemy>();
    }

    public void StopBaseAttack()
    {
        BaseEnemyScript.IsAttacking = false;
        BaseEnemyScript.DoAttack1 = false;
    }
}
