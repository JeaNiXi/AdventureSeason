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
        StandPatrol();
    }
}
