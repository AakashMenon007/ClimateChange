using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;
using Yarn.Unity;

public class LabScientistTeleportVolcano : MonoBehaviour
{
    public Transform target;            // Target position for teleportation
    public GameObject teleportVFX;     // VFX prefab to play during teleportation
    public GameObject scientist;

    private NPC3D npc3D;

    public VideoPlayer tsunamiVideo;
    public GameObject screen;
    public ParticleSystem lava1, lava2, lava3, smoke;
    public AudioSource volcanoAudio;
    public GameObject volcano;

    private void Start()
    {
        screen.SetActive(false);

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

    [YarnCommand("teleportVolcano")]
    public void Teleport()
    {
        Debug.Log("Calling from Yarn script");
        StartCoroutine(TeleportSequence());
    }

    private IEnumerator TeleportSequence()
    {

        // Step 1: Play VFX at the current position
        PlayVFX(scientist.transform.position);

        // Step 2: Wait for the duration of the VFX before teleporting
        yield return new WaitForSeconds(GetVFXDuration());

        // Step 3: Teleport the Lab Scientist to the target position
        scientist.transform.position = target.position;
        scientist.transform.rotation = target.rotation;

        // Step 4: Play VFX at the target position
        PlayVFX(scientist.transform.position);

        //Step 5: Update the talkToNode in NPC3D AFTER teleportation
        if (npc3D != null)
        {
            npc3D.talkToNode = "Volcano";
            npc3D.ResetNodeUsage(); // Allow re-triggering the node
            Debug.Log("Updated talkToNode to: Volcano");
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

    [YarnCommand("volcano")]
    public void Volcano()
    {
        lava1.Play();
        lava2.Play();
        lava3.Play();
        smoke.Play();
        volcano.GetComponent<Animator>().enabled = true;
        volcanoAudio.Play();
    }

    [YarnCommand("tsunami")]
    public void Tsunami()
    {
        screen.SetActive(true);

        tsunamiVideo.Play();

        //if (!tsunamiVideo.isPlaying)
        //{
        //    screen.SetActive(false);
        //}

    }
}
