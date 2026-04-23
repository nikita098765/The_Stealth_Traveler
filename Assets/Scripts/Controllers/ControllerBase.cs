using UnityEngine;

public abstract class ControllerBase : MonoBehaviour {
    [SerializeField] private ConstControllerBaseConfig controllerConfig;

    protected Rigidbody2D rb;
    protected Collider2D collider2d;
    protected Animator animator;
    protected float offsetToCheckForTouching;
    protected bool facingRight, wasGrounded = true;
    protected LayerMask whatIsGround, whatIsObstacle;

    protected const string isRunningText = "isRunning";
    protected const string isJumpingText = "isJumping";

    protected void Start() {
        rb = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();

        facingRight = transform.localScale.x > 0 ? true : false;

        if (controllerConfig != null) {
            offsetToCheckForTouching = controllerConfig.offsetToCheckForTouching;
            whatIsGround = controllerConfig.whatIsGround;
            whatIsObstacle = controllerConfig.whatIsObstacle;
        }
    }

    protected bool IsTouchingObstacle(Bounds bounds) {
        Vector2 boxCenter = (Vector2)bounds.center +
            new Vector2(facingRight ? bounds.extents.x : -bounds.extents.x, 0);
        Vector2 boxSize = new Vector2(offsetToCheckForTouching,
            bounds.size.y - offsetToCheckForTouching);
        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, whatIsObstacle);
    }

    protected bool MoveRequest(bool isTouchingObstacle, float direction) {
        return !isTouchingObstacle || (isTouchingObstacle && (facingRight && direction < 0 ||
            !facingRight && direction > 0));
    }

    protected bool IsGrounded(Bounds bounds) {
        Vector2 boxCenter = new Vector2(bounds.center.x, bounds.min.y + offsetToCheckForTouching);
        Vector2 boxSize = new Vector2(bounds.size.x - offsetToCheckForTouching,
            offsetToCheckForTouching + offsetToCheckForTouching);
        return Physics2D.OverlapBox(boxCenter, boxSize, 0f, whatIsGround);
    }

    protected void HandleLanding(bool isGrounded) {
        if (!wasGrounded && isGrounded) {
            animator.SetBool(isJumpingText, false);
        }
        wasGrounded = isGrounded;
    }

    protected void Flip(float direction) {
        if ((direction > 0 && !facingRight) || (direction < 0 && facingRight)) {
            facingRight = !facingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }
}
