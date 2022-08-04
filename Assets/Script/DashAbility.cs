using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{

    WhiteMageController WMG;
    public ParticleSystem ps;

    public float dashSpeed;
    public float dashTime;

    void Start()
    {
        WMG = GetComponent<WhiteMageController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(Dash());
        }
    }

    public IEnumerator Dash()
    {
        if (WMG.casting == false && WMG.canMove == true)
        {
            float startTime = Time.time;
            while (Time.time < startTime + dashTime)
            {
                ps.Play();
                WMG.controller.Move(WMG.desiredMoveDirection * dashSpeed * Time.deltaTime);
                yield return null;
                ps.Stop();
            }
        }

    }
}
