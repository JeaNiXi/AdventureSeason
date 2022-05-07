using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] EnemySObject sObject;
    Rigidbody2D objRigidBody;
    Animator objAnimator;

    //SO variables
    private string _name;
    public string Name { get => _name; }
    private bool _canStandPartol;
    public bool CanStandPatrol { get => _canStandPartol; }
    private float _patrolStandingTime;
    public float PatrolStandingTime { get => _patrolStandingTime; }


    protected void InitializeObject()
    {
        objRigidBody = gameObject.GetComponent<Rigidbody2D>();
        objAnimator = gameObject.GetComponentInChildren<Animator>();
        InitSO();
    }
    private void InitSO()
    {
        _name = sObject.Name;
        _canStandPartol = sObject.CanStandPatrol;
        _patrolStandingTime = sObject.PatrolStandingTime;
    }


    protected void StandPatrol()
    {
        Move(2f); 
    }

    private void Move(float moveTime)
    {
        objAnimator.SetBool("isMoving", true);
        objRigidBody.velocity = new Vector2(4f, objRigidBody.velocity.y);
    }
}
