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
    private enum BattleState
    {
        ENABLED,
        DISABLED
    }

    [SerializeField] private DirectionEnum ObjectDirection;
    [SerializeField] private Movement ObjectMovement = Movement.ALLOWED;
    [SerializeField] private BattleState ObjectBattleState = BattleState.DISABLED;

    [Space]
    [SerializeField] Transform Hero;
    [Space]

    [SerializeField] private Transform FOVCastPoint;
    [SerializeField] private Transform NearAttackCastPoint;
    [SerializeField] private float _nearAttackDistance = 2f;
    [SerializeField] private float _viewDistance=20f;

    [SerializeField] private LayerMask playerLayer;
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

    private float _attackInterval;
    public float AttackInterval { get => _attackInterval; }

    private float _searchDuration;
    public float SearchDuration { get => _searchDuration; }

    // State Variables
    private bool _isMoving;
    public bool IsMoving { get => _isMoving; set => _isMoving = value; }

    private bool _isAttacking;
    public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }

    private bool _doAttack1;
    public bool DoAttack1 { get => _doAttack1; set => _doAttack1 = value; }

    private bool _canAttack;
    public bool CanAttack { get => _canAttack; set => _canAttack = value; }

    private bool _isSearching;
    public bool IsSearching { get => _isSearching; set => _isSearching = value; }

    // Code Variables
    private float _moveCoefficient = 1.0f;
    public float MoveCoefficient { get => _moveCoefficient; set => _moveCoefficient = value; }


    [SerializeField] private bool _searchCoroutineRunning;

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
        _attackInterval = sObject.AttackInterval;
        _searchDuration = sObject.SearchDuration;

        CanAttack = true;
        
    }

    protected void SetSpriteDirection(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            objSpriteRenderer.flipX = true;
            _viewDistance = -Mathf.Abs(_viewDistance);
            _nearAttackDistance = -Mathf.Abs(_nearAttackDistance);
        }
        if (direction == Vector2.right)
        {
            objSpriteRenderer.flipX = false;
            _viewDistance = Mathf.Abs(_viewDistance);
            _nearAttackDistance = Mathf.Abs(_nearAttackDistance);
        }
    }

    // Behaviour
    protected void MoveStandPatrol()
    {
        if (ObjectBattleState == BattleState.DISABLED || IsSearching)
        {
            if (ObjectMovement == Movement.ALLOWED && objObstaclesDetectionScript.IsGrounded)
            {
                IsMoving = true;
                objRigidBody.velocity = new Vector2(Direction.x * MoveSpeed * MoveCoefficient, objRigidBody.velocity.y);
            }
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
        if ((ObjectBattleState==BattleState.DISABLED || IsSearching) && objObstaclesDetectionScript.IsGrounded)
        {
            if (objObstaclesDetectionScript.HasObstacleRight && objObstaclesDetectionScript.HasObstacleRightUp && !objObstaclesDetectionScript.HasObstacleLeft && ObjectDirection == DirectionEnum.RIGHT && ObjectMovement == Movement.ALLOWED)
            {
                ObjectMovement = Movement.RESTRICTED;
                IsMoving = false;
                StartCoroutine(WaitTimeSetDirectionAndMoveState(PatrolStandingTime, DirectionEnum.LEFT, Movement.ALLOWED));
            }
            if (objObstaclesDetectionScript.HasObstacleLeft && objObstaclesDetectionScript.HasObstacleLeftUp && !objObstaclesDetectionScript.HasObstacleRight && ObjectDirection == DirectionEnum.LEFT && ObjectMovement == Movement.ALLOWED)
            {
                ObjectMovement = Movement.RESTRICTED;
                IsMoving = false;
                StartCoroutine(WaitTimeSetDirectionAndMoveState(PatrolStandingTime, DirectionEnum.RIGHT, Movement.ALLOWED));
            }
            if (!objObstaclesDetectionScript.HasNoHoleRight && ObjectDirection == DirectionEnum.RIGHT && ObjectMovement == Movement.ALLOWED)
            {
                ObjectMovement = Movement.RESTRICTED;
                IsMoving = false;
                StartCoroutine(WaitTimeSetDirectionAndMoveState(PatrolStandingTime, DirectionEnum.LEFT, Movement.ALLOWED));
            }
            if (!objObstaclesDetectionScript.HasNoHoleLeft && ObjectDirection == DirectionEnum.LEFT && ObjectMovement == Movement.ALLOWED)
            {
                ObjectMovement = Movement.RESTRICTED;
                IsMoving = false;
                StartCoroutine(WaitTimeSetDirectionAndMoveState(PatrolStandingTime, DirectionEnum.RIGHT, Movement.ALLOWED));
            }
        }
        if(!objObstaclesDetectionScript.IsGrounded)
        {
            IsMoving = false;
        }
    }
    //Search for Player

    protected void SearchForPlayer()
    {
        RaycastHit2D rayHit = Physics2D.Linecast(FOVCastPoint.transform.position, new Vector2(FOVCastPoint.transform.position.x+_viewDistance, FOVCastPoint.transform.position.y), playerLayer);
       // if ((rayHit.collider != null && ObjectBattleState==BattleState.DISABLED) || (rayHit.collider != null && IsSearching))
        if ((rayHit.collider != null && ObjectBattleState==BattleState.DISABLED) || (rayHit.collider != null && IsSearching))
        {
            ObjectBattleState = BattleState.ENABLED;
            Debug.Log("Found Player");
            IsSearching = false;
        }
        if (rayHit.collider == null && ObjectBattleState == BattleState.ENABLED && !IsSearching) 
        {
            Debug.Log("Lost Player");
            TryFindLostPlayer();
        }
    }

    //Attack State

    protected void UpdateBattleState()
    {
        if(ObjectBattleState==BattleState.ENABLED)
        {
            RaycastHit2D rayAttackHit = Physics2D.Linecast(NearAttackCastPoint.transform.position, new Vector2(NearAttackCastPoint.transform.position.x + _nearAttackDistance, NearAttackCastPoint.transform.position.y), playerLayer);
            if (rayAttackHit.collider != null && !IsAttacking && CanAttack)
            {
                IsMoving = false;
                CanAttack = false;
                Attack(); 
            }
            if (rayAttackHit.collider == null && !IsAttacking && !IsSearching)
                TryCatchPlayer();
            else
                return;
        }
        else
        {
            return;
        }
    }

    private void TryCatchPlayer()
    {
        IsMoving = true;
        MoveCoefficient = 2.5f;
        objRigidBody.velocity = new Vector2(Direction.x * MoveSpeed * MoveCoefficient, objRigidBody.velocity.y);
    }
    private void TryFindLostPlayer()
    {
        Debug.Log("TryFindLostPlayer Started");
        IsSearching = true;
        StartCoroutine(SearchCooldown(SearchDuration));
    }
    protected void Attack()
    {
        StartCoroutine(WaitBeforeCanAttack(AttackInterval));
        if(!IsAttacking)
        {
            IsAttacking = true;
            DoAttack1 = true;
        }
    }

    //IEnumerators
    IEnumerator WaitTimeSetDirectionAndMoveState(float time, DirectionEnum direction, Movement moveState)
    {
            yield return new WaitForSeconds(time);
            ObjectDirection = direction;
            ObjectMovement = moveState;
            yield break;
    }
    IEnumerator WaitBeforeCanAttack(float time)
    {
        yield return new WaitForSeconds(time);
        CanAttack = true;
        yield break;
    }
    IEnumerator SearchCooldown(float duration)
    {
        _searchCoroutineRunning = true;
        Debug.Log("coro Started");
        yield return new WaitForSeconds(duration);
        if (IsSearching)
        {
            IsSearching = false;
            ObjectBattleState = BattleState.DISABLED;
            MoveCoefficient = 1f;
            Debug.Log("coro ended");
            _searchCoroutineRunning = false;
        }
        _searchCoroutineRunning = false;
        yield break;
    }
    /////// Animations handler.

    protected void UpdateAnimations()
    {
        objAnimator.SetBool("isMoving", IsMoving);
        objAnimator.SetBool("doAttack1", DoAttack1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(FOVCastPoint.transform.position, new Vector2(FOVCastPoint.transform.position.x + _viewDistance, FOVCastPoint.transform.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(NearAttackCastPoint.transform.position, new Vector2(NearAttackCastPoint.transform.position.x + _nearAttackDistance, NearAttackCastPoint.transform.position.y));
    }
}

