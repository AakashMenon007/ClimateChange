using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

namespace SceneTransitionFXPro
{
    public class TransitionManager : MonoBehaviour
    {
        public enum TransitionEffect
        {
            Slide, Fade, Zoom, Wipe, SpinWithFade, CardFlip, Cube, Pop, SmoothFlow, Glitch
        }

        public TransitionEffect selectedEffect = TransitionEffect.Slide;

        public float transitionSpeed = 1f;
        public float spinDuration = 1f;

        public enum CubeDirection { Left, Right }
        public CubeDirection cubeDirection = CubeDirection.Left;

        public enum SlideDirection { Left, Right, Up, Down }
        public SlideDirection slideDirection = SlideDirection.Left;

        public enum WipeDirection { Left, Right, Up, Down }
        public WipeDirection wipeDirection = WipeDirection.Left;

        public enum PopType { PopIn, PopOut }
        public PopType popType = PopType.PopIn;

        public enum FlowDirection { Left, Right }
        public FlowDirection flowDirection = FlowDirection.Left;

        public float fadeInAfter = 0f;
        public float zoomInAfter = 0f;

        public float glitchDuration = 0.5f;  // New for glitch transition

        public float zRotationAmount = 5f;
        public float reverseZRotationAmount = 5f;

        [SerializeField] private RectTransform currentScenePanel;  // Current scene's UI panel
        [SerializeField] private RectTransform otherScenePanel;    // Placeholder panel for the "other" scene
        [SerializeField] private SceneLoader sceneLoader;          // SceneLoader to handle loading of the next scene
        [SerializeField] private Camera mainCamera;                // Main Camera to control perspective

        public void OnPlayButtonClick()
        {
            switch (selectedEffect)
            {
                case TransitionEffect.Slide:
                    StartCoroutine(PerformSlideTransition());
                    break;

                case TransitionEffect.Fade:
                    StartCoroutine(PerformFadeTransition());
                    break;

                case TransitionEffect.Zoom:
                    StartCoroutine(PerformZoomTransition());
                    break;

                case TransitionEffect.Wipe:
                    StartCoroutine(PerformWipeTransition());
                    break;

                case TransitionEffect.SpinWithFade:
                    StartCoroutine(PerformSpinWithFadeTransition());
                    break;

                case TransitionEffect.CardFlip:
                    StartCoroutine(PerformCardFlipTransition());
                    break;

                case TransitionEffect.Cube:
                    StartCoroutine(PerformCubeTransition());
                    break;

                case TransitionEffect.Pop:
                    StartCoroutine(PerformPopTransition());
                    break;

                case TransitionEffect.SmoothFlow:
                    StartCoroutine(PerformSmoothFlowTransition());
                    break;

                case TransitionEffect.Glitch:
                    StartCoroutine(PerformGlitchTransition());
                    break;
            }
        }

        // - Slide transition coroutine - //
        private IEnumerator PerformSlideTransition()
        {
            otherScenePanel.anchoredPosition = Vector2.zero;

            Vector2 currentSceneStartPos = currentScenePanel.anchoredPosition;
            Vector2 currentSceneEndPos = Vector2.zero;
            Vector2 otherSceneStartPos = Vector2.zero;
            Vector2 otherSceneEndPos = Vector2.zero;

            // Setup slide positions based on the selected direction, including diagonals
            switch (slideDirection)
            {
                case SlideDirection.Left:
                    currentSceneEndPos = new Vector2(-Screen.width, 0);
                    otherSceneStartPos = new Vector2(Screen.width, 0);
                    break;
                case SlideDirection.Right:
                    currentSceneEndPos = new Vector2(Screen.width, 0);
                    otherSceneStartPos = new Vector2(-Screen.width, 0);
                    break;
                case SlideDirection.Up:
                    currentSceneEndPos = new Vector2(0, Screen.height);
                    otherSceneStartPos = new Vector2(0, -Screen.height);
                    break;
                case SlideDirection.Down:
                    currentSceneEndPos = new Vector2(0, -Screen.height);
                    otherSceneStartPos = new Vector2(0, Screen.height);
                    break;
            }

            otherScenePanel.anchoredPosition = otherSceneStartPos;
            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;
                currentScenePanel.anchoredPosition = Vector2.Lerp(currentSceneStartPos, currentSceneEndPos, elapsedTime);
                otherScenePanel.anchoredPosition = Vector2.Lerp(otherSceneStartPos, Vector2.zero, elapsedTime);
                yield return null;
            }

            // After the transition, load the "other" scene
            sceneLoader.LoadScene();
        }

