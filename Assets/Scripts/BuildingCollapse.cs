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
    private Vector3[] initialPositions;
    private Quaternion[] initialRotations;

    private void Start()
    {
        // Find building parts
        if (buildingParts == null || buildingParts.Length == 0)
        {
            buildingParts = GetComponentsInChildren<Rigidbody>();
        }

        buildingCollider = GetComponent<BoxCollider>();

        // Save initial positions and rotations of building parts
        initialPositions = new Vector3[buildingParts.Length];
        initialRotations = new Quaternion[buildingParts.Length];
        for (int i = 0; i < buildingParts.Length; i++)
        {
            initialPositions[i] = buildingParts[i].transform.localPosition;
            initialRotations[i] = buildingParts[i].transform.localRotation;
        }
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

    // Reset building to its original state
    public void ResetBuilding()
    {
        isCollapsed = false;

        if (buildingCollider != null)
        {
            buildingCollider.enabled = true;
        }

        for (int i = 0; i < buildingParts.Length; i++)
        {
            if (buildingParts[i] != null)
            {
                // Reset position and rotation
                buildingParts[i].transform.localPosition = initialPositions[i];
                buildingParts[i].transform.localRotation = initialRotations[i];

                // Make parts kinematic again
                buildingParts[i].isKinematic = true;
                buildingParts[i].velocity = Vector3.zero;
                buildingParts[i].angularVelocity = Vector3.zero;
            }
        }
    }
}
