using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SeismoCube : MonoBehaviour
{

    public InputActionReference buttonAction; //reference to the mapped trigger button action
    public Transform rightController; //reference to the right VR controller for raycasting
    public GameObject lowIntensityScreen, mediumIntensityScreen, highIntensityScreen;
    public GameObject seismoCube1, seismoCube2, seismoCube3;
    public float maxRaycastDistance = 10f;//max distance

    private void Start()
    {

    }
    private void Update()
    {
        if (buttonAction.action.IsPressed()) //detect trigger press
        {
            RaycastHit hit;
            if (Physics.Raycast(rightController.position, rightController.forward, out hit, maxRaycastDistance)) //cast a ray from the right controller
            {
                if (hit.collider.CompareTag("Button1")) //check if hit object is tagged as "Button"
                {
                    highIntensityScreen.SetActive(false);
                    lowIntensityScreen.SetActive(true);
                    mediumIntensityScreen.SetActive(false);
                    seismoCube1.GetComponent<Animator>().enabled = true;
                    seismoCube2.GetComponent<Animator>().enabled = false;
                    seismoCube3.GetComponent<Animator>().enabled = false;
                }
                else if (hit.collider.CompareTag("Button2"))
                {
                    highIntensityScreen.SetActive(false);
                    lowIntensityScreen.SetActive(false);
                    mediumIntensityScreen.SetActive(true);
                    seismoCube1.GetComponent<Animator>().enabled = false;
                    seismoCube2.GetComponent<Animator>().enabled = true;
                    seismoCube3.GetComponent<Animator>().enabled = false;
                }
                else if (hit.collider.CompareTag("Button3"))
                {
                    highIntensityScreen.SetActive(true);
                    lowIntensityScreen.SetActive(false);
                    mediumIntensityScreen.SetActive(false);
                    seismoCube1.GetComponent<Animator>().enabled = false;
                    seismoCube2.GetComponent<Animator>().enabled = false;
                    seismoCube3.GetComponent<Animator>().enabled = true;
                }
                else if (hit.collider.CompareTag("Button4"))
                {
                    highIntensityScreen.SetActive(false);
                    lowIntensityScreen.SetActive(false);
                    mediumIntensityScreen.SetActive(false);
                    seismoCube1.GetComponent<Animator>().enabled = false;
                    seismoCube2.GetComponent<Animator>().enabled = false;
                    seismoCube3.GetComponent<Animator>().enabled = false;
                }

            }
        }
    }
}

