using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CharacterMovement2 : MonoBehaviour
{
    public NavMeshAgent agent;       // NavMeshAgent component
    public Animator animator;       // Animator component
    public Transform target;        // Target position
    public float idleTime = 2f;     // Idle duration before walking
    public float stopDistance = 0.5f; // Stopping distance from the target

    private void Start()
    {
        // Ensure NavMeshAgent and Animator are assigned
        if (!agent) agent = GetComponent<NavMeshAgent>();
        if (!animator) animator = GetComponent<Animator>();

        // Start the idle and walk cycle
        StartCoroutine(IdleAndWalkCycle());
    }

    private IEnumerator IdleAndWalkCycle()
    {
        // Idle phase
        animator.SetBool("IsWalking", false); // Trigger idle animation
        yield return new WaitForSeconds(idleTime); // Wait for idle duration

        // Walk phase
        animator.SetBool("IsWalking", true); // Trigger walk animation
        agent.SetDestination(target.position); // Set NavMeshAgent to move toward target

        // Wait until the agent reaches the target
        while (agent.remainingDistance > stopDistance || agent.pathPending)
        {
            yield return null; // Wait until agent reaches the target
        }

        // Ensure the agent stops moving
        agent.ResetPath();

        // Transition to idle animation
        animator.SetBool("IsWalking", false); // Trigger idle animation
    }
}
