using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Bullet")){
            Destroy(other.gameObject);
            //Play blocking sound
            AudioManager.GetInstance().PlaySoundEffect("Sounds/Lightsaber/Lightsaber_deflection",gameObject.transform,0.5f);
        }
    }
}