        // - Fade transition coroutine - //
        private IEnumerator PerformFadeTransition()
        {
            otherScenePanel.anchoredPosition = Vector2.zero;

            CanvasGroup canvasGroupA = currentScenePanel.GetComponent<CanvasGroup>() ?? currentScenePanel.gameObject.AddComponent<CanvasGroup>();
            CanvasGroup canvasGroupB = otherScenePanel.GetComponent<CanvasGroup>() ?? otherScenePanel.gameObject.AddComponent<CanvasGroup>();

            canvasGroupB.alpha = 0;

            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;
                canvasGroupA.alpha = Mathf.Lerp(1, 0, elapsedTime);
                yield return null;
            }

            yield return new WaitForSeconds(fadeInAfter);

            elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;
                canvasGroupB.alpha = Mathf.Lerp(0, 1, elapsedTime);
                yield return null;
            }

            sceneLoader.LoadScene();
        }

        // - Zoom transition coroutine - //
        private IEnumerator PerformZoomTransition()
        {
            otherScenePanel.anchoredPosition = Vector2.zero;
            otherScenePanel.localScale = Vector3.zero;

            Vector3 originalScaleA = currentScenePanel.localScale;
            Vector3 targetScaleA = Vector3.zero;
            Vector3 originalScaleB = Vector3.zero;
            Vector3 targetScaleB = Vector3.one;

            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;
                currentScenePanel.localScale = Vector3.Lerp(originalScaleA, targetScaleA, elapsedTime);
                yield return null;
            }

            yield return new WaitForSeconds(zoomInAfter);

            elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;
                otherScenePanel.localScale = Vector3.Lerp(originalScaleB, targetScaleB, elapsedTime);
                yield return null;
            }

            sceneLoader.LoadScene();
        }

        // - Wipe transition coroutine - //
        private IEnumerator PerformWipeTransition()
        {
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            Vector2 startPos = Vector2.zero;
            Vector2 endPos = Vector2.zero;

            switch (wipeDirection)
            {
                case WipeDirection.Left:
                    startPos = new Vector2(screenWidth, 0);
                    endPos = Vector2.zero;
                    break;
                case WipeDirection.Right:
                    startPos = new Vector2(-screenWidth, 0);
                    endPos = Vector2.zero;
                    break;
                case WipeDirection.Up:
                    startPos = new Vector2(0, -screenHeight);
                    endPos = Vector2.zero;
                    break;
                case WipeDirection.Down:
                    startPos = new Vector2(0, screenHeight);
                    endPos = Vector2.zero;
                    break;
            }

            otherScenePanel.anchoredPosition = startPos;
            float elapsedTime = 0f;

            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;
                otherScenePanel.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsedTime);
                yield return null;
            }

            sceneLoader.LoadScene();
        }

        // - Spin with Fade transition coroutine - //
        private IEnumerator PerformSpinWithFadeTransition()
        {
            // Reset "otherScenePanel" position and rotation
            otherScenePanel.anchoredPosition = Vector2.zero;
            otherScenePanel.localRotation = Quaternion.Euler(0, 0, 0);
            otherScenePanel.localScale = Vector3.one;

            // Add CanvasGroup components for fading
            CanvasGroup canvasGroupA = currentScenePanel.GetComponent<CanvasGroup>() ?? currentScenePanel.gameObject.AddComponent<CanvasGroup>();
            CanvasGroup canvasGroupB = otherScenePanel.GetComponent<CanvasGroup>() ?? otherScenePanel.gameObject.AddComponent<CanvasGroup>();

            canvasGroupB.alpha = 0;  // Ensure next scene is fully transparent initially

            float elapsedTime = 0f;
            float initialRotation = 0f;
            float finalRotation = 360f; // One full spin (360 degrees)

            // Spin out and fade out current scene based on spin duration
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * (1f / spinDuration);  // Controls the spin to last for "spinDuration" seconds

                // Spin and fade out current scene
                currentScenePanel.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(initialRotation, finalRotation, elapsedTime));
                canvasGroupA.alpha = Mathf.Lerp(1, 0, elapsedTime);

                yield return null;
            }

            yield return new WaitForSeconds(fadeInAfter);

            elapsedTime = 0f;

            // Fade in the next scene (using the same transition speed)
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;  // Controls fade in speed (not spin duration)

                // Fade in next scene
                canvasGroupB.alpha = Mathf.Lerp(0, 1, elapsedTime);

                yield return null;
            }

            // After the transition, load the next scene
            sceneLoader.LoadScene();
        }

        // - CardFlip transition coroutine - //
        private IEnumerator PerformCardFlipTransition()
        {
            // Ensure the next scene starts in a hidden state (scaled down)
            otherScenePanel.localScale = Vector3.zero;
            otherScenePanel.anchoredPosition = Vector2.zero;  // Make sure it's centered

            float elapsedTime = 0f;
            float initialRotation = 0f;
            float finalRotation = 90f;  // Half rotation (flip out)

            // Flip out the current scene (rotate 90 degrees on Y-axis)
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;
                float rotation = Mathf.Lerp(initialRotation, finalRotation, elapsedTime);
                currentScenePanel.localRotation = Quaternion.Euler(0, rotation, 0);  // Rotate around Y-axis

                // Once fully flipped out, hide the current scene
                if (rotation >= 90f)
                {
                    currentScenePanel.localScale = Vector3.zero;  // Hide the current scene
                    break;  // Move on to the next step
                }

                yield return null;
            }

            // Reset elapsed time for the next scene flip in
            elapsedTime = 0f;
            initialRotation = 90f;  // Start at 90 degrees for the next scene to flip in
            finalRotation = 0f;  // Rotate back to 0 degrees (normal view)

            // Flip in the next scene (rotate back to 0 degrees)
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;
                float rotation = Mathf.Lerp(initialRotation, finalRotation, elapsedTime);
                otherScenePanel.localRotation = Quaternion.Euler(0, rotation, 0);  // Rotate next scene around Y-axis
                otherScenePanel.localScale = Vector3.one;  // Ensure the next scene is visible during the flip

                yield return null;
            }

            // After the transition, load the next scene
            sceneLoader.LoadScene();
        }

        // - 3D Cube transition coroutine - //
        private IEnumerator PerformCubeTransition()
        {
            // Set camera to perspective mode for 3D effect
            mainCamera.orthographic = false;
            mainCamera.fieldOfView = 60f;

            // Set initial positions and rotations based on the direction
            Vector3 currentSceneStartRotation = Vector3.zero;
            Vector3 currentSceneEndRotation;
            Vector3 nextSceneStartRotation;
            Vector3 nextSceneEndRotation = Vector3.zero;
            float scenePanelOffset;

            // Adjust positions and rotations based on CubeDirection
            if (cubeDirection == CubeDirection.Left)
            {
                currentSceneEndRotation = new Vector3(0, 90, 0); // Rotate 90 degrees to the left
                nextSceneStartRotation = new Vector3(0, -90, 0); // Start next scene rotated to -90 degrees
                scenePanelOffset = Screen.width * 0.75f; // Move next scene to the right (positive X)
            }
            else
            {
                currentSceneEndRotation = new Vector3(0, -90, 0); // Rotate 90 degrees to the right
                nextSceneStartRotation = new Vector3(0, 90, 0); // Start next scene rotated to 90 degrees
                scenePanelOffset = -Screen.width * 0.75f; // Move next scene to the left (negative X)
            }

            // Position the current and next scenes accordingly
            otherScenePanel.anchoredPosition = new Vector2(scenePanelOffset, 0);
            currentScenePanel.anchoredPosition = Vector2.zero;

            // Rotate the next scene into the correct start position
            otherScenePanel.localRotation = Quaternion.Euler(nextSceneStartRotation);

            float elapsedTime = 0f;
            while (elapsedTime < 1f)
            {
                elapsedTime += Time.deltaTime * transitionSpeed;

                // Rotate the current scene out
                currentScenePanel.localRotation = Quaternion.Euler(Vector3.Lerp(currentSceneStartRotation, currentSceneEndRotation, elapsedTime));

                // Rotate the next scene in
                otherScenePanel.localRotation = Quaternion.Euler(Vector3.Lerp(nextSceneStartRotation, nextSceneEndRotation, elapsedTime));

                // Move both panels during the transition
                currentScenePanel.anchoredPosition = new Vector2(Mathf.Lerp(0, -scenePanelOffset, elapsedTime), 0);
                otherScenePanel.anchoredPosition = new Vector2(Mathf.Lerp(scenePanelOffset, 0, elapsedTime), 0);

                yield return null;
            }

            // Load the next scene after the transition
            sceneLoader.LoadScene();

            // Reset camera back to orthographic mode if needed
            mainCamera.orthographic = true;
        }

        // - Pop transition coroutine - //
        private IEnumerator PerformPopTransition()
        {
            if (popType == PopType.PopIn)
            {
                // Pop In: Current scene disappears, and new scene panel pops in
                currentScenePanel.localScale = Vector3.one;
                otherScenePanel.localScale = Vector3.zero; // Start other scene hidden

                // Ensure Scene B's panel is centered before animation
                otherScenePanel.anchoredPosition = Vector2.zero;

                // Instantly hide the current scene
                currentScenePanel.gameObject.SetActive(false);

                float elapsedTime = 0f;
                while (elapsedTime < 1f)
                {
                    elapsedTime += Time.deltaTime * transitionSpeed;
                    otherScenePanel.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, elapsedTime); // Pop in the new scene
                    yield return null;
                }

                // Load the actual next scene after pop-in animation ends
                sceneLoader.LoadScene();
            }
            else if (popType == PopType.PopOut)
            {
                // Pop Out: Animate the current scene out, then load the next scene without showing the next panel during animation
                currentScenePanel.localScale = Vector3.one;

                float elapsedTime = 0f;
                while (elapsedTime < 1f)
                {
                    elapsedTime += Time.deltaTime * transitionSpeed;
                    currentScenePanel.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, elapsedTime); // Pop out the current scene
                    yield return null;
                }

                // Load the next scene after pop-out animation ends
                sceneLoader.LoadScene();  // Load the actual scene after the pop-out animation is completed
            }
        }

        // - Smooth Flow Transition Coroutine - //
        private IEnumerator PerformSmoothFlowTransition()
        {
            // Determine initial positions based on flow direction
            float screenWidth = Screen.width;
            Vector2 currentSceneStartPos = Vector2.zero;
            Vector2 currentSceneEndPos;
            Vector2 nextSceneStartPos;

            if (flowDirection == FlowDirection.Left)
            {
                currentSceneEndPos = new Vector2(-screenWidth * 0.95f, 0);  // Move left
                nextSceneStartPos = new Vector2(screenWidth, 0);  // Start from the right
            }
            else
            {
                currentSceneEndPos = new Vector2(screenWidth * 0.95f, 0);  // Move right
                nextSceneStartPos = new Vector2(-screenWidth, 0);  // Start from the left
            }

            // Initial setup
            otherScenePanel.anchoredPosition = nextSceneStartPos;
            currentScenePanel.anchoredPosition = currentSceneStartPos;

            // Add CanvasGroup components for fading
            CanvasGroup currentCanvasGroup = currentScenePanel.GetComponent<CanvasGroup>() ?? currentScenePanel.gameObject.AddComponent<CanvasGroup>();
            CanvasGroup nextCanvasGroup = otherScenePanel.GetComponent<CanvasGroup>() ?? otherScenePanel.gameObject.AddComponent<CanvasGroup>();

            // Ensure the next scene starts faded out and scaled down
            nextCanvasGroup.alpha = 0;
            otherScenePanel.localScale = new Vector3(0.85f, 0.85f, 1); // Starts slightly smaller

            float elapsedTime = 0f;
            float totalDuration = 1f / transitionSpeed;
            AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

            while (elapsedTime < totalDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / totalDuration;
                float easeT = easingCurve.Evaluate(t);

                // Current scene panel animation
                currentScenePanel.anchoredPosition = Vector2.Lerp(currentSceneStartPos, currentSceneEndPos, easeT);
                currentScenePanel.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.95f, 0.95f, 1), easeT);
                currentScenePanel.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(0, flowDirection == FlowDirection.Left ? -zRotationAmount : zRotationAmount, easeT));  // Z rotation
                currentCanvasGroup.alpha = Mathf.Lerp(1f, 0.7f, easeT);

                // Next scene panel animation
                otherScenePanel.anchoredPosition = Vector2.Lerp(nextSceneStartPos, Vector2.zero, easeT);
                otherScenePanel.localScale = Vector3.Lerp(new Vector3(0.85f, 0.85f, 1), Vector3.one, easeT);
                otherScenePanel.localRotation = Quaternion.Euler(0, 0, Mathf.Lerp(flowDirection == FlowDirection.Left ? reverseZRotationAmount : -reverseZRotationAmount, 0, easeT));  // Reverse Z rotation
                nextCanvasGroup.alpha = Mathf.Lerp(0f, 1f, easeT);

                yield return null;
            }

            // Load the next scene after the transition completes
            sceneLoader.LoadScene();
        }


        // - Glitch Transition Coroutine - //
        private IEnumerator PerformGlitchTransition()
        {
            float elapsedTime = 0f;
            Vector2 originalPosition = currentScenePanel.anchoredPosition;
            CanvasGroup currentCanvasGroup = currentScenePanel.GetComponent<CanvasGroup>() ?? currentScenePanel.gameObject.AddComponent<CanvasGroup>();

            while (elapsedTime < glitchDuration)
            {
                elapsedTime += Time.deltaTime;
                float glitchProgress = Mathf.PingPong(elapsedTime * transitionSpeed * 10f, 1f);  // Randomize movement

                // Jitter effect (small random shifts)
                currentScenePanel.anchoredPosition = originalPosition + new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f)) * glitchProgress;

                // Add a quick flickering color or distortion
                currentCanvasGroup.alpha = Random.Range(0.5f, 1f);

                // Optional: Add chromatic aberration-like color shifts or pixelation (can use shaders here if needed)
                yield return null;
            }

            // Reset position and alpha
            currentScenePanel.anchoredPosition = originalPosition;
            currentCanvasGroup.alpha = 1f;

            // Load the next scene after the glitch effect
            sceneLoader.LoadScene();
        }

    }
}