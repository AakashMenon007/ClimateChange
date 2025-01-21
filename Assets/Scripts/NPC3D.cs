using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPC3D : MonoBehaviour
{
    [Header("Character")]
    public string characterName = "";

    [Header("Yarn Specific")]
    public string talkToNode = "";
    public YarnProject scriptToLoad;
    public DialogueRunner dialogueRunner; // Reference to the dialogue control
    public GameObject dialogueCanvas;    // Reference to the canvas

    [Header("Dialogue Canvas")]
    public Vector3 PostionSpeachBubble = new Vector3(0f, 2.0f, 0.0f);
    private float canvasTurnSpeed = 2;
    private bool canvasActive;
    private GameObject playerGameObject;

    private bool nodeUsed = false; // Flag for tracking if the node has been used

    void Start()
    {
        dialogueCanvas = GameObject.FindGameObjectWithTag("Dialogue Canvas");
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        playerGameObject = GameObject.FindGameObjectWithTag("Player");

        if (scriptToLoad != null && dialogueRunner != null)
        {
            dialogueRunner.yarnProject = scriptToLoad; // Adds the script to the dialogue system
        }
    }

    void Update()
    {
        if (canvasActive)
        {
            Vector3 lookDir = dialogueCanvas.transform.position - playerGameObject.transform.position;
            float radians = Mathf.Atan2(lookDir.x, lookDir.z);
            float degrees = radians * Mathf.Rad2Deg;

            float str = Mathf.Min(canvasTurnSpeed * Time.deltaTime, 1);
            Quaternion targetRotation = Quaternion.Euler(0, degrees, 0);
            dialogueCanvas.transform.rotation = Quaternion.Slerp(dialogueCanvas.transform.rotation, targetRotation, str);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters and if the node is allowed to run
        if (other.gameObject.CompareTag("Player") && !nodeUsed)
        {
            if (!string.IsNullOrEmpty(talkToNode))
            {
                if (dialogueCanvas != null)
                {
                    // Move the Canvas to the object and offset
                    canvasActive = true;
                    dialogueCanvas.transform.SetParent(transform); // Use the root to prevent scaling
                    dialogueCanvas.GetComponent<RectTransform>().anchoredPosition3D = transform.TransformVector(PostionSpeachBubble);
                }

                if (dialogueRunner.IsDialogueRunning)
                {
                    dialogueRunner.Stop();
                }

                Debug.Log($"Starting dialogue for node: {talkToNode}");
                dialogueRunner.StartDialogue(talkToNode);

                // Mark the node as used after starting the dialogue
                nodeUsed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canvasActive = false;
            dialogueRunner.Stop();
        }
    }

    /// <summary>
    /// Resets the node usage to allow the dialogue to trigger again.
    /// </summary>
    public void ResetNodeUsage()
    {
        nodeUsed = false;
        Debug.Log("Node usage reset, dialogue can be triggered again.");
    }
}
