using UnityEngine;
using System.Collections.Generic;

public class ResetManager : MonoBehaviour
{
    // Struct to store initial state
    private struct ObjectState
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 velocity;
        public Vector3 angularVelocity;
    }

    // Dictionary to map GameObjects to their initial state
    private Dictionary<GameObject, ObjectState> initialStates = new Dictionary<GameObject, ObjectState>();

    // List of objects to track
    public List<GameObject> trackableObjects;

    private void Start()
    {
        // Save initial states of all trackable objects and their children
        foreach (GameObject obj in trackableObjects)
        {
            SaveStateRecursive(obj);
        }
    }

    // Recursive method to save the state of a GameObject and its children
    private void SaveStateRecursive(GameObject obj)
    {
        if (obj.TryGetComponent(out Rigidbody rb))
        {
            initialStates[obj] = new ObjectState
            {
                position = obj.transform.position,
                rotation = obj.transform.rotation,
                velocity = rb.velocity,
                angularVelocity = rb.angularVelocity
            };
        }
        else
        {
            initialStates[obj] = new ObjectState
            {
                position = obj.transform.position,
                rotation = obj.transform.rotation,
                velocity = Vector3.zero,
                angularVelocity = Vector3.zero
            };
        }

        // Recursively save states of all children
        foreach (Transform child in obj.transform)
        {
            SaveStateRecursive(child.gameObject);
        }
    }

    // Method to reset the environment
    public void ResetEnvironment()
    {
        foreach (var obj in initialStates.Keys)
        {
            if (obj != null)
            {
                // Reset position and rotation
                obj.transform.position = initialStates[obj].position;
                obj.transform.rotation = initialStates[obj].rotation;

                // Reset Rigidbody properties if it exists
                if (obj.TryGetComponent(out Rigidbody rb))
                {
                    rb.velocity = initialStates[obj].velocity;
                    rb.angularVelocity = initialStates[obj].angularVelocity;
                    rb.isKinematic = false; // Ensure Rigidbody is non-kinematic
                }

                // Handle custom reset behavior for components like BuildingCollapse
                if (obj.TryGetComponent(out BuildingCollapse buildingCollapse))
                {
                    buildingCollapse.ResetBuilding();
                }
            }
        }
    }
}
