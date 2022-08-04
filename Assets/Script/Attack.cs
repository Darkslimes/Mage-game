using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Camera _cam;
    [SerializeField] private Vector3 _vector;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _firepoint;
    [SerializeField] private float _speed = 30;
    public Animator anim;

    private void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            ShootProjectile();
        }

        if (Input.GetMouseButton(0))
        {
            anim.SetTrigger("slash");
        }
    }

     void ShootProjectile()
     {
        Ray _ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit _hit;

        if (Physics.Raycast(_ray, out _hit))
            _vector = _hit.point;
        else
            _vector = _ray.GetPoint(1000);

        InstantiateProjectile(_firepoint);
     }

     void InstantiateProjectile(Transform firepoint)
     {
        var _projectileObj = Instantiate(_projectile, firepoint.position, Quaternion.identity) as GameObject;
        _projectileObj.GetComponent<Rigidbody>().velocity = (_vector - firepoint.position).normalized * _speed;
     }
    private void SetAnimZero()
    {
        anim.SetFloat("InputMagnitude", 0);
        anim.SetFloat("InputZ", 0);
        anim.SetFloat("InputX", 0);
    }
}
