using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private bool _collided;
    public float damage, particleDestoyDelay;
    public GameObject Marker;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag  != "Bullet" && collision.gameObject.tag != "Player" && !_collided)
        {
            _collided = true;
            var impact = Instantiate(Marker, collision.contacts[0].point, Quaternion.identity) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other) // будет исчезать при столкновении с любым тригером
    {
        if (!_collided)
        {
            _collided = true;
            var impact = Instantiate(Marker, other.ClosestPoint(transform.position), other.transform.rotation) as GameObject;
            Destroy(impact, 2);
            Destroy(gameObject);
        }
    }
}
