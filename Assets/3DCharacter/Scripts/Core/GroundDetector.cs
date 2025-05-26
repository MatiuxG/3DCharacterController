using UnityEngine;

public class GroundDetector : MonoBehaviour
{
    [Header("Ground Settings")]
    [SerializeField] LayerMask groundLayers;
    [Tooltip("Tags que serán ignorados incluso si están en la capa de suelo")]
    [SerializeField] string[] ignoredTags;

    private PlayerLocomotion playerLocomotion;

    void Start()
    {
        playerLocomotion = GetComponentInParent<PlayerLocomotion>();
        if (playerLocomotion == null)
        {
            Debug.LogError("[GroundDetector] No se encontró PlayerLocomotion en el padre.");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (IsLayerGround(other.gameObject.layer) && !IsIgnored(other.gameObject))
        {
            Debug.Log($"[GroundDetector] ENTER → {other.name}");
            playerLocomotion.RegisterGroundContact(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (IsLayerGround(other.gameObject.layer) && !IsIgnored(other.gameObject))
        {
            Debug.Log($"[GroundDetector] EXIT → {other.name}");
            playerLocomotion.UnregisterGroundContact(other);
        }
    }
    private bool IsIgnored(GameObject obj)
    {
        foreach (string tag in ignoredTags)
        {
            if (obj.CompareTag(tag))
            {
                Debug.Log($"[GroundDetector] Ignorado por tag: {obj.name} (Tag: {tag})");
                return true;
            }
        }
        return false;
    }

    private bool IsLayerGround(int layer)
    {
        return ((1 << layer) & groundLayers) != 0;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.05f);
    }
        
}
