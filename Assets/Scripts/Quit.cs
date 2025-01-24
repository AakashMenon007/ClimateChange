using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    // Reference to the button
    [SerializeField] private Button quitButton;

    private void Start()
    {
        // Ensure the button is assigned and set up the OnClick listener
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitButtonClicked);
        }
        else
        {
            Debug.LogError("Quit Button not assigned!");
        }
    }

    // This function is called when the quit button is clicked
    public void OnQuitButtonClicked()
    {
        QuitGame();
    }

    // Function to quit the game
    private void QuitGame()
    {
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // For standalone builds
        Application.Quit();
#endif

        Debug.Log("Game Exited");
    }
}
