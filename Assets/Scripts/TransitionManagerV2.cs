using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManagerV2 : MonoBehaviour
{
    private static Vector3 savedPlayerPosition; // To save the player's position
    private static Quaternion savedPlayerRotation; // To save the player's rotation

    [SerializeField] private string citySceneName = "CityScene";
    [SerializeField] private string labMasterSceneName = "Lab Master 2";

    // Reference to the player object
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Find player by tag
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player GameObject has the tag 'Player'.");
        }
    }

    // Save the player's position and transition to the City scene
    public void SavePositionAndLoadCityScene()
    {
        if (player == null)
        {
            Debug.LogError("Player not found! Transition canceled.");
            return;
        }

        savedPlayerPosition = player.transform.position; // Save position
        savedPlayerRotation = player.transform.rotation; // Save rotation

        if (Application.CanStreamedLevelBeLoaded(citySceneName))
        {
            SceneManager.LoadScene(citySceneName);
        }
        else
        {
            Debug.LogError($"Scene '{citySceneName}' is not in the build settings! Please add it.");
        }
    }

    // Load Lab Master scene and restore the player's position
    public void LoadLabMasterScene()
    {
        if (Application.CanStreamedLevelBeLoaded(labMasterSceneName))
        {
            SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to the scene loaded event
            SceneManager.LoadScene(labMasterSceneName);
        }
        else
        {
            Debug.LogError($"Scene '{labMasterSceneName}' is not in the build settings! Please add it.");
        }
    }

    // Callback for when the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to prevent repeated calls

        if (scene.name == labMasterSceneName && player != null)
        {
            player = GameObject.FindGameObjectWithTag("Player"); // Reassign player if needed
            if (player != null)
            {
                player.transform.position = savedPlayerPosition; // Restore position
                player.transform.rotation = savedPlayerRotation; // Restore rotation
            }
        }
    }
}
