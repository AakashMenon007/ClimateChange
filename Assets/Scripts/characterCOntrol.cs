using UnityEngine;
using UnityEngine.AI;
using System.Collections;


public class CharacterMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;
    public Transform target;
    public float idleTime = 2f; // How long to stay idle before walking

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        StartCoroutine(IdleAndWalkCycle());
    }

    private IEnumerator IdleAndWalkCycle()
    {
        // Idle for a few seconds
        animator.SetBool("IsWalking", false); // Idle animation
        yield return new WaitForSeconds(idleTime);

        // Walk to the target
        animator.SetBool("IsWalking", true); // Walk animation
        agent.SetDestination(target.position);

        // Wait until the agent reaches the target
        while (Vector3.Distance(agent.transform.position, target.position) > 0.1f)
        {
            yield return null; // Wait for the agent to reach the target
        }

        // Transition back to idle
        animator.SetBool("IsWalking", false);
    }
}
