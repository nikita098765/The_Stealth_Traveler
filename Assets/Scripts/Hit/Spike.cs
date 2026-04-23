using UnityEngine;

public class Spike : MonoBehaviour {
    [SerializeField] private int damage;

    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null) {
            Hit.Instance.SetSpikePosition(transform.position);
            player.ChangeHealth(-damage);
        }
    }
}
