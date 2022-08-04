using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform spawn;
    public Transform player;
    public  float destroyDelay = 5;
    public LayerMask whatIsGround, whatIsPlayer;
    Animator animator;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    public float heath; 

    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    public float sightRange, attackRange;
    public bool playerInSinghtRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        playerInSinghtRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSinghtRange && !playerInAttackRange) Patroling();
        if (playerInSinghtRange && !playerInAttackRange) ChasePlayer();
        if (playerInSinghtRange && playerInAttackRange) StartCoroutine(AttackPlayer());

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    //private void AttackPlayer()
    //{
    //    agent.SetDestination(transform.position);

    //    transform.LookAt(player);

    //    if (!alreadyAttacked)
    //    {
    //        animator.SetTrigger("ThrowProjectile");
    //        Rigidbody rb = Instantiate(projectile, spawn.transform.position, spawn.transform.rotation).GetComponent<Rigidbody>();
    //        rb.AddForce(transform.forward * 40f, ForceMode.Impulse);
    //        rb.AddForce(transform.up * 5f, ForceMode.Impulse);

    //        alreadyAttacked = true;
    //        Invoke(nameof(ResetAttack), timeBetweenAttacks);
    //    }
    //}
    public IEnumerator AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            animator.SetTrigger("ThrowProjectile");
            yield return new WaitForSeconds(0.3f);
            Rigidbody rb = Instantiate(projectile, spawn.transform.position, spawn.transform.rotation).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 40f, ForceMode.Impulse);
            rb.AddForce(transform.up * 3f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
        yield return null;
    }


    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    //public void TakeDamage(int damage)
    //{
    //    heath -= damage;

    //    if (heath <= 0) Invoke(nameof(DestoyEnemy), 0.5f);
    //}

    //private void DestoyEnemy()
    //{
    //    Destroy(gameObject);
    //}
}
