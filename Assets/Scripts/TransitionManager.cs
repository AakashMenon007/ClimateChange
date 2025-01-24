using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    private static Vector3 savedPlayerPosition; // To save the player's position

    // Save the player's position and transition to another scene
    public void SavePositionAndLoadCityScene(GameObject player)
    {
        savedPlayerPosition = player.transform.position; // Save player's position
        string sceneName = "CityScene";

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' is not in the build settings!");
        }
    }

    // Load "Lab Master 2" and restore the player's position
    public void LoadLabMasterScene(GameObject player)
    {
        string sceneName = "Lab Master 2";

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.sceneLoaded += (scene, mode) =>
            {
                if (scene.name == sceneName)
                {
                    player.transform.position = savedPlayerPosition; // Restore position
                    SceneManager.sceneLoaded -= null; // Remove event listener after restoring
                }
            };
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' is not in the build settings!");
        }
    }

    // GUI for buttons to trigger scene transitions
    private void OnGUI()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Find player by tag

        if (player == null)
        {
            GUILayout.Label("Player not found! Ensure the player GameObject has the tag 'Player'.");
            return;
        }

        if (GUILayout.Button("Save Position & Transition to CityScene"))
        {
            SavePositionAndLoadCityScene(player);
        }

        if (GUILayout.Button("Transition back to Lab Master 2 at Previous Position"))
        {
            LoadLabMasterScene(player);
        }
    }
}
