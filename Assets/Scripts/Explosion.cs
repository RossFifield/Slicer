using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float force = 1;
    SphereCollider sphereCollider;

    public void SetForce(float f)
    {
        force = f;
    }
    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        StartCoroutine(DelayedDestroyed(3));
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null)
        {
            return;
        }
        rb.AddExplosionForce(force, transform.position, 0);
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
