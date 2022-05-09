using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] EnemySObject sObject;
    private enum DirectionEnum 
    {   
        LEFT,
        RIGHT
    };
    private enum Movement
    {
        ALLOWED,
        RESTRICTED
    }

    [SerializeField] private DirectionEnum ObjectDirection;
    private Movement ObjectMovement = Movement.ALLOWED;
    protected Vector2 Direction
    {
        get
        {
            if(ObjectDirection==DirectionEnum.RIGHT)
            {
                return Vector2.right;
            }
            else
            {
                return Vector2.left;
            }
        }
    }


    Rigidbody2D objRigidBody;
    Animator objAnimator;
    SpriteRenderer objSpriteRenderer;
    ObstaclesDetection objObstaclesDetectionScript;


    

    //
    /// <MovingBehaviour>
    /// Use MoveStandPatrol so the enemy may move from place to place and search for player.
    /// Use StandPatrol so the enemy stands in one place, but starts combat if player comes near.
    /// Use SearchForEnemy so the enemy searches in real-time for enemy near.
    /// Using of StartCombat to check for "player present" and search a way to defeat player.
    /// Return to patrol after player lost.
    /// Return to patrol point after player lost.
    /// </summary>
    //enum Direction { RIGHT, LEFT};

    //SO variables
    private string _name;
    public string Name { get => _name; }

    private float _moveSpeed;
    public float MoveSpeed { get => _moveSpeed; }

    private bool _isStandingStill;
    public bool IsStandingStill { get => _isStandingStill; }

    private bool _canStandPartol;
    public bool CanStandPatrol { get => _canStandPartol; }

    private float _patrolStandingTime;
    public float PatrolStandingTime { get => _patrolStandingTime; }

    // State Variables
    private bool _isMoving;
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    // Code Private Variables
    private Vector2 _moveDirection;

    private float _moveCoefficient = 1.0f;

    protected void InitializeObject()
    {
        objRigidBody = gameObject.GetComponent<Rigidbody2D>();
        objAnimator = gameObject.GetComponentInChildren<Animator>();
        objSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        objObstaclesDetectionScript = gameObject.GetComponent<ObstaclesDetection>();
        InitSO();
    }
    private void InitSO()
    {
        _name = sObject.Name;
        _moveSpeed = sObject.MoveSpeed;
        _isStandingStill = sObject.IsStandingStill;
        _canStandPartol = sObject.CanStandPatrol;
        _patrolStandingTime = sObject.PatrolStandingTime;

    }

    protected void SetSpriteDirection(Vector2 direction)
    {
        if (direction == Vector2.left)
            objSpriteRenderer.flipX = true;
        if (direction == Vector2.right)
            objSpriteRenderer.flipX = false;
    }

    // Behaviour
    protected void MoveStandPatrol()
    {
        if (ObjectMovement == Movement.ALLOWED)
        {
            IsMoving = true;
            objRigidBody.velocity = new Vector2(Direction.x * MoveSpeed * _moveCoefficient, objRigidBody.velocity.y);
        }  
    }
    protected void StandPatrol()
    {

    }
    protected void Move()
    {

    }
    protected void CheckForObstacles()
    {
        if (objObstaclesDetectionScript.HasObstacleRight && objObstaclesDetectionScript.HasObstacleRightUp && !objObstaclesDetectionScript.HasObstacleLeft && ObjectDirection == DirectionEnum.RIGHT && ObjectMovement==Movement.ALLOWED) 
        {
            Debug.Log("started check 1");
            ObjectMovement = Movement.RESTRICTED;
            IsMoving = false;
            StartCoroutine(WaitTimeSetDirectionAndMoveState(2f,DirectionEnum.LEFT,Movement.ALLOWED));
        }
        else
        {
            
        }
        if (objObstaclesDetectionScript.HasObstacleLeft && objObstaclesDetectionScript.HasObstacleLeftUp && !objObstaclesDetectionScript.HasObstacleRight && ObjectDirection == DirectionEnum.LEFT && ObjectMovement==Movement.ALLOWED)
        {
            Debug.Log("started check 2");
            ObjectMovement = Movement.RESTRICTED;
            IsMoving = false;
            StartCoroutine(WaitTimeSetDirectionAndMoveState(2f, DirectionEnum.RIGHT, Movement.ALLOWED));
        }
        else
        {

        }
    }

    //IEnumerators
    IEnumerator WaitTimeSetDirectionAndMoveState(float time, DirectionEnum direction, Movement moveState)
    {
        yield return new WaitForSeconds(time);
        ObjectDirection = direction;
        ObjectMovement = moveState;
        Debug.Log($"Changed Direction to {moveState}");
        yield break;
    }
    /////// Animations handler.
    
    protected void UpdateAnimations()
    {
        objAnimator.SetBool("isMoving", IsMoving);
    }
}
