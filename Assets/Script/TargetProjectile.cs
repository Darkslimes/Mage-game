using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjectile : MonoBehaviour
{
    public float speed = 15f;
    public GameObject hit;
    public GameObject flash;
    public GameObject[] Detached;
    private Transform target;
    private Vector3 targetOffset;
    public float damageAmount = 5f;
    private ParticleSystem part;

    Health health;

    [Space]
    [Header("PROJECTILE PATH")]
    private float randomUpAngle;
    private float randomSideAngle;
    public float sideAngle = 25;
    public float upAngle = 20;

    void Start()
    {
        
        part = GetComponent<ParticleSystem>(); 
        FlashEffect();
        newRandom();
    }

    void newRandom()
    {
        randomUpAngle = Random.Range(0, upAngle);
        randomSideAngle = Random.Range(-sideAngle, sideAngle);
    }

    //Link from movement controller
    //TARGET POSITION + TARGET OFFSET
    public void UpdateTarget(Transform targetPosition , Vector3 Offset)
    {
        target = targetPosition;
        targetOffset = Offset;
    }

    void Update()
    {
        if (target == null)
        {
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {

                    detachedPrefab.transform.parent = null;
                }
            }
            Destroy(gameObject);
            return;
        }


        Vector3 forward = ((target.position + targetOffset) - transform.position);
        Vector3 crossDirection = Vector3.Cross(forward, Vector3.up);
        Quaternion randomDeltaRotation = Quaternion.Euler(0, randomSideAngle, 0) * Quaternion.AngleAxis(randomUpAngle, crossDirection);
        Vector3 direction = randomDeltaRotation * ((target.position + targetOffset) - transform.position);

        float distanceThisFrame = Time.deltaTime * speed;

        if (direction.magnitude <= distanceThisFrame)
        {
            //Debug.Log("hit1");
            //health.GetComponent<Health>().TakeDamage(damageAmount);
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceThisFrame, Space.World);
        transform.rotation = Quaternion.LookRotation(direction);
    }
    //private void OnParticleCollision(GameObject other)
    //{
    //    if(other.tag == "Enemy")
    //    {
    //        Debug.Log("hit");
    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.tag == "Enemy")
    //    {
    //        other.GetComponent<Health>().TakeDamage(damageAmount);
    //        Debug.Log("hit");
    //    }
    //}

    void FlashEffect()
    {
        if (flash != null)
        {

            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }

    void HitTarget()
    {
        if (hit != null)
        { 
            var hitInstance = Instantiate(hit, target.position + targetOffset, transform.rotation);
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {

                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }     
        Destroy(gameObject);
    }
}
