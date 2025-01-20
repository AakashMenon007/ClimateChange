using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabScientistTeleport : MonoBehaviour
{
    public Transform target;            // Target position for teleportation
    public GameObject teleportVFX;     // VFX prefab to play during teleportation

    private void Start()
    {
        if (target == null)
        {
            Debug.LogError("Target Transform is not assigned!");
        }

        if (teleportVFX == null)
        {
            Debug.LogError("Teleport VFX prefab is not assigned!");
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) // Press 'T' to teleport
        {
            GetComponent<LabScientistTeleport>().Teleport();
        }
    }

    public void Teleport()
    {
        StartCoroutine(TeleportSequence());
    }

    private IEnumerator TeleportSequence()
    {
        // Step 1: Play VFX at the current position
        PlayVFX(transform.position);

        // Step 2: Wait for the duration of the VFX before teleporting
        yield return new WaitForSeconds(GetVFXDuration());

        // Step 3: Teleport the Lab Scientist to the target position
        transform.position = target.position;

        // Step 4: Play VFX at the target position
        PlayVFX(transform.position);
    }

    private void PlayVFX(Vector3 position)
    {
        // Instantiate the VFX at the specified position
        if (teleportVFX != null)
        {
            GameObject vfxInstance = Instantiate(teleportVFX, position, Quaternion.identity);

            // Automatically destroy the VFX after its duration
            Destroy(vfxInstance, GetVFXDuration());
        }
    }

    private float GetVFXDuration()
    {
        // Get the duration of the Particle System
        ParticleSystem particleSystem = teleportVFX.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            return particleSystem.main.duration;
        }

        Debug.LogWarning("Teleport VFX does not have a ParticleSystem! Returning default duration of 1 second.");
        return 1f; // Default duration if no particle system is found
    }
}


