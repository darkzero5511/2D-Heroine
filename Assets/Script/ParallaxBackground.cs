using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private Camera mainCamera;
    private float lastMainCameraPositionX;

    [SerializeField] private ParallaxLayer[] backgroundLayers;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        float currentCameraPosstionX = mainCamera.transform.position.x;
        float distanceToMove = currentCameraPosstionX - lastMainCameraPositionX;
        lastMainCameraPositionX = currentCameraPosstionX;

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
        }
    }
}
