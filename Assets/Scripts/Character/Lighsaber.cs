using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Lighsaber : MonoBehaviour
{
    //The number of vertices to create per frame
    private const int NUM_VERTICES = 12;

    // [SerializeField]
    // [Tooltip("The blade object")]
    // private GameObject _blade = null;
     
    [SerializeField]
    [Tooltip("The empty game object located at the tip of the blade")]
    private GameObject _tip = null;

    [SerializeField]
    [Tooltip("The empty game object located at the base of the blade")]
    private GameObject _base = null;

    // [SerializeField]
    // [Tooltip("The mesh object with the mesh filter and mesh renderer")]
    // private GameObject _meshParent = null;

    // [SerializeField]
    // [Tooltip("The number of frame that the trail should be rendered for")]
    // private int _trailFrameLength = 3;

    /*
    [SerializeField]
    [ColorUsage(true, true)]
    [Tooltip("The colour of the blade and trail")]
    private Color _colour = Color.red;
    */

    [SerializeField]
    private GameObject explosionTemplate;
    [SerializeField]
    [Tooltip("The amount of force applied to each side of a slice")]
    private float _forceAppliedToCut = 3f;

    public Collider shield;


    private BoxCollider _meshCollider;
    private Animator _anim;
    private Vector3 _triggerEnterTipPosition;
    private Vector3 _triggerEnterBasePosition;
    private Vector3 _triggerExitTipPosition;
    private double cutAngle=0;

    private Vector3 _planeNormal;
    private Vector3 _planePoint;

    void Start()
    {
        // get variables
        _meshCollider = GetComponentInChildren<BoxCollider>();
        _anim = GetComponentInChildren<Animator>();

        //start with collider disabled
        _meshCollider.enabled = false;     
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Vector3 camPos = Camera.main.transform.position;
            StartCoroutine(SwordSwing2(camPos));
        }
        //Blocking or not
        if(Input.GetMouseButtonDown(1)){
            _anim.SetBool("Blocking",true);
            shield.enabled=true;
        }
        if(Input.GetMouseButtonUp(1)){
            _anim.SetBool("Blocking",false);
            shield.enabled=false;
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        _triggerEnterTipPosition = _tip.transform.position;
        _triggerEnterBasePosition = _base.transform.position;
        
    }

    private void OnTriggerExit(Collider other)
    {
        _triggerExitTipPosition = _tip.transform.position;
        //Sliceable sliced = other.GetComponent<Sliceable>();
        Slice sliced = other.GetComponent<Slice>();
        if (sliced != null && _anim.GetCurrentAnimatorStateInfo(0).IsName("SwordSlash") ){
            CutSomething(other.gameObject);
        }   
    }

    void CutSomething(GameObject other){

        foreach (Transform child in other.transform)
        {
            Debug.Log("child");
            CutSomething(child.gameObject);
        }
        if (other.GetComponent<MeshFilter>() == null && other.GetComponent<SkinnedMeshRenderer>() == null)
        {
            Debug.Log("Destroyed " + other.gameObject.name);
            Destroy(other.gameObject);
            return;
        }
        if (other.GetComponent<SkinnedMeshRenderer>() != null)
        {
            MeshFilter newMeshFilter = other.AddComponent<MeshFilter>();
            Mesh bakedMesh = new Mesh();
            other.GetComponent<SkinnedMeshRenderer>().BakeMesh(bakedMesh);
            newMeshFilter.mesh = new Mesh();
            newMeshFilter.mesh = bakedMesh;
        }
        //Create a triangle between the tip and base so that we can get the normal
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;

        //Get the point perpendicular to the triangle above which is the normal
        //https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        Slice sliced = other.GetComponent<Slice>();
        if (sliced != null)
        {
            sliced.callbackOptions.onCompleted.AddListener(SpawnExplosion);
            sliced.ComputeSlice(normal, _triggerExitTipPosition);
            //SpawnExplosion();
        }
        StartCoroutine(DelayedDetroy(other.gameObject));
        //Destroy(other.gameObject);
    }

    IEnumerator DelayedDetroy(GameObject obj)
    {
        yield return null;
        yield return null;
        yield return null;
        Destroy(obj);
    }


    public void ResetPositionAfterAnimation(){
        _meshCollider.enabled = false;
        gameObject.transform.parent.transform.localRotation = Quaternion.identity;
        
    }

    IEnumerator SwordSwing2(Vector3 camPos)
    {
        _meshCollider.enabled = true;
        //do normal randomization magic
        cutAngle=RandomizeCut(camPos);
        _anim.SetTrigger("Cut");
         //play slicing sound
         AudioManager.GetInstance().PlaySoundEffect("Sounds/Lightsaber/Lightsaber_swing_1",gameObject.transform,0.5f);
        //wait to finish animation
        yield return new WaitForSeconds(1.0f);
        
    }
    double RandomizeCut(Vector3 camPos){
        Vector3 randomizedDirection = (new Vector3(Random.Range(0f,2f)-1,Random.Range(0f,2f)-1,0)).normalized;
        Vector3 targetPoint = camPos + randomizedDirection;
        _planeNormal = targetPoint;
        _planePoint = camPos;
        float xDiff = targetPoint.x - camPos.x;
        float yDiff = targetPoint.y - camPos.y;
        double a = System.Math.Atan2(yDiff, xDiff) * 180.0 / System.Math.PI;
        gameObject.transform.parent.Rotate(0,0,(float)a);
        return a;
    }

    void SpawnExplosion()
    {
        Vector3 midpoint = (_triggerEnterBasePosition + _triggerEnterTipPosition) / 2;
        Explosion explosion = Instantiate(explosionTemplate, midpoint, Quaternion.identity).GetComponent<Explosion>();
        explosion.SetForce(_forceAppliedToCut);
        explosion.SetCenterOfExplosion(_triggerEnterBasePosition);
        explosion.SetColliderRadius((_triggerEnterBasePosition - _triggerEnterTipPosition).magnitude);
    }
}
