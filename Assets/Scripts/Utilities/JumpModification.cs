using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpModification : MonoBehaviour
{
    [Space]
    [Header("Vars")]
    private float fallMultiplier = 2.5f;
    private float lowJumpMultiplier = 2f;


    Rigidbody2D ArzuedRigidBody2D;

    private void Start()
    {
        ArzuedRigidBody2D = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (ArzuedRigidBody2D.velocity.y < 0)
        {
            ModifyJump(fallMultiplier);
        }
        else if (ArzuedRigidBody2D.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            ModifyJump(lowJumpMultiplier);
        }
    }

    private void ModifyJump(float multiplier)
    {
        ArzuedRigidBody2D.velocity += (multiplier - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
    }
}
