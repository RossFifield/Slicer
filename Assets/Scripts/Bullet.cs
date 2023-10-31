using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Vector3 direction;
    private Rigidbody rb;
    private string targetTag;

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }

    public void SetTarget(string target)
    {
        targetTag = target;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        transform.LookAt(transform.position + direction);
    }

    // Update is called once per frame
    void Update()
    {
        //rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            Destroy(gameObject);
        }
        else if (other.transform.root.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}
