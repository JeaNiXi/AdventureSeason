using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDetection : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask GameGround;
    public LayerMask PlayerLayer;

    [Space]
    public bool HasObstacleRight;
    public bool HasObstacleLeft;
    public bool HasObstacleRightUp;
    public bool HasObstacleLeftUp;
    public bool HasNoHoleRight;
    public bool HasNoHoleLeft;
    public bool IsGrounded;

    public Collider2D IsAttackingRight;
    public Collider2D IsAttackingLeft;

    [Space]
    [Header("Collisions")]

    [SerializeField] private Vector3 rightCubeSize = new Vector3(1f,2f,2f);
    [SerializeField] private Vector2 rightCubePosition = new Vector2(1f, 1f);
    [SerializeField] private Vector3 leftCubeSize = new Vector3(1f, 2f, 2f);
    [SerializeField] private Vector2 leftCubePosition = new Vector2(-1f, 1f);

    [SerializeField] private Vector3 rightUpperCubeSize = new Vector3(1f, 2f, 2f);
    [SerializeField] private Vector2 rightUpperCubePosition = new Vector2(1f, 1.2f);
    [SerializeField] private Vector3 leftUpperCubeSize = new Vector3(1f, 2f, 2f);
    [SerializeField] private Vector2 leftUpperCubePosition = new Vector2(-1f, 1.2f);

    [SerializeField] private Vector3 rightHoleCubeSize = new Vector3(1f, 2f, 2f);
    [SerializeField] private Vector2 rightHoleCubePosition = new Vector2(1f, 0.2f);
    [SerializeField] private Vector3 leftHoleCubeSize = new Vector3(1f, 2f, 2f);
    [SerializeField] private Vector2 leftHoleCubePosition = new Vector2(-1f, 0.2f);

    [SerializeField] private Vector3 groundCheckSize = new Vector3(1f, 2f, 2f);
    [SerializeField] private Vector2 groundCheckPosition = new Vector2(-1f, 3f);

    [SerializeField] private float attackRadius;
    [SerializeField] private Vector2 RightAttackPosition;
    [SerializeField] private Vector2 LeftAttackPosition;

    private void Start()
    {

    }
    private void Update()
    {
        HasObstacleRight = Physics2D.OverlapBox((Vector2)gameObject.transform.position + rightCubePosition, rightCubeSize, 0f, GameGround);
        HasObstacleLeft = Physics2D.OverlapBox((Vector2)gameObject.transform.position + leftCubePosition, leftCubeSize, 0f, GameGround);
        HasObstacleRightUp = Physics2D.OverlapBox((Vector2)gameObject.transform.position + rightUpperCubePosition, rightUpperCubeSize, 0f, GameGround);
        HasObstacleLeftUp = Physics2D.OverlapBox((Vector2)gameObject.transform.position + leftUpperCubePosition, leftUpperCubeSize, 0f, GameGround);
        HasNoHoleRight = Physics2D.OverlapBox((Vector2)gameObject.transform.position + rightHoleCubePosition, rightHoleCubeSize, 0f, GameGround);
        HasNoHoleLeft = Physics2D.OverlapBox((Vector2)gameObject.transform.position + leftHoleCubePosition, leftHoleCubeSize, 0f, GameGround);
        IsGrounded = Physics2D.OverlapBox((Vector2)gameObject.transform.position + groundCheckPosition, groundCheckSize, 0f, GameGround);

        IsAttackingRight = Physics2D.OverlapCircle((Vector2)gameObject.transform.position + RightAttackPosition, attackRadius, PlayerLayer);
        IsAttackingLeft = Physics2D.OverlapCircle((Vector2)gameObject.transform.position + LeftAttackPosition, attackRadius, PlayerLayer);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube((Vector2)transform.position + rightCubePosition, rightCubeSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftCubePosition, leftCubeSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + rightUpperCubePosition, rightUpperCubeSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftUpperCubePosition, leftUpperCubeSize);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube((Vector2)transform.position + rightHoleCubePosition, rightHoleCubeSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftHoleCubePosition, leftHoleCubeSize);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckPosition, groundCheckSize);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere((Vector2)transform.position + RightAttackPosition, attackRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + LeftAttackPosition, attackRadius);

    }
}
