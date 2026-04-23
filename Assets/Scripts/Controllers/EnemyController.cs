using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : ControllerBase {
    [Header("Type Settings")]
    [SerializeField] private Enemy enemyType;

    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [Header("Patrol Are Settings")]
    [SerializeField] private Transform minX;
    [SerializeField] private Transform maxX;
    [SerializeField] private float idleTime;

    [Header("Detection Settings")]
    [SerializeField] private Transform eyePosition;
    [SerializeField] private float obstacleDetectionDistance;
    [SerializeField] private float playerDetectionDistance;
    [SerializeField] private LayerMask whatIsPlayer;

    [Header("After Destroy Settings")]
    [SerializeField] private GameObject bonePrefab;
    [SerializeField] private GameObject destroyEffect;

    private enum Enemy { patrolling, standing }
    private RaycastHit2D eyeRay;

    private PlayerController player;
    private LineRenderer eyeLine;
    private float direction;
    private bool isWaiting = false;
    Vector3 endPosition;

    private new void Start() {
        base.Start();
        player = FindFirstObjectByType<PlayerController>();
        direction = transform.localScale.x;
        eyeLine = GetComponent<LineRenderer>();
        endPosition = eyePosition.position;
        if (enemyType == Enemy.standing) {
            isWaiting = true;
        }
    }

    private void FixedUpdate() {
        Vector2 vectorDirection = direction > 0 ? Vector2.right : Vector2.left;

        eyeRay = Physics2D.Raycast(eyePosition.position, vectorDirection,
            playerDetectionDistance, whatIsObstacle | whatIsPlayer);

        if (eyeRay.collider != null) {
            endPosition = eyeRay.point;
            if (eyeRay.collider.GetComponent<PlayerController>() != null && !player.GetIsActiveInvisibility()) {
                GameManager.Instance.Lose();
            }
        } else {
            endPosition = eyePosition.position + (Vector3)vectorDirection * playerDetectionDistance;
        }

        if (isWaiting) {
            return;
        }

        Bounds bounds = collider2d.bounds;

        if (MoveRequest(IsTouchingObstacle(bounds), direction)) {
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
            animator.SetBool(isRunningText, true);

            if (rb.position.x <= minX.position.x && direction < 0 ||
                rb.position.x >= maxX.position.x && direction > 0) {
                StartCoroutine(WaitAndTurn());
            }
        }
        bool isObstacleDetected = Physics2D.Raycast(new Vector2(bounds.center.x, bounds.min.y),
            vectorDirection, obstacleDetectionDistance, whatIsObstacle);
        bool isGrounded = IsGrounded(bounds);

        if (isObstacleDetected && isGrounded && wasGrounded) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool(isJumpingText, true);
        }

        HandleLanding(isGrounded);
    }

    private void LateUpdate() {
        eyeLine.SetPosition(0, eyePosition.position);
        eyeLine.SetPosition(1, endPosition);
    }

    private IEnumerator WaitAndTurn() {
        isWaiting = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetBool(isRunningText, false);
        yield return new WaitForSeconds(idleTime);
        direction *= -1;
        Flip(direction);
        isWaiting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Bounds bounds = collider2d.bounds;
        if (collision.gameObject.GetComponent<PlayerController>() != null &&
            (eyeRay.collider == null || eyeRay.collider.GetComponent<PlayerController>() == null ||
            player.GetIsActiveInvisibility())) {
            Vector3 boundsCenter = bounds.center;
            Destroy(gameObject);
            Instantiate(destroyEffect, boundsCenter, Quaternion.identity);
            Instantiate(bonePrefab, boundsCenter, Quaternion.identity);
            if (player.GetInvisibility() < player.GetMaxInvisibility()) {
                player.ChangeInvisibility(1);
            }
        }
    }
}
