using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public GameObject Sword;
    private MeshCollider mesh;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        Sword.GetComponent<MeshCollider>().enabled = false;
        mesh = Sword.GetComponent<MeshCollider>();
        anim = Sword.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SwordSwing2());
        }       
    }
    IEnumerator SwordSwing2()
    {
        
        mesh.enabled = true;
        anim.SetTrigger("Cut");
         //play slicing sound
        GetComponent<AudioSource>().Play();
        //wait to finish animation
        yield return new WaitForSeconds(1.0f);
        //Sword.GetComponent<Animator>().Play("NewState");
        mesh.enabled = false;
    }
}
