using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupSceneManager : MonoBehaviour
{
    // Name of the scene to load when the button is clicked
    [SerializeField] private string targetSceneName = "Lab Master 2";

    // Method to load the target scene
    public void StartGame()
    {
        if (Application.CanStreamedLevelBeLoaded(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError($"Scene '{targetSceneName}' is not in the build settings! Please add it to the build settings.");
        }
    }
}
