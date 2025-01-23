using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class LabScientistTeleportSeismo : MonoBehaviour
{
    public Transform target;            // Target position for teleportation
    public GameObject teleportVFX;     // VFX prefab to play during teleportation
    public GameObject scientist;

    private NPC3D npc3D;

    public AudioSource teleportAudio;

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

        if (scientist != null)
        {
            npc3D = scientist.GetComponent<NPC3D>();
            if (npc3D == null)
            {
                Debug.LogError("NPC3D script is not attached to the scientist!");
            }
        }
        else
        {
            Debug.LogError("Scientist GameObject is not assigned!");
        }
    }

    [YarnCommand("teleportSeismo")]
    public void Teleport()
    {
        Debug.Log("Calling from Yarn script");
        StartCoroutine(TeleportSequence());
    }

    private IEnumerator TeleportSequence()
    {

        // Step 1: Play VFX at the current position
        PlayVFX(scientist.transform.position);
        teleportAudio.Play();

        // Step 2: Wait for the duration of the VFX before teleporting
        yield return new WaitForSeconds(GetVFXDuration());

        // Step 3: Teleport the Lab Scientist to the target position
        scientist.transform.position = target.position;
        scientist.transform.rotation = target.rotation;

        // Step 4: Play VFX at the target position
        PlayVFX(scientist.transform.position);
        teleportAudio.Play();

        //Step 5: Update the talkToNode in NPC3D AFTER teleportation
        if (npc3D != null)
        {
            npc3D.talkToNode = "Seismograph";
            npc3D.ResetNodeUsage(); // Allow re-triggering the node
            Debug.Log("Updated talkToNode to: Seismograph");
        }

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