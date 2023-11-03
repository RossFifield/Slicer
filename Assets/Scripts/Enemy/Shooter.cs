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

    [SerializeField]
    private float inaccuracy;

    private float timeSinceLastShot;
    private bool shootable = false;

    private string soundPath = "Sounds/Guns/Blaster_1";

    private AudioSource audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
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
        float noiseX = direction.x + Random.Range(-inaccuracy, inaccuracy);
        float noiseY = direction.y + Random.Range(-inaccuracy, inaccuracy);
        float noiseZ = direction.z + Random.Range(-inaccuracy, inaccuracy);
        direction = new Vector3(noiseX, noiseY, noiseZ);
        direction.Normalize();
        newBullet.SetDirection(direction);
        newBullet.SetTarget(targetTag);
        AudioManager.GetInstance().PlaySoundEffect(soundPath,this.transform,1.5f);
        timeSinceLastShot = 0;
        shootable = false;
    }
}
