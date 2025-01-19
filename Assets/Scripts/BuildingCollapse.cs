using System.Collections;
using UnityEngine;

public class BuildingCollapse : MonoBehaviour
{
    public float collapseThreshold = 4.0f;
    public float collapseForce = 10.0f;
    public Rigidbody[] buildingParts;
    [SerializeField] private GameObject collapseVFXPrefab; // Explosion particle system prefab
    public bool isCollapsed = false;

    private BoxCollider buildingCollider;

    private void Start()
    {
        // Find building parts
        if (buildingParts == null || buildingParts.Length == 0)
        {
            buildingParts = GetComponentsInChildren<Rigidbody>();
        }

        buildingCollider = GetComponent<BoxCollider>();
    }

    public void OnEarthquakeTriggered(float magnitude)
    {
        if (!isCollapsed && magnitude >= collapseThreshold)
        {
            StartCoroutine(CollapseBuildingGradually(magnitude));
        }
    }

    private IEnumerator CollapseBuildingGradually(float magnitude)
    {
        if (buildingCollider != null)
        {
            buildingCollider.enabled = false;
        }

        // Trigger explosion effect
        TriggerCollapseEffect();

        // Apply collapse force to building parts
        foreach (var part in buildingParts)
        {
            if (part != null)
            {
                part.isKinematic = false;
                part.AddForce(Vector3.down * magnitude * collapseForce, ForceMode.Impulse);
                yield return new WaitForSeconds(0.2f);
            }
        }

        isCollapsed = true;
    }

    private void TriggerCollapseEffect()
    {
        // Instantiate the collapse VFX prefab
        if (collapseVFXPrefab != null)
        {
            GameObject vfxInstance = Instantiate(collapseVFXPrefab, transform.position, Quaternion.identity);

            // Ensure the VFX is destroyed after it finishes playing
            ParticleSystem ps = vfxInstance.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                // Destroy the VFX object after the particle system is done
                Destroy(vfxInstance, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Debug.LogWarning("collapseVFXPrefab does not contain a ParticleSystem component.");
            }
        }
        else
        {
            Debug.LogWarning("collapseVFXPrefab is not assigned in the Inspector.");
        }
    }
}
