using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AiLocomotion : MonoBehaviour
{

    public Transform playerTransform;
    public float maxDistance =1.0f;
    public float maxTime = 1.0f;


    NavMeshAgent agent;
    Animator animator;
    float timer = 0.0f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
     
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0.0f)
        {
            float sqDistance = (playerTransform.position - agent.destination).sqrMagnitude;
            if(sqDistance > maxDistance*maxDistance) {
                agent.destination = playerTransform.position;
            }
            timer = maxTime;
        }
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }
}
