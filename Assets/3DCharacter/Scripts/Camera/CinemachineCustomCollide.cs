using UnityEngine;
using Unity.Cinemachine;

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("Cinemachine/Extensions/Custom Camera Collision")]
public class CinemachineCustomCollide : CinemachineExtension
{
    [Header("Collision Settings")]
    public LayerMask collisionLayers = ~0;
    public float cameraRadius = 0.3f;
    public float defaultDistance = 4f;
    public float minDistance = 0.5f;
    public float smoothSpeed = 15f;

    private float currentDistance;
    private Vector3 currentPosition;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage != CinemachineCore.Stage.Body || vcam.Follow == null)
            return;

        Transform follow = vcam.Follow;

        Vector3 pivot = follow.position;
        Vector3 camDir = (state.RawPosition - pivot).normalized;

        float targetDistance = defaultDistance;

        if (Physics.SphereCast(pivot, cameraRadius, camDir, out RaycastHit hit, defaultDistance, collisionLayers))
        {
            targetDistance = Mathf.Clamp(hit.distance, minDistance, defaultDistance);
        }

        currentDistance = Mathf.Lerp(currentDistance == 0 ? defaultDistance : currentDistance, targetDistance, deltaTime * smoothSpeed);

        Vector3 finalPos = pivot + camDir * currentDistance;
        state.RawPosition = finalPos;
    }
}
