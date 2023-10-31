using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private Transform origin;

    [SerializeField]
    private float shootCoolDown;

    private float timeSinceLastShot;
    private bool shootable = false;

    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shootable == false)
        {
            timeSinceLastShot += Time.deltaTime;
        }
        if (timeSinceLastShot >= shootCoolDown)
        {
            shootable = true;
        }
    }

    public void Shoot(Vector3 direction, string targetTag)
    {
        if (shootable == false)
        {
            return;
        }
        Bullet newBullet = Instantiate(bulletPrefab, origin.position, Quaternion.identity).GetComponent<Bullet>();
        newBullet.SetDirection(direction);
        newBullet.SetTarget(targetTag);
        audio.Play();
        timeSinceLastShot = 0;
        shootable = false;
    }
}
