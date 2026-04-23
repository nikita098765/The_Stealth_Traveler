using UnityEngine;
using System.Collections.Generic;

public class ParallaxManager : MonoBehaviour {
    [System.Serializable]
    public class ParallaxLayer {
        public Transform layerTransform;
        [Range(0f, 1f)] public float parallaxStrength;
    }

    [SerializeField] private List<ParallaxLayer> layers;

    public static ParallaxManager Instance {  get; private set; }

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    public void Move() {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        deltaMovement.y = 0;
        lastCameraPosition = cameraTransform.position;
        foreach (ParallaxLayer layer in layers) {
            layer.layerTransform.position += deltaMovement * layer.parallaxStrength;
        }
    }
}
