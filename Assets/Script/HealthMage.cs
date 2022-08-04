using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMage : MonoBehaviour
{
    public float healthPoint;
    Animator animator;
    public float currectHealthPoint;
    public float speed = 5;

    void Start()
    {
        currectHealthPoint = healthPoint;
        animator = GetComponent<Animator>();
    }

    public IEnumerator TakeDamage()
    {
        currectHealthPoint -= Time.deltaTime * speed;
        //currectHealthPoint -= damageAmount;
        if (currectHealthPoint <= 0.0f)
        {
            GetComponent<EnemyAI>().enabled = false;
            animator.SetTrigger("Die");
            ShowBody(false);
            GetComponent<BoxCollider>().enabled = false;
            animator.enabled = false;
            yield return null;

        }
    }
    private void Update()
    {
        StartCoroutine(TakeDamage());
    }

    void ShowBody(bool state)
    {
        SkinnedMeshRenderer[] skinMeshList = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer smr in skinMeshList)
        {
            smr.enabled = state;
        }
    }
}
