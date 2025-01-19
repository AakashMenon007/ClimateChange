using UnityEngine;

public class BuildingCollapse : MonoBehaviour
{
    public float collapseThreshold = 4.0f; // Minimum magnitude for the building to collapse
    public float collapseForce = 10.0f;    // Force applied to the building during collapse
    public Rigidbody[] buildingParts;      // Rigidbodies of building pieces
    public ParticleSystem collapseVFX;     // Particle system for collapse effect
    public bool isCollapsed = false;       // Tracks if the building has already collapsed

    private BoxCollider buildingCollider;  // BoxCollider of the main building

    private void Start()
    {
        // Find all rigidbodies attached to the building parts
        if (buildingParts == null || buildingParts.Length == 0)
        {
            buildingParts = GetComponentsInChildren<Rigidbody>();
        }

        // Get the BoxCollider attached to the building
        buildingCollider = GetComponent<BoxCollider>();

        if (buildingCollider == null)
        {
            Debug.LogWarning("No BoxCollider found on the building. Ensure the building has a BoxCollider.");
        }
    }

    public void OnEarthquakeTriggered(float magnitude)
    {
        if (!isCollapsed && magnitude >= collapseThreshold)
        {
            Debug.Log($"Earthquake triggered with magnitude {magnitude}. Threshold: {collapseThreshold}");
            CollapseBuilding(magnitude);
        }
        else
        {
            Debug.Log($"Earthquake magnitude {magnitude} is below the collapse threshold of {collapseThreshold}. No collapse triggered.");
        }
    }

    private void CollapseBuilding(float magnitude)
    {
        Debug.Log("Building collapse triggered!");

        // Turn off the BoxCollider to prevent further interactions
        if (buildingCollider != null)
        {
            buildingCollider.enabled = false;
            Debug.Log("Building BoxCollider disabled.");
        }

        // Trigger VFX effect for the collapse
        if (collapseVFX != null)
        {
            collapseVFX.Play();
            Debug.Log("Playing building collapse VFX.");
        }

        // Apply physics to each part of the building
        foreach (var part in buildingParts)
        {
            if (part != null)
            {
                // Turn off isKinematic to allow the part to be affected by physics
                part.isKinematic = false;

                // Apply a downward force with a slight horizontal component
                Vector3 randomDirection = new Vector3(
                    Random.Range(-1f, 1f), // Horizontal force (left-right)
                    -Random.Range(0.5f, 1f), // Downward force (negative for falling)
                    Random.Range(-1f, 1f)  // Horizontal force (front-back)
                );

                part.AddForce(randomDirection * magnitude * collapseForce, ForceMode.Impulse);
                Debug.Log($"Applying force to {part.gameObject.name}.");
            }
        }

        isCollapsed = true; // Mark the building as collapsed
        Debug.Log("Building has collapsed.");
    }
}
