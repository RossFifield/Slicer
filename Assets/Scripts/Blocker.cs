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
            Debug.Log("bullet blocked");
            //Play blocking sound
            GetComponent<AudioSource>().Play();
        }
    }
}
