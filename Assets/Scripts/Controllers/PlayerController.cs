using UnityEngine;

public class PlayerController : ControllerBase {
    [Header("Movement Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    private PlayerUI playerUI;
    private MainCamera mainCamera;
    private int health, maxHealth, invisibility, maxInvisibility;
    private float moveInput;
    private bool jumpInput = false, isActiveInvisibility = false;

    private new void Start() {
        base.Start();
        playerUI = GetComponent<PlayerUI>();
        mainCamera = Camera.main.GetComponent<MainCamera>();
        mainCamera.MoveToPlayer();

        maxHealth = playerUI.GetHeartsCount();
        health = PlayerPrefs.GetInt(PlayerPrefsKeys.health);
        if (health == 0) {
            health = maxHealth;
        }

        maxInvisibility = playerUI.GetInvisibilityTokensCount();
        invisibility = PlayerPrefs.GetInt(PlayerPrefsKeys.invisibility);
        playerUI.Initialize();
    }

    private void Update() {
        if (Time.timeScale != 0f) {
            moveInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space)) {
                jumpInput = true;
            }
        }
    }

    private void FixedUpdate() {
        Bounds bounds = collider2d.bounds;

        if (moveInput != 0) {
            if (MoveRequest(IsTouchingObstacle(bounds), moveInput)) {
                rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
                Flip(moveInput);
            }
            animator.SetBool(isRunningText, true);
        } else {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            animator.SetBool(isRunningText, false);
        }

        bool isGrounded = IsGrounded(bounds);
        bool tryJump = jumpInput;
        jumpInput = false;

        if (tryJump && isGrounded) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator.SetBool(isJumpingText, true);
            jumpInput = false;
        }

        HandleLanding(isGrounded);
    }

    public void ChangeHealth(int amount) {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        playerUI.UpdateHeartsUI();

        if (health == 0) {
            GameManager.Instance.Lose();
        } else if (amount < 0) {
            animator.SetBool(isRunningText, false);
            animator.SetBool(isJumpingText, false);
            StartCoroutine(mainCamera.Shake());
            Hit.Instance.ApplyKnockback(rb, GetComponent<SpriteRenderer>());
        }
    }

    public void ChangeInvisibility(int amount) {
        invisibility += amount;
        invisibility = Mathf.Clamp(invisibility, 0, maxInvisibility);
        playerUI.UpdateInvisibilityUI();

        playerUI.CheckInvisibilityReady();
    }

    public int GetHealth() {
        return health;
    }

    public void SetInvisibility(int invisibility) {
        this.invisibility = invisibility;
    }

    public int GetMaxHealth() {
        return maxHealth;
    }

    public int GetInvisibility() {
        return invisibility;
    }

    public int GetMaxInvisibility() {
        return maxInvisibility;
    }

    public bool GetIsActiveInvisibility() {
        return isActiveInvisibility;
    }

    public void SetIsActiveInvisibility(bool isActiveInvisibility) {
        this.isActiveInvisibility = isActiveInvisibility;
    }
}
