using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneTransitionFXPro
{
    public class SceneLoader : MonoBehaviour
    {
        public string fromScene;  // Set in the Inspector
        public string toScene;    // Set in the Inspector

        public void LoadScene()
        {
            // If current scene matches the "fromScene", load the "toScene"
            if (SceneManager.GetActiveScene().name == fromScene)
            {
                SceneManager.LoadScene(toScene);
            }
        }
    }
}
