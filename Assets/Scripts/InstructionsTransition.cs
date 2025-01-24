using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionButtonWithManager : MonoBehaviour
{
    // The scene name to load when the button is clicked (can be set in the Inspector)
    [SerializeField] private string sceneToLoad;

    // Reference to the Button
    [SerializeField] private Button transitionButton;

    // Reference to the TransitionManagerV2 to use the existing scene management logic
    [SerializeField] private TransitionManagerV2 transitionManager;

    private void Start()
    {
        // Ensure the button is assigned and set up the OnClick listener
        if (transitionButton != null)
        {
            transitionButton.onClick.AddListener(OnButtonClicked);
        }
        else
        {
            Debug.LogError("Transition Button not assigned!");
        }

        // Ensure the TransitionManager is assigned
        if (transitionManager == null)
        {
            Debug.LogError("TransitionManagerV2 is not assigned!");
        }
    }

    // This function is called when the button is clicked
    private void OnButtonClicked()
    {
        // Use TransitionManagerV2 to handle the scene transition
        if (transitionManager != null)
        {
            transitionManager.SavePositionAndLoadCityScene(); // Saving the player position before transitioning
        }

        LoadScene(sceneToLoad); // Load the new scene specified
    }

    // Load the scene based on the scene name
    private void LoadScene(string sceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' is not in the build settings! Please add it.");
        }
    }
}
