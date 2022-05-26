using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arzued : MonoBehaviour
{
    // Adding basic connections.
    Rigidbody2D ArzuedRigidbody2D;
    CapsuleCollider2D ArzuedCapsuleCollider2D;
    ArzuedAnimations ArzuedAnimationScript;
    ArzuedCollisions ArzuedCollisionsScript;


    enum Movement
    {
        RESTRICTED,
        ALLOWED
    }
    enum Direction
    {
        RIGHT,
        LEFT
    }
    enum Status
    {
        ALIVE,
        DEAD
    }

    [SerializeField] CharacterSObject SObject;
    [Space]
    [SerializeField] Movement arzuedMovement = Movement.ALLOWED;
    [SerializeField] Direction arzuedDirection = Direction.RIGHT;
    [SerializeField] Status arzuedStatus = Status.ALIVE;

    Coroutine HitRoutine;

    [SerializeField] private GameObject InventoryObj;
    [SerializeField] private Slider HPSlider;
    [SerializeField] private Gradient HPGradient;
    [SerializeField] private Image HPImage;

    // Character vars.
    private float _moveSpeed = 10.0f;
    private float _jumpForce = 10.0f;
    private float _wallSlideSpeed = 2.0f;
    private float _wallJumpForce = 10.0f;
    private float _slideTime = 0.6f;
    private float _dashAnimationTime = 0.1f;
    private float _dashStopTime = 0.3f;
    private float _dashImpulse = 10.0f;
    private float _dashUpImpulse = 12.0f;

    

    // Naming core vars.
    private float _xInput;
    private float _yInput;
    private Vector2 _inputVector;
    private Vector2 _wallJumpVector;
    private Quaternion _deathRotation;

    [Space]
    // Booleans
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _isIdle;
    private bool _isMoving;
    private bool _isJumping;
    private bool _isFalling;
    [SerializeField] private bool _isGrabbingEdge;
    [SerializeField] private bool _isHanging;
    private bool _hasDashJumped;
    private bool _isSliding;
    private bool _isDashing;
    private bool _isAttacking;
    private bool _canAttack;
    private bool _isDashAttacking;
    private bool _isHurt;
    private bool _isDead;
    [SerializeField] private bool _canDashAttack;

    private bool _isInPain;
    private bool _canTakeHit = true;
    public bool CanTakeHit { get => _canTakeHit; set => _canTakeHit = value; }
    public bool IsInPain { get => _isInPain; set => _isInPain = value; }
    private bool _painCoroutine;

    public bool _isWallSliding;
    public bool _isWallJumping;

    // Properties
    public bool CanMove
    {
        get
        {
            return _canMove;
        }
        set
        {
            _canMove = value;
        }
    }
    public bool IsIdle
    {
        get
        {
            return _isIdle;
        }
        set
        {
            _isIdle = value;
        }
    }
    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        set
        {
            _isMoving = value;
        }
    }
    public bool IsAbleToMove
    {
        get
        {
            return _isMoving;
        }
    }
    public bool IsJumping
    {
        get
        {
            return _isJumping;
        }
        set
        {
            _isJumping = value;
        }
    }
    public bool IsFalling
    {
        get
        {
            return _isFalling;
        }
        set
        {
            _isFalling = value;
        }
    }
    public bool IsWallSliding
    {
        get
        {
            return _isWallSliding;
        }
        set
        {
            _isWallSliding = value;
        }
    }
    public bool IsGrabbingEdge
    {
        get
        {
            return _isGrabbingEdge;
        }
        set
        {
            _isGrabbingEdge = value;
        }
    }
    public bool IsHanging
    {
        get
        {
            return _isHanging;
        }
        set
        {
            _isHanging = value;
        }
    }
    public bool IsSliding
    {
        get
        {
            return _isSliding;
        }
        set
        {
            _isSliding = value;
        }
    }
    public bool IsDashing
    {
        get
        {
            return _isDashing;
        }
        set
        {
            _isDashing = value;
        }
    }
    public bool IsAttacking
    {
        get
        {
            return _isAttacking;
        }
        set
        {
            _isAttacking = value;
        }
    }
    public bool CanAttack
    {
        get
        {
            return _canAttack;
        }
        set
        {
            _canAttack = value;
        }
    }
    public bool IsDashAttacking
    {
        get
        {
            return _isDashAttacking;
        }
        set
        {
            _isDashAttacking = value;
        }
    }
    public bool CanDashAttack
    {
        get
        {
            return _canDashAttack;
        }
        set
        {
            _canDashAttack = value;
        }
    }
    public bool IsHurt
    {
        get
        {
            return _isHurt;
        }
        set
        {
            _isHurt = value;
        }
    }
    public bool IsDead
    {
        get
        {
            return _isDead;
        }
        set
        {
            _isDead = value;
        }
    }

    [Space]
    //SO variables
    private string _name;
    public string Name { get => _name; }
    private float _health;
    public float Health { get => _health; set => _health = value; }
    private float _baseDamage;
    public float BaseDamage { get => _baseDamage; }
    private float _heavyDamage;
    public float HeavyDamage { get => _heavyDamage; }

    // Character Variables

    private float _damageModificator = 1;
    public float DamageModificator { get => _damageModificator; set => _damageModificator = value; }
    private float _heavyDamageModificator = 1;
    public float HeavyDamageModificator { get => _heavyDamageModificator; set => _heavyDamageModificator = value; }
    private float _maxHealth;
    public float MaxHealth { get => _maxHealth; set => _maxHealth = value; }
    
    private void Start()
    {
        Initialize();
        //Time.timeScale = 0.2f;
    }

    private void Initialize()
    {
        ArzuedRigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        ArzuedCapsuleCollider2D = gameObject.GetComponent<CapsuleCollider2D>();
        ArzuedAnimationScript = gameObject.GetComponentInChildren<ArzuedAnimations>();
        ArzuedCollisionsScript = gameObject.GetComponent<ArzuedCollisions>();
        InitSO();
        InitHealthBar();
        StartGame();
    }

    private void InitSO()
    {
        _name = SObject.Name;
        _health = SObject.Health;
        _baseDamage = SObject.BaseDamage;
        _heavyDamage = SObject.HeavyDamage;
    }
    private void InitHealthBar()
    {
        MaxHealth = Health;
        UpdateHealthSlider();
        HPImage.color = HPGradient.Evaluate(1f);
    }
    private void UpdateHealthSlider()
    {
        HPSlider.maxValue = MaxHealth;
        HPSlider.value = Health;
        HPImage.color = HPGradient.Evaluate(HPSlider.normalizedValue);
    }
    private void StartGame()
    {
        _canMove = true;
        _canAttack = true;
    }

    private void Update()
    {

        if (arzuedStatus == Status.ALIVE || !IsDead)
        {
            CheckHealthStatus();
            //InventoryCheck();
            UpdateHealthSlider();
        }

        // Input Controller Below
        if (arzuedMovement == Movement.RESTRICTED && arzuedStatus == Status.ALIVE)
        {
            _xInput = 0;
            if (Input.GetButtonDown("Jump") && !IsHurt)
            {
                IsHanging = false;
                IsGrabbingEdge = false;
                IsJumping = true;
                arzuedMovement = Movement.ALLOWED;
                Jump(Vector2.up, _jumpForce);
            }
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !IsHurt)
            {
                arzuedMovement = Movement.ALLOWED;
            }
            if (IsFalling && !IsHurt)
            {
                IsHanging = false;
                IsGrabbingEdge = false;
                arzuedMovement = Movement.ALLOWED;
            }
            if (IsHurt)
            {
                arzuedMovement = Movement.ALLOWED;
                IsHanging = false;
                IsGrabbingEdge = false;
                IsFalling = true;
                IsHurt = false;
            }
        }
        else
        {
            if (!IsAttacking && !IsHurt && arzuedStatus != Status.DEAD)
            {
                ArzuedRigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                IsHanging = false;
                IsGrabbingEdge = false;
            }

        }
        // Basic horizontal movement.
        if (arzuedMovement != Movement.RESTRICTED && CanMove)
        {
            _xInput = Input.GetAxis("Horizontal");
            _yInput = Input.GetAxis("Vertical");
            _inputVector = new Vector2(_xInput, _yInput);

        }
        if ((!_isWallSliding || (!ArzuedCollisionsScript.IsGrounded && _inputVector.y != 0)) && !IsHurt && arzuedStatus != Status.DEAD)
        {
            Move(_inputVector);
        }
        // Check for jumping.
        if (Input.GetButtonDown("Jump") && ArzuedCollisionsScript.IsGrounded && arzuedMovement != Movement.RESTRICTED && !IsSliding && !IsDashing && CanMove && !IsAttacking && arzuedStatus != Status.DEAD)
        {
            Jump(Vector2.up, _jumpForce);
        }
        // Check for jumping from wall.
        if (Input.GetButtonDown("Jump") && _isWallSliding && arzuedMovement != Movement.RESTRICTED && arzuedStatus != Status.DEAD)
        {
            WallJump();
        }
        // Check for dashing while jumping.
        if (Input.GetButtonDown("Fire2") && !ArzuedCollisionsScript.IsGrounded && !_hasDashJumped && !IsWallSliding && !IsHanging && arzuedStatus != Status.DEAD)
        {
            DashJump();
        }
        // Check for sliding.
        if (Input.GetKeyDown(KeyCode.LeftControl) && ArzuedCollisionsScript.IsGrounded && !IsHanging && Mathf.Abs(ArzuedRigidbody2D.velocity.y) < 1f && !IsIdle && !IsWallSliding && !IsDashing && !IsSliding && CanMove && arzuedStatus != Status.DEAD)
        {
            Slide();
        }
        // Check for sprinting
        if (Input.GetKeyDown(KeyCode.LeftShift) && ArzuedCollisionsScript.IsGrounded && !IsHanging && Mathf.Abs(ArzuedRigidbody2D.velocity.y) < 1f && !IsIdle && !IsWallSliding && !IsDashing && !IsSliding && CanMove && arzuedStatus != Status.DEAD)
        {
            Dash();
        }
        // Check for Jumping/Falling
        if (ArzuedRigidbody2D.velocity.y > 0 && !ArzuedCollisionsScript.IsGrounded && !IsHurt)
        {
            _isJumping = true;
            _isFalling = false;
            _isIdle = false;
            IsSliding = false;
            IsHanging = false;
            CanMove = true;
        }
        if (ArzuedRigidbody2D.velocity.y < 0 && !ArzuedCollisionsScript.IsGrounded && !IsHurt)
        {
            _isFalling = true;
            _isJumping = false;
            _isMoving = false;
            _isIdle = false;
            _isSliding = false;
            CanMove = true;
        }
        // Check for sliding or dashing up or down.
        if (Mathf.Abs(ArzuedRigidbody2D.velocity.y) > 1f && ArzuedCollisionsScript.IsGrounded && (IsSliding || IsDashing) && arzuedStatus != Status.DEAD)
        {
            if (IsSliding)
            {
                IsSliding = false;
            }
            else
            {
                IsDashing = false;
            }
            ArzuedRigidbody2D.velocity = Vector2.zero;
        }
        // Check if character is grounded.
        if (ArzuedCollisionsScript.IsGrounded)
        {
            _isFalling = false;
            _isJumping = false;
            _isWallSliding = false;
            ResetDash();
        }
        else
        {
            _isIdle = false;
        }
        // Check if character is idle.
        if (_inputVector == Vector2.zero && !_isFalling && !IsAttacking && !IsHurt)
        {
            _isIdle = true;
        }
        // Check for wall sliding.
        if ((ArzuedCollisionsScript.IsOnLeftWall && _inputVector.x < 0 && !ArzuedCollisionsScript.IsGrounded && !ArzuedCollisionsScript.IsHittingHead && !IsHurt) || (ArzuedCollisionsScript.IsOnRightWall && _inputVector.x > 0 && !ArzuedCollisionsScript.IsGrounded && !ArzuedCollisionsScript.IsHittingHead && !IsHurt))
        {
            _isWallSliding = true;
            _isFalling = false;
            WallSlide();
        }
        else if (!ArzuedCollisionsScript.IsGrounded && !IsHurt)
        {
            _isWallSliding = false;
            _isFalling = true;
        }
        else
        {
            _isWallSliding = false;
        }
        if (((ArzuedCollisionsScript.IsGrabbingRight && arzuedDirection == Direction.RIGHT) || (ArzuedCollisionsScript.IsGrabbingLeft && arzuedDirection == Direction.LEFT)) && !IsJumping && Mathf.Abs(_inputVector.x) > 0 && !IsHurt && arzuedStatus != Status.DEAD)
        {
            GrabEdge();
        }

        // Attacks Start Here
        // Checking for attack
        if (Input.GetButtonDown("Fire1") && ArzuedCollisionsScript.IsGrounded && !IsAttacking && !IsSliding && !IsHanging && !IsGrabbingEdge && !IsFalling && !IsJumping && CanAttack && !IsDashAttacking && arzuedStatus != Status.DEAD)
        {
            if (!IsDashing)
            {
                ArzuedRigidbody2D.velocity = Vector2.zero;
                //ArzuedRigidbody2D.bodyType = RigidbodyType2D.Static;
                IsIdle = false;
                CanMove = false;
                IsMoving = false;
                IsAttacking = true;
            }
        }
        if (Input.GetButtonDown("Fire1") && CanDashAttack)
        {
            IsDashAttacking = true;
        }
    }

    // Basic Movement Functions.
    private void Move(Vector2 direction)
    {
        if (!_canMove)
        {
            return;
        }
        if (_inputVector.x > 0)
        {
            IsAttacking = false;
            arzuedDirection = Direction.RIGHT;
            ArzuedAnimationScript.Flip(false);
            _isMoving = true;
            _isIdle = false;
        }
        else if (_inputVector.x < 0)
        {
            IsAttacking = false;
            arzuedDirection = Direction.LEFT;
            ArzuedAnimationScript.Flip(true);
            _isMoving = true;
            _isIdle = false;
        }
        else
        {
            _isMoving = false;
        }
        ArzuedRigidbody2D.velocity = new Vector2(_inputVector.x * _moveSpeed, ArzuedRigidbody2D.velocity.y);
    }
    private void Jump(Vector2 direction, float jumpForce)
    {
        ArzuedRigidbody2D.velocity = direction * jumpForce;
    }
    private void WallSlide()
    {
        ResetDash();

        if (_inputVector.x != 0 && !_isWallJumping && ArzuedRigidbody2D.velocity.y <= 0)
        {
            ArzuedRigidbody2D.velocity = Vector2.up * -_wallSlideSpeed;
        }
    }
    private void WallJump()
    {
        ResetDash();
        _isWallJumping = true;
        if (ArzuedCollisionsScript.IsOnLeftWall)
        {
            _wallJumpVector = Vector2.right;
            StartCoroutine(DisableMovement(.3f));
            Jump(_wallJumpVector / 2 + Vector2.up, _wallJumpForce);
        }
        if (ArzuedCollisionsScript.IsOnRightWall)
        {
            _wallJumpVector = Vector2.left;
            StartCoroutine(DisableMovement(.3f));
            Jump(_wallJumpVector / 2 + Vector2.up, _wallJumpForce);
        }
    }
    private void GrabEdge()
    {
        ResetDash();

        _isFalling = false;
        _isWallSliding = false;
        _isMoving = false;
        _isIdle = false;
        if (!IsHanging)
        {
            _isGrabbingEdge = true;
        }

        arzuedMovement = Movement.RESTRICTED;

        ArzuedRigidbody2D.bodyType = RigidbodyType2D.Kinematic;
        ArzuedRigidbody2D.velocity = Vector2.zero;
    }
    private void DashJump()
    {
        _hasDashJumped = true;
        ArzuedRigidbody2D.velocity = Vector2.zero;
        ArzuedRigidbody2D.velocity += Vector2.up * _dashUpImpulse;
    }
    private void ResetDash()
    {
        _hasDashJumped = false;
    }
    private void Slide()
    {
        CanMove = false;
        IsSliding = true;
        StartCoroutine(StopSliding(_slideTime));
    }
    private void Dash()
    {
        CanMove = false;
        CanAttack = false;
        IsDashing = true;
        CanDashAttack = true;
        StartCoroutine(DashController());
    }
    // ATTACK ETC
    internal void Attack(float damage, float modificator)
    {
        if(ArzuedCollisionsScript.attackedEnemiesRight.Length!= 0 && arzuedDirection==Direction.RIGHT)
        {
            foreach(Collider2D enemy in ArzuedCollisionsScript.attackedEnemiesRight)
            {
                enemy.GetComponent<BaseEnemy>().TakeHit(damage * modificator);
            }
        }
        if (ArzuedCollisionsScript.attackedEnemiesLeft.Length != 0 && arzuedDirection == Direction.LEFT)
        {
            foreach (Collider2D enemy in ArzuedCollisionsScript.attackedEnemiesLeft)
            {
                enemy.GetComponent<BaseEnemy>().TakeHit(damage * modificator);
            }
        }
    }
    // ENUMERATORS
    private IEnumerator DashController()
    {
        while (!IsDashAttacking)
        {
            if (arzuedDirection == Direction.RIGHT)
            {
                ArzuedRigidbody2D.AddForce(Vector2.right * _dashImpulse, ForceMode2D.Impulse);
            }
            else
            {
                ArzuedRigidbody2D.AddForce(Vector2.left * _dashImpulse, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(_dashAnimationTime);
            IsDashing = false;
            yield return new WaitForSeconds(_dashStopTime - _dashAnimationTime);
            if (arzuedStatus != Status.DEAD)
                ArzuedRigidbody2D.velocity = Vector2.zero;
            yield break;
        }
        yield break;
    }
    private IEnumerator StopSliding(float slideTime)
    {
        yield return new WaitForSeconds(slideTime);
        IsSliding = false;
        yield break;
    }
    private IEnumerator DisableMovement(float time)
    {
        _canMove = false;
        if (ArzuedCollisionsScript.IsOnLeftWall)
        {
            ArzuedAnimationScript.Flip(false);
        }
        else
        {
            ArzuedAnimationScript.Flip(true);
        }
        yield return new WaitForSeconds(time);
        _isWallJumping = false;
        _canMove = true;
        yield break;
    }
    private IEnumerator HurtDelay(float delay)
    {
        Debug.Log("Started Hert coro");
        _canTakeHit = false;
        yield return new WaitForSeconds(delay);
        Debug.Log("Ended hert coro");
        _canTakeHit = true;
        yield break;
    }
    // Collidders
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _canTakeHit)
        {
            //TakeHit(collision);
            //HitPhysics();
            //EnemyHitPhysics(collision);
        }
        if (arzuedStatus == Status.DEAD && collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(ArzuedCapsuleCollider2D, collision.collider);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && _canTakeHit)
        {
            //TakeHit(collision);
            //HitPhysics();
        }
        if (arzuedStatus == Status.DEAD && collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(ArzuedCapsuleCollider2D, collision.collider);
        }
    }

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Enemy"))
    //    {
    //        _painCoroutine = false;
    //        StopCoroutine(HitRoutine);
    //        IsInPain = false;

    //        Debug.Log("Routine ended");
    //    }
    //}
    internal void TakeHit(Collision2D collision) // add collision hit read
    {
        if (!IsHurt)
        {
            TakeDamage(collision.gameObject.GetComponent<BaseEnemy>().CollisionDamage);
            HitRoutine = StartCoroutine(HurtDelay(0.6f));
            IsIdle = false;
            IsMoving = false;
            IsFalling = false;
            IsJumping = false;
            IsAttacking = false;

            IsHurt = true;
        }
    }
    internal void TakeHit(float damage)
    {
        TakeDamage(damage);
        IsIdle = false;
        IsMoving = false;
        IsFalling = false;
        IsJumping = false;
        IsAttacking = false;

        IsHurt = true;
    }
    private void HitPhysics() //fix velocity for static body while attack (fixed? )
    {
        float bounce = 4f;
        if (!IsFalling)
        {
            ArzuedRigidbody2D.velocity = Vector2.up * bounce;
        }
    }
    private void EnemyHitPhysics(Collision2D collision)
    {
        float bounce = 4f;
        collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * bounce;
    }
    private Vector2 GetCollisionDirection()
    {
        if (arzuedDirection == Direction.RIGHT)
            return Vector2.left;
        else
            return Vector2.right;
    }
    ////
    ////// GAME MECHANICS BELOW
    ////
    //private void InventoryCheck()
    //{
    //    if (Input.GetKeyDown(KeyCode.I) && !InventoryObj.activeSelf)
    //    {
    //        GameManager.Instance.PauseGame(true);
    //        InventoryObj.SetActive(true);
    //    }
    //    else if(Input.GetKeyDown(KeyCode.I) && InventoryObj.activeSelf)
    //    {
    //        CloseInventory();
    //    }
    //}
    //public void CloseInventory()
    //{
    //    GameManager.Instance.PauseGame(false);
    //    InventoryObj.SetActive(false);
    //}
    private void CheckHealthStatus()
    {
        if (Health <= 0)
        {
            SetDeadStatus();
        }
    }
    private void SetDeadStatus()
    {
        bool foundRot = false;
        Debug.Log("is Dead");

        arzuedStatus = Status.DEAD;
        arzuedMovement = Movement.RESTRICTED;
        if (IsHanging)
        {
            CanMove = false;
            IsHanging = false;
            IsGrabbingEdge = false;
            IsSliding = false;
        }
        RaycastHit2D deathHit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 50f);
        //Debug.DrawRay(gameObject.transform.position, Vector2.down, Color.yellow);
        if (deathHit.collider != null && deathHit.collider.gameObject.CompareTag("Ground"))
        {
            foundRot = true;
            _deathRotation = deathHit.collider.gameObject.transform.rotation;
            Debug.Log($" rotation is {_deathRotation}");
        }

        if (foundRot && ArzuedCollisionsScript.IsGrounded)
        {
            _inputVector = Vector2.zero;
            ArzuedCapsuleCollider2D.enabled = false;
            ArzuedRigidbody2D.bodyType = RigidbodyType2D.Static;
            gameObject.transform.rotation = _deathRotation;
            GameManager.Instance.GPState = GameManager.GlobalPlayerState.DEAD;
            PlayDeathAnimation();

        }
        //disable collider
        //remove player tag
        //kinematic
    }
    private void PlayDeathAnimation()
    {
        IsMoving = false;
        IsFalling = false;
        IsIdle = false;

        IsDead = true;
    }
    private void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log($"Health Left {Health} ");
    }
}
