using UnityEngine;

public class Bone : MonoBehaviour {
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<PlayerController>() != null) {
            Destroy(gameObject);
            Inventory.Instance.ChangeAmountBones(1);
        }
    }
}
