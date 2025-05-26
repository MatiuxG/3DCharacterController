using UnityEngine;
using Unity.Cinemachine;

[ExecuteAlways]
[SaveDuringPlay]
[AddComponentMenu("")] // No mostrar en menú Add Component
public class LimitCameraHeightExtension : CinemachineExtension
{
    [Tooltip("Altura máxima permitida por encima del objetivo")]
    public float MaxHeightOffset = 2.0f;

    [Tooltip("Altura mínima permitida por encima del objetivo")]
    public float MinHeightOffset = 0.3f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body && vcam.Follow != null)
        {
            float targetY = vcam.Follow.position.y;
            float clampedY = Mathf.Clamp(state.RawPosition.y, targetY + MinHeightOffset, targetY + MaxHeightOffset);
            Vector3 corrected = state.RawPosition;
            corrected.y = clampedY;
            state.RawPosition = corrected;
        }
    }
}
