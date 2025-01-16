using UnityEngine;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    [Header("Audio Settings")]
    public VoiceOverView voiceOverView; // Reference to the VoiceOverView component
    public AudioSource barmanSource;    // Reference to the AudioSource

    [YarnCommand("audioSetNPC1")]
    public void AudioSetNPC1()
    {
        // Ensure the VoiceOverView and VoiceSource are assigned
        if (voiceOverView == null)
        {
            Debug.LogError("VoiceOverView is not assigned in YarnCommands.");
            return;
        }

        if (barmanSource == null)
        {
            Debug.LogError("VoiceSource is not assigned in YarnCommands.");
            return;
        }

        // Set the audio source in VoiceOverView        voiceOverView.audioSource = barmanSource;
        Debug.Log("VoiceOverView audio source set to " + barmanSource.name);
    }
}
