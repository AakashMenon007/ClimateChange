using UnityEngine;

namespace SceneTransitionFXPro
{
    public class PlayEffect : MonoBehaviour
    {
        public TransitionManager transitionManager;  // Assign in Inspector

        public void OnButtonClick()
        {
            if (transitionManager != null)
            {
                transitionManager.OnPlayButtonClick();
            }
            else
            {
                Debug.LogError("TransitionManager is not assigned in PlayEffect.");
            }
        }
    }
}
