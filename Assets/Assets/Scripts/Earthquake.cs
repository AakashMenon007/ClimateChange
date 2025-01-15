using UnityEngine;
using System.Collections;
using Unitycoder.Demos;

public class EarthquakeSimulation : MonoBehaviour
{
    public GameObject ground;              // The ground or central object that will shake (can be a parent object)
    public GameObject xrRig;               // The XR Rig (camera and player controller)
    public Camera vrCamera;                // The camera in the XR Rig
    public float earthquakeDuration = 5f;  // Duration of the earthquake
    public float shakeIntensity = 1f;      // Intensity of the shake
    public float destructionDelay = 3f;    // Delay before destruction starts
    public float shakeInterval = 0.1f;     // Interval between shakes for a more natural feel
    public float cameraShakeDuration = 0.5f; // Duration of the camera shake
    public float cameraShakeIntensity = 0.2f; // Intensity of the camera shake

    private Vector3 originalGroundPosition;
    private Vector3 originalCameraPosition;

    void Start()
    {
        if (ground == null || xrRig == null || vrCamera == null)
        {
            Debug.LogError("Essential objects are not assigned. Please assign them in the inspector.");
            return;
        }

        originalGroundPosition = ground.transform.position;
        originalCameraPosition = vrCamera.transform.localPosition;

        StartCoroutine(StartEarthquake());
    }

    IEnumerator StartEarthquake()
    {
        float elapsed = 0f;
        float cameraShakeElapsed = 0f;

        // Shake the camera and environment during the earthquake
        while (elapsed < earthquakeDuration)
        {
            // Apply random shake force to the ground (environment)
            Vector3 shakeOffset = new Vector3(
                Random.Range(-shakeIntensity, shakeIntensity),
                0,  // No vertical shake for the ground
                Random.Range(-shakeIntensity, shakeIntensity)
            );

            // Apply shake to the ground
            ground.transform.position = originalGroundPosition + shakeOffset;

            // Apply force to all destructible objects
            Rigidbody[] allRigidbodies = FindObjectsOfType<Rigidbody>();
            foreach (Rigidbody rb in allRigidbodies)
            {
                if (rb.CompareTag("Destructible"))
                {
                    // Apply a random shake force to destructible objects
                    rb.AddForce(shakeOffset, ForceMode.Impulse);
                }
            }

            // Apply camera shake effect
            if (cameraShakeElapsed < cameraShakeDuration)
            {
                Vector3 cameraShake = new Vector3(
                    Random.Range(-cameraShakeIntensity, cameraShakeIntensity),
                    Random.Range(-cameraShakeIntensity, cameraShakeIntensity),
                    Random.Range(-cameraShakeIntensity, cameraShakeIntensity)
                );
                vrCamera.transform.localPosition = originalCameraPosition + cameraShake;
                cameraShakeElapsed += shakeInterval;
            }
            else
            {
                // Reset camera shake after the duration is over
                vrCamera.transform.localPosition = originalCameraPosition;
            }

            elapsed += shakeInterval;
            yield return new WaitForSeconds(shakeInterval);
        }

        // Reset the ground position after the shake
        ground.transform.position = originalGroundPosition;
        vrCamera.transform.localPosition = originalCameraPosition;

        // Wait for a delay before triggering destruction
        yield return new WaitForSeconds(destructionDelay);

        // Trigger destruction of buildings/objects
        TriggerDestruction();
    }

    void TriggerDestruction()
    {
        // Find all destructible objects in the scene and destroy them (or trigger mesh explosions)
        GameObject[] destructibleObjects = GameObject.FindGameObjectsWithTag("Destructible");

        foreach (GameObject obj in destructibleObjects)
        {
            // Example: If you have SimpleMeshExploder attached to objects, you can trigger the explosion here
            if (SimpleMeshExploder.instance != null)
            {
                SimpleMeshExploder.instance.Explode(obj.transform);
            }
            else
            {
                Debug.LogError("SimpleMeshExploder instance is not initialized.");
            }
        }
    }
}
