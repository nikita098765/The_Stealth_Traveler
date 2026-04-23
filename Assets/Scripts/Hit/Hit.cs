using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Hit : MonoBehaviour {
    [SerializeField]
    private float knockbackForce, immobilizeTime, flickerInterval;

    public static Hit Instance { get; private set; }

    private PlayerController player;
    private PlayerUI playerUI;
    private Vector2 spikePosition;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        player = GetComponent<PlayerController>();
        playerUI = GetComponent<PlayerUI>();
    }

    public void SetSpikePosition(Vector2 spikePosition) {
        this.spikePosition = spikePosition;
    }

    public void ApplyKnockback(Rigidbody2D rb, SpriteRenderer spriteRenderer) {
        Vector2 direction = new Vector2(Mathf.Sign(rb.position.x - spikePosition.x), 1).normalized;
        rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        StartCoroutine(ImmobilizeAfterHit(spriteRenderer));
    }

    private IEnumerator ImmobilizeAfterHit(SpriteRenderer spriteRenderer) {
        player.enabled = false;
        float elapsed = 0f;
        while (elapsed < immobilizeTime) {
            SetSpriteAlpha(spriteRenderer, 0f);
            yield return new WaitForSeconds(flickerInterval);
            SetSpriteAlpha(spriteRenderer, player.GetIsActiveInvisibility() ? playerUI.GetOpacity() : 1f);
            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval * 2;
        }
        player.enabled = true;
    }

    private void SetSpriteAlpha(SpriteRenderer spriteRenderer, float alpha) {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}
