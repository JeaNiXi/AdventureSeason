using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : BaseEnemy
{
    private void Start()
    {
        InitializeObject();
    }
    private void Update()
    {
        UpdateAnimations();
        SetSpriteDirection(Direction);
        MoveStandPatrol();
        CheckForObstacles();
        SearchForPlayer();
        UpdateBattleState();
        CheckStatus();
    }
    private void LateUpdate()
    {

    }
}
