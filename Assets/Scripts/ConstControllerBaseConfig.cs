using UnityEngine;

[CreateAssetMenu(fileName = "ConstControllerBaseConfig", menuName = "Scriptable Objects/ConstControllerBaseConfig")]
public class ConstControllerBaseConfig : ScriptableObject {
    [field: SerializeField] public float offsetToCheckForTouching { get; private set; }
    [field: SerializeField] public LayerMask whatIsGround { get; private set; }
    [field: SerializeField] public LayerMask whatIsObstacle { get; private set; }
}
