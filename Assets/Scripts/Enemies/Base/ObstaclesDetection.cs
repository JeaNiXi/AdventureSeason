using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDetection : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask GameGround;

    [Space]
    public bool HasObstacleRight;
    public bool HasObstacleLeft;
    public bool HasObstacleRightUp;
    public bool HasObstacleLeftUp;

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


    private void Start()
    {

    }
    private void Update()
    {
        HasObstacleRight = Physics2D.OverlapBox((Vector2)gameObject.transform.position + rightCubePosition, rightCubeSize, 0f, GameGround);
        HasObstacleLeft = Physics2D.OverlapBox((Vector2)gameObject.transform.position + leftCubePosition, leftCubeSize, 0f, GameGround);
        HasObstacleRightUp = Physics2D.OverlapBox((Vector2)gameObject.transform.position + rightUpperCubePosition, rightUpperCubeSize, 0f, GameGround);
        HasObstacleLeftUp = Physics2D.OverlapBox((Vector2)gameObject.transform.position + leftUpperCubePosition, leftUpperCubeSize, 0f, GameGround);


        Debug.Log(HasObstacleRight);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube((Vector2)transform.position + rightCubePosition, rightCubeSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftCubePosition, leftCubeSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + rightUpperCubePosition, rightUpperCubeSize);
        Gizmos.DrawWireCube((Vector2)transform.position + leftUpperCubePosition, leftUpperCubeSize);

    }
}
