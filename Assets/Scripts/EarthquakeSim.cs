using UnityEngine;
using UnityEngine.XR;

public class EarthquakeSimulator : MonoBehaviour
{
    public float magnitude = 5.0f; // Default magnitude
    public float duration = 10.0f; // Duration of the earthquake
    public Transform playerCamera; // Assign the player's camera for camera shake
    public float shakeIntensity = 0.5f; // Camera shake intensity
    public bool isEarthquakeActive = false;

    private float elapsedTime = 0f;

    private XRNode inputSource = XRNode.Head; // Reference to the head transform for XR

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
                isEarthquakeActive = false; // End earthquake
                elapsedTime = 0f;
            }
        }
    }

    public void StartEarthquake()
    {
        elapsedTime = 0f;
        isEarthquakeActive = true;
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
        }
    }

    private void ShakeCamera()
    {
        if (playerCamera != null)
        {
            // Applying shake to XR Head Transform instead of the camera directly
            Vector3 shakeOffset = new Vector3(
                Random.Range(-1f, 1f) * shakeIntensity,
                Random.Range(-1f, 1f) * shakeIntensity,
                Random.Range(-1f, 1f) * shakeIntensity
            );

            // Get the head's position using InputTracking
            Vector3 headPosition = InputTracking.GetLocalPosition(inputSource);

            // Apply the shake to the head's position
            playerCamera.localPosition = headPosition + shakeOffset * Time.deltaTime;
        }
    }
}
