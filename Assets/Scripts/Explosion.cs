using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float force = 1;
    SphereCollider sphereCollider;
    Vector3 centerOfExplosion;
    float colliderRadius = 1;

    public void SetForce(float f)
    {
        force = f;
    }

    public void SetCenterOfExplosion(Vector3 pos)
    {
        centerOfExplosion = pos;
    }

    public void SetColliderRadius(float r)
    {
        colliderRadius = r;
    }

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = colliderRadius;
        StartCoroutine(DelayedDestroyed(3));
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }
        rb.AddExplosionForce(force, centerOfExplosion, 0);
    }

    IEnumerator DelayedDestroyed(int frame)
    {
        for (int i = 0; i < frame; i++)
        {
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }
}
