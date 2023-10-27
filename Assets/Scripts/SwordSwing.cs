using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSwing : MonoBehaviour
{
    public GameObject Sword;

    // Start is called before the first frame update
    void Start()
    {
        Sword.GetComponent<MeshCollider>().enabled = false;
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
        Sword.GetComponent<MeshCollider>().enabled = true;
        Sword.GetComponent<Animator>().Play("SwordSwing");
        yield return new WaitForSeconds(1.0f);
        Sword.GetComponent<Animator>().Play("NewState");
        Sword.GetComponent<MeshCollider>().enabled = false;
    }
}
