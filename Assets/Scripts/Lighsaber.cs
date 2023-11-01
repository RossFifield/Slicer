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

    [SerializeField]
    [ColorUsage(true, true)]
    [Tooltip("The colour of the blade and trail")]
    private Color _colour = Color.red;

    [SerializeField]
    [Tooltip("The amount of force applied to each side of a slice")]
    private float _forceAppliedToCut = 3f;

    public Collider shield;

    private Mesh _mesh;
    private MeshCollider _meshCollider;
    private Animator _anim;
    private Vector3[] _vertices;
    private int[] _triangles;
    private int _frameCount;
    private Vector3 _previousTipPosition;
    private Vector3 _previousBasePosition;
    private Vector3 _triggerEnterTipPosition;
    private Vector3 _triggerEnterBasePosition;
    private Vector3 _triggerExitTipPosition;

    private Vector3 _planeNormal;
    private Vector3 _planePoint;

    void Start()
    {
        // get variables
        _meshCollider = GetComponentInChildren<MeshCollider>();
        _anim = GetComponentInChildren<Animator>();

        //start with collider disabled
        _meshCollider.enabled = false;
        

        //Set starting position for tip and base
        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
        
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
    
    void LateUpdate()
    {
             //Track the previous base and tip positions for the next frame
        _previousTipPosition = _tip.transform.position;
        _previousBasePosition = _base.transform.position;
        //_frameCount += NUM_VERTICES;
    }

    private void OnTriggerEnter(Collider other)
    {
        _triggerEnterTipPosition = _tip.transform.position;
        _triggerEnterBasePosition = _base.transform.position;
        Sliceable sliced = other.GetComponent<Sliceable>();
        Debug.Log("is this slicable? "+ (sliced != null));
        Debug.Log("is this slashing? "+_anim.GetCurrentAnimatorStateInfo(0).IsName("SwordSlash"));
        if(sliced != null && _anim.GetCurrentAnimatorStateInfo(0).IsName("SwordSlash") ){
            Debug.Log("I Entered something!");
            CutSomething(other);
        }   
    }

    private void OnTriggerExit(Collider other)
    {
        
        _triggerExitTipPosition = _tip.transform.position;
            
    }

    void CutSomething(Collider other){
        //Create a triangle between the tip and base so that we can get the normal
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;

        //Get the point perpendicular to the triangle above which is the normal
        //https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        //Transform the normal so that it is aligned with the object we are slicing's transform.
        Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * _planeNormal)).normalized;

        //Get the enter position relative to the object we're cutting's local transform
        Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(_planePoint);

        Plane plane = new Plane();

        plane.SetNormalAndPosition(
                transformedNormal,
                transformedStartingPoint);

        var direction = Vector3.Dot(Vector3.up, transformedNormal);

        //Flip the plane so that we always know which side the positive mesh is on
        if (direction < 0)
        {
            plane = plane.flipped;
        }

        GameObject[] slices = Slicer.Slice(plane, other.gameObject);
        if (slices == null)
        {
            return;
        }
        Destroy(other.gameObject);
       
        //slices[0].GetComponent<MeshFilter>().mesh.RecalculateUVDistributionMetrics();
        //slices[0].GetComponent<MeshFilter>().mesh.RecalculateNormals();
        slices[0].GetComponent<MeshFilter>().mesh.Optimize();
        //slices[1].GetComponent<MeshFilter>().mesh.RecalculateUVDistributionMetrics();
        //slices[1].GetComponent<MeshFilter>().mesh.RecalculateNormals();
        slices[1].GetComponent<MeshFilter>().mesh.Optimize();

        Rigidbody rigidbody = slices[1].GetComponent<Rigidbody>();
        Vector3 newNormal = transformedNormal + Vector3.up * _forceAppliedToCut;
        rigidbody.AddForce(newNormal, ForceMode.Impulse);
    }

    IEnumerator SwordSwing2(Vector3 camPos)
    {
        _meshCollider.enabled = true;
        //do normal randomization magic
        double angle=RandomizeCut(camPos);
        _anim.SetTrigger("Cut");
         //play slicing sound
         AudioManager.GetInstance().PlaySoundEffect("Sounds/Lightsaber/Lightsaber_swing_1",gameObject.transform);
        GetComponent<AudioSource>().Play();
        //wait to finish animation
        yield return new WaitForSeconds(1.0f);
        gameObject.transform.parent.transform.Rotate(0,0,(float)(-angle));
        _meshCollider.enabled = false;
    }
    double RandomizeCut(Vector3 camPos){
        //TODO debug why the fuk is not working :(
        Vector3 randomizedDirection = (new Vector3(Random.Range(0,2)-1,Random.Range(0,2)-1,0)).normalized;
        Vector3 targetPoint = camPos + randomizedDirection;
        _planeNormal = targetPoint;
        _planePoint = camPos;
        float xDiff = targetPoint.x - camPos.x;
        float yDiff = targetPoint.y - camPos.y;
        double a = System.Math.Atan2(yDiff, xDiff) * 180.0 / System.Math.PI;
        gameObject.transform.parent.Rotate(0,0,(float)a);
        return a;
    }
}
