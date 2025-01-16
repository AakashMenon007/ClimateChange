using UnityEngine;
using Exploder;
using System.Collections;
using Exploder.Utils;

public class EarthquakeSimulator : MonoBehaviour
{
    public float earthquakeStartDelay = 5f; // Delay before the earthquake starts
    public float destructionInterval = 2f; // Time interval between each object's destruction
    public float earthquakeDuration = 15f; // Total duration of the earthquake (optional for stopping)

    private GameObject[] explodableObjects; // Array to hold all tagged objects
    private int currentIndex = 0; // Tracks the current object being destroyed
    private bool earthquakeActive = false;
    private float earthquakeTimer = 0f;

    private void Start()
    {
        // Delay the start of the earthquake
        Invoke(nameof(StartEarthquake), earthquakeStartDelay);
    }

    private void Update()
    {
        if (earthquakeActive)
        {
            earthquakeTimer += Time.deltaTime;

            // Stop the earthquake after the duration ends
            if (earthquakeTimer >= earthquakeDuration)
            {
                earthquakeActive = false;
                CancelInvoke(nameof(DestroyNextObject));
            }
        }
    }

    private void StartEarthquake()
    {
        // Find all objects tagged as "Exploder"
        explodableObjects = GameObject.FindGameObjectsWithTag(ExploderObject.Tag);

        if (explodableObjects.Length > 0)
        {
            earthquakeActive = true;
            earthquakeTimer = 0f;
            currentIndex = 0;

            // Start sequentially destroying objects
            InvokeRepeating(nameof(DestroyNextObject), 0f, destructionInterval);
        }
        else
        {
            Debug.LogWarning("No objects found with tag 'Exploder'.");
        }
    }

    private void DestroyNextObject()
    {
        if (currentIndex < explodableObjects.Length)
        {
            // Get the current object to destroy
            GameObject target = explodableObjects[currentIndex];

            if (target != null)
            {
                // Use Exploder to destroy the object
                ExploderObject exploder = ExploderSingleton.Instance;
                if (exploder != null)
                {
                    exploder.ExplodeObject(target);

                    // Optional: Add particle effects, sound, or camera shake here
                }
            }

            currentIndex++; // Move to the next object
        }
        else
        {
            // Stop destruction after all objects are processed
            CancelInvoke(nameof(DestroyNextObject));
            Debug.Log("All objects destroyed.");
        }
    }
}
