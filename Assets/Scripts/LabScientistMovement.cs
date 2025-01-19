using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LabScientistMovement : MonoBehaviour
{
    public Transform target;          // The target position for the scientist to walk to
    public float speed = 2.0f;        // Walking speed
    public float stopDistance = 0.5f; // Distance to stop from the target
    public Animator animator;         // Animator component for the character

    private bool isWalking = false;

    private void Update()
    {
        if (target == null || animator == null) return;

        // Calculate the distance to the target
        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        // If the character is close enough to the target, switch to idle animation
        if (distanceToTarget <= stopDistance)
        {
            if (isWalking)
            {
                isWalking = false;
                animator.SetBool("Walk", false); // Stop the walk animation
                animator.SetBool("Idle", true);  // Trigger the idle animation
            }
            return;
        }

        // If not at the target, move towards it
        MoveTowardsTarget();
    }

    private void MoveTowardsTarget()
    {
        // Start the walk animation if not already walking
        if (!isWalking)
        {
            isWalking = true;
            animator.SetBool("Idle", false); // Ensure the idle animation stops
            animator.SetBool("Walk", true); // Trigger the walk animation
        }

        // Calculate direction and move the character
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Rotate the character to face the target
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
    }
}
