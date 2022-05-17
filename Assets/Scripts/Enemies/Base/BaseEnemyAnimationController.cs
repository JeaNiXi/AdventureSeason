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

    public void StopTakeHitBool()
    {
        BaseEnemyScript.TakeHitBool = false;

    }

    public void DestroyObject()
    {
        BaseEnemyScript.DestoyEnemy();
    }

    public void ChangeDirectionOnHit()
    {
        BaseEnemyScript.ObjectBattleState = BaseEnemy.BattleState.ENABLED;
    }

    public void DoAttack()
    {
        BaseEnemyScript.OrganizeAttack();
    }
}
