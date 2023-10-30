using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighsaber : MonoBehaviour
{
    //The number of vertices to create per frame
    private const int NUM_VERTICES = 12;

    [SerializeField]
    [Tooltip("The blade object")]
    private GameObject _blade = null;
     
    [SerializeField]
    [Tooltip("The empty game object located at the tip of the blade")]
    private GameObject _tip = null;

    [SerializeField]
    [Tooltip("The empty game object located at the base of the blade")]
    private GameObject _base = null;

    [SerializeField]
    [Tooltip("The mesh object with the mesh filter and mesh renderer")]
    private GameObject _meshParent = null;

    [SerializeField]
    [Tooltip("The number of frame that the trail should be rendered for")]
    private int _trailFrameLength = 3;

    [SerializeField]
    [ColorUsage(true, true)]
    [Tooltip("The colour of the blade and trail")]
    private Color _colour = Color.red;

    [SerializeField]
    [Tooltip("The amount of force applied to each side of a slice")]
    private float _forceAppliedToCut = 3f;

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
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SwordSwing2());
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
    }

    private void OnTriggerExit(Collider other)
    {
        _triggerExitTipPosition = _tip.transform.position;
        Sliceable sliced = other.GetComponent<Sliceable>();
        if(sliced != null){
            CutSomething(other);
        }       
    }

    void CutSomething(Collider other){
        //Create a triangle between the tip and base so that we can get the normal
        Vector3 side1 = _triggerExitTipPosition - _triggerEnterTipPosition;
        Vector3 side2 = _triggerExitTipPosition - _triggerEnterBasePosition;

        Vector3[] triangle = new Vector3[3];
        triangle[0] = _triggerEnterTipPosition;
        triangle[1] = _triggerEnterBasePosition;
        triangle[2] = _triggerExitTipPosition;

        //Get the point perpendicular to the triangle above which is the normal
        //https://docs.unity3d.com/Manual/ComputingNormalPerpendicularVector.html
        Vector3 normal = Vector3.Cross(side1, side2).normalized;

        //Transform the normal so that it is aligned with the object we are slicing's transform.
        Vector3 transformedNormal = ((Vector3)(other.gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        //Get the enter position relative to the object we're cutting's local transform
        Vector3 transformedStartingPoint = other.gameObject.transform.InverseTransformPoint(_triggerEnterTipPosition);

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

        GameObject[] slices = Slicer.Slice(plane, other.gameObject,triangle);
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

    IEnumerator SwordSwing2()
    {
        
        _meshCollider.enabled = true;
        _anim.SetTrigger("Cut");
         //play slicing sound
        GetComponent<AudioSource>().Play();
        //wait to finish animation
        yield return new WaitForSeconds(1.0f);
        //Sword.GetComponent<Animator>().Play("NewState");
        _meshCollider.enabled = false;
    }
}
