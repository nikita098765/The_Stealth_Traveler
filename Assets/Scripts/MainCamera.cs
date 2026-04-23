using System.Collections;
using UnityEngine;

public class MainCamera : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float minY;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeAmplitude;

    private Transform player;
    private Vector3 shakeOffset = Vector2.zero;

    private void Start() {
        player = FindFirstObjectByType<PlayerController>().transform;
    }

    private void LateUpdate() {
        if (player != null) {
            Vector3 newPosition = Vector3.Lerp(transform.position,
                new Vector3(player.position.x, player.position.y, transform.position.z),
                smoothSpeed * Time.deltaTime);
            newPosition.y = Mathf.Max(newPosition.y, minY);
            transform.position = newPosition + shakeOffset;
            ParallaxManager.Instance.Move();
        }
    }

    public void MoveToPlayer() {
        transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
    }

    public IEnumerator Shake() {
        float elapsed = 0f;
        while (elapsed < shakeDuration) {
            shakeOffset = new Vector3(Random.Range(-shakeAmplitude, shakeAmplitude),
                Random.Range(-shakeAmplitude, shakeAmplitude), 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeOffset = Vector2.zero;
    }
}
