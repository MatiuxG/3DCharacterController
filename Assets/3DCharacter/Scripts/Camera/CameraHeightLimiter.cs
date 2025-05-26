using UnityEngine;

public class CameraHeightLimiter : MonoBehaviour
{
    [Header("Player reference")]
    [SerializeField] Transform target; // el "Follow Target" o "Camera Pivot"

    [Header("Limites")]
    [SerializeField] float maxHeightOffset = 2.0f;
    [SerializeField] float minHeightOffset = 0.2f;

    void LateUpdate()
    {
        if (!target) return;

        Vector3 pos = transform.position;
        float playerY = target.position.y;

        float maxHeight = playerY + maxHeightOffset;
        float minHeight = playerY + minHeightOffset;

        pos.y = Mathf.Clamp(pos.y, minHeight, maxHeight);
        transform.position = pos;
    }
}
