using UnityEngine;
using System.Collections.Generic;

public class LowFrequencyEarthquake : MonoBehaviour
{
    public float magnitude = 2.0f;
    public float duration = 5.0f;
    public Transform playerCamera;
    public float shakeIntensity = 0.3f;

    private bool isEarthquakeActive = false;
    private float elapsedTime = 0f;

    // List of BuildingCollapse scripts for multiple buildings
    public List<BuildingCollapse> buildingCollapses;

    // Earthquake sound effect
    public AudioClip lowFrequencySound;
    private AudioSource audioSource;

    private void Start()
    {
        // Initialize AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = lowFrequencySound;
        audioSource.loop = true; // Loop the sound for the duration of the earthquake
    }

    void Update()
    {
        if (isEarthquakeActive)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < duration)
            {
                ApplyTremors();
                ShakeCamera();
            }
            else
            {
                StopEarthquake();
            }
        }
    }

    public void TriggerEarthquake()
    {
        Debug.Log($"Triggering Low Frequency Earthquake with magnitude: {magnitude}");
        isEarthquakeActive = true;
        elapsedTime = 0f;
        enabled = true; // Enable the script

        // Play sound effect
        if (audioSource != null && lowFrequencySound != null)
        {
            audioSource.Play();
        }

        // Notify all building collapse systems
        foreach (var buildingCollapse in buildingCollapses)
        {
            if (buildingCollapse != null)
            {
                buildingCollapse.OnEarthquakeTriggered(magnitude);
            }
        }
    }

    private void StopEarthquake()
    {
        isEarthquakeActive = false;
        elapsedTime = 0f;
        enabled = false; // Disable the script

        // Stop sound effect
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    private void ApplyTremors()
    {
        Rigidbody[] rigidbodies = FindObjectsOfType<Rigidbody>();
        foreach (Rigidbody rb in rigidbodies)
        {
            Vector3 randomForce = new Vector3(
                Random.Range(-1f, 1f) * magnitude,
                0,
                Random.Range(-1f, 1f) * magnitude
            );

            rb.AddForce(randomForce, ForceMode.Impulse);
            Debug.Log($"Applying tremor force to Rigidbody: {rb.gameObject.name} with magnitude: {magnitude}");
        }
    }

    private void ShakeCamera()
    {
        if (playerCamera != null)
        {
            Vector3 shakeOffset = new Vector3(
                Random.Range(-1f, 1f) * shakeIntensity,
                Random.Range(-1f, 1f) * shakeIntensity,
                Random.Range(-1f, 1f) * shakeIntensity
            );

            playerCamera.localPosition += shakeOffset * Time.deltaTime;
            Debug.Log("Shaking camera with intensity: " + shakeIntensity);
        }
    }

    public void EmergencyStop()
    {
        Debug.Log("Emergency Stop Triggered: Stopping Earthquake");
        isEarthquakeActive = false;
        elapsedTime = 0f;
        enabled = false; // Disable the script

        // Stop sound effect
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

}
