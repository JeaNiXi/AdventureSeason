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

    Coroutine SearchRoutine;

    [SerializeField] private DirectionEnum ObjectDirection;
    [SerializeField] private Movement ObjectMovement = Movement.ALLOWED;
    [SerializeField] private BattleState ObjectBattleState = BattleState.DISABLED;

    [Space]
    [SerializeField] Transform Hero;
    [Space]

    [SerializeField] private Transform FOVCastPoint;
    [SerializeField] private Transform NearAttackCastPoint;

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

    private float _nearAttackDistance;
    public float NearAttackDistance { get => _nearAttackDistance; set => _nearAttackDistance = value; }

    private float _viewDistance;
    public float ViewDistance { get => _viewDistance; }

    private float _attackInterval;
    public float AttackInterval { get => _attackInterval; }

    private float _searchDuration;
    public float SearchDuration { get => _searchDuration; }

    private float _health;
    public float Health { get => _health; }

    private float _collisionDamage;
    public float CollisionDamage { get => _collisionDamage; }


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

    private bool RoutineCheck;
    //6 Ground Layer 8 Player Layer

    int hitMask = (1 << 6) | (1 << 8);

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
        _nearAttackDistance = sObject.NearAttackDistance;
        _viewDistance = sObject.ViewDistance;
        _attackInterval = sObject.AttackInterval;
        _searchDuration = sObject.SearchDuration;
        _health = sObject.Health;
        _collisionDamage = sObject.CollisionDamage;

        CanAttack = true;
        
    }

    protected void SetSpriteDirection(Vector2 direction)
    {

        if (direction == Vector2.left)
        {
            objSpriteRenderer.flipX = true;
            //_viewDistance = -Mathf.Abs(_viewDistance);
            NearAttackDistance = -Mathf.Abs(NearAttackDistance);
        }
        if (direction == Vector2.right)
        {
            objSpriteRenderer.flipX = false;
            //_viewDistance = Mathf.Abs(_viewDistance);
            NearAttackDistance = Mathf.Abs(NearAttackDistance);
        }
    }

    // Behaviour
    protected void MoveStandPatrol()
    {
        if (ObjectBattleState == BattleState.DISABLED)
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
        // working -- RaycastHit2D rayHit = Physics2D.Linecast(FOVCastPoint.transform.position, new Vector2(FOVCastPoint.transform.position.x+_viewDistance, FOVCastPoint.transform.position.y), playerLayer);
        // working -- RaycastHit2D rayHit = Physics2D.Raycast(FOVCastPoint.transform.position, Direction, 10f, playerLayer);
        RaycastHit2D rayHit = Physics2D.Raycast(FOVCastPoint.transform.position, Direction, ViewDistance, hitMask);
        Debug.DrawRay(FOVCastPoint.transform.position, Direction * ViewDistance, Color.white);

        if(rayHit.collider!=null && rayHit.collider.CompareTag("Ground"))
        {
            if (GameManager.Instance.GPState == GameManager.GlobalPlayerState.DEAD && ObjectBattleState == BattleState.ENABLED) 
            {
                Debug.Log("Object dead, stopping");
                if (RoutineCheck)
                {
                    StopCoroutine(SearchRoutine);
                }
                FullyStopSearch();
            }
            else
            {
                return;
            }
        }
        else
        if (rayHit.collider != null && ObjectBattleState == BattleState.DISABLED)
        {
            ObjectBattleState = BattleState.ENABLED;
            Debug.Log("Found Player");
        }
        if (rayHit.collider == null && ObjectBattleState == BattleState.ENABLED && !IsSearching) 
        {
            //ObjectBattleState = BattleState.DISABLED;
            //MoveCoefficient = 1f;
            //Debug.Log("Lost Player");
            Debug.Log("Lost Player, started search.");
            TryFindLostPlayer();
        }
        if(rayHit.collider!=null && ObjectBattleState == BattleState.ENABLED && IsSearching)
        {
            //if (_searchCoroutineRunning)
            //{
                Debug.Log("Object found Stopping Coroutine");
                StopCoroutine(SearchRoutine);
                FullyStopSearch();
            //}
        }
        //if (ObjectBattleState == BattleState.ENABLED && !IsSearching && GameManager.Instance.GPState == GameManager.GlobalPlayerState.DEAD)
        //{
        //    Debug.Log("Object dead, stopping");
        //    StopCoroutine(SearchRoutine);
        //    FullyStopSearch();
        //}
    }

    //Attack State

    protected void UpdateBattleState()
    {
        if(ObjectBattleState==BattleState.ENABLED)
        {
            RaycastHit2D rayAttackHit = Physics2D.Linecast(NearAttackCastPoint.transform.position, new Vector2(NearAttackCastPoint.transform.position.x + NearAttackDistance, NearAttackCastPoint.transform.position.y), playerLayer);
            if (rayAttackHit.collider != null && !IsAttacking && CanAttack)
            {
                IsMoving = false;
                CanAttack = false;
                Attack(); 
            }
            if (rayAttackHit.collider == null && !IsAttacking)
                TryCatchPlayer();
            else
            {
                return;
            }
        }
        else
        {

            return;
        }
    }

    private void TryCatchPlayer()
    {
        if (objObstaclesDetectionScript.IsGrounded)
        {
            IsMoving = true;
            MoveCoefficient = 2.5f;
            objRigidBody.velocity = new Vector2(Direction.x * MoveSpeed * MoveCoefficient, objRigidBody.velocity.y);
            ChangeDirectionOnSearch();
        }
        else
        {
            IsMoving = false;
        }
    }
    private void TryFindLostPlayer()
    {
        IsSearching = true;

        SearchRoutine = StartCoroutine(SearchCooldown(SearchDuration));
    }
    private void ChangeDirectionOnSearch()
    {
        if (Hero.transform.position.x - gameObject.transform.position.x >= 2 && objObstaclesDetectionScript.IsGrounded)
        {
            ObjectDirection = DirectionEnum.RIGHT;
        }
        else if (Hero.transform.position.x - gameObject.transform.position.x <-2 && objObstaclesDetectionScript.IsGrounded)
        {
            ObjectDirection = DirectionEnum.LEFT;
        }
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
    private void FullyStopSearch()
    {
        ObjectBattleState = BattleState.DISABLED;
        IsSearching = false;
        MoveCoefficient = 1f;
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
        RoutineCheck = true;
        Debug.Log("Search Coro Started");
        yield return new WaitForSeconds(duration);
        FullyStopSearch();
        Debug.Log("Search Coro Ended");
        RoutineCheck = false;
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
        Gizmos.color = Color.green;
        Gizmos.DrawLine(NearAttackCastPoint.transform.position, new Vector2(NearAttackCastPoint.transform.position.x + NearAttackDistance, NearAttackCastPoint.transform.position.y));
    }
}

