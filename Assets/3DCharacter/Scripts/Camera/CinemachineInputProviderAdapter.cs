using UnityEngine;
using Unity.Cinemachine;

public class CinemachineInputProviderAdapter : MonoBehaviour
{
    [SerializeField] InputManager inputManager;

    private void Awake()
    {
        if (inputManager == null)
            inputManager = FindFirstObjectByType<InputManager>();
    }

    // Example for InputAxisController usage
    public InputAxis horizontalAxis = new InputAxis();
    public InputAxis verticalAxis = new InputAxis();

    private void Update()
    {
        Vector2 delta = inputManager.GetCameraDelta();
        horizontalAxis.Value = delta.x;
        verticalAxis.Value = delta.y;
    }
}
