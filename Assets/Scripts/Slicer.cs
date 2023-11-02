﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    class Slicer
    {
        /// <summary>
        /// Slice the object by the plane 
        /// </summary>
        /// <param name="plane"></param>
        /// <param name="objectToCut"></param>
        /// <returns></returns>
        /// 
        //private static float cutThreshold = 0.3f;
        public static GameObject[] Slice(Plane plane, GameObject objectToCut)
        {            
            //Get the current mesh and its verts and tris
            Mesh mesh = objectToCut.GetComponent<MeshFilter>().mesh;
            var a = mesh.GetSubMesh(0);
            Sliceable sliceable = objectToCut.GetComponent<Sliceable>();

            if(sliceable == null)
            {
                Debug.Log("Not slicable");
                return null;
                //throw new NotSupportedException("Cannot slice non sliceable object, add the sliceable script to the object or inherit from sliceable to support slicing");
            }
            
            //Create left and right slice of hollow object
            SlicesMetadata slicesMeta = new SlicesMetadata(plane, mesh, sliceable.IsSolid, sliceable.ReverseWireTriangles, sliceable.ShareVertices, sliceable.SmoothVertices);

            // Deprecated?
            // calculate the area of the Triangle that covers the slicesMeta.PointsAlongPlane polygon. and the area of the polygon and see if it is enough to cut.

            // List<Vector3> intersectedPoints = slicesMeta.PointsAlongPlane;

            // double midCount = intersectedPoints.Count / 2;

            // Debug.Log("access index for middle point: "+(int)Math.Floor(midCount));

            // Vector3 middlePoint = intersectedPoints[0] + (intersectedPoints[0] - intersectedPoints[(int)Math.Floor(midCount)]) / 2;
            // middlePoint = objectToCut.transform.TransformVector(middlePoint);
            // float polygonArea = 0;

            // Vector3 lastPoint = objectToCut.transform.TransformVector(intersectedPoints.Last());

            // Vector3 closesPointToBase = lastPoint;
            // float minimumDistance = 99999999999;

            // foreach (Vector3 point in intersectedPoints)
            // {
            //     Vector3 localizedPoint = objectToCut.transform.TransformVector(point);
            //     // add the triangle area to the polygon area
            //     polygonArea += (Vector3.Cross(middlePoint - localizedPoint, middlePoint - lastPoint).magnitude) / 2;

            //     // find the distance between current point and base 
            //     float distance = Vector3.Distance(localizedPoint, triangle[1]);
            //     if (distance < minimumDistance)
            //     {
            //         closesPointToBase = localizedPoint;
            //         minimumDistance = distance;
            //     }
            //     //update last point for triangulation
            //     lastPoint = localizedPoint;
            // }

            // float cutArea = Vector3.Cross(triangle[0] - closesPointToBase, triangle[0] - triangle[2]).magnitude / 2;
            // if (cutArea <= polygonArea * cutThreshold)
            // {
            //     return null;
            // }

            GameObject positiveObject = CreateMeshGameObject(objectToCut, slicesMeta.OriginalVolume);
            positiveObject.name = string.Format("{0}_positive", objectToCut.name);

            GameObject negativeObject = CreateMeshGameObject(objectToCut, slicesMeta.OriginalVolume);
            negativeObject.name = string.Format("{0}_negative", objectToCut.name);

            var positiveSideMeshData = slicesMeta.PositiveSideMesh;
            var negativeSideMeshData = slicesMeta.NegativeSideMesh;

            positiveObject.GetComponent<MeshFilter>().mesh = positiveSideMeshData;
            negativeObject.GetComponent<MeshFilter>().mesh = negativeSideMeshData;

            SetupCollidersAndRigidBodys(ref positiveObject, positiveSideMeshData, sliceable.UseGravity);
            SetupCollidersAndRigidBodys(ref negativeObject, negativeSideMeshData, sliceable.UseGravity);

            return new GameObject[] { positiveObject, negativeObject};
        }

        /// <summary>
        /// Creates the default mesh game object.
        /// </summary>
        /// <param name="originalObject">The original object.</param>
        /// <returns></returns>
        private static GameObject CreateMeshGameObject(GameObject originalObject, float originalVolume)
        {
            Material[] originalMaterial;
            bool isSkinnedMesh;
            if (originalObject.GetComponent<MeshRenderer>() == null)
            {
                originalMaterial = originalObject.GetComponent<SkinnedMeshRenderer>().materials;
                isSkinnedMesh = true;
            }
            else
            {
                originalMaterial = originalObject.GetComponent<MeshRenderer>().materials;
                isSkinnedMesh = false;
            }
          
            GameObject meshGameObject = new GameObject();
            Sliceable originalSliceable = originalObject.GetComponent<Sliceable>();

            meshGameObject.AddComponent<MeshFilter>();
            meshGameObject.AddComponent<MeshRenderer>();
            Sliceable sliceable = meshGameObject.AddComponent<Sliceable>();

            sliceable.IsSolid = originalSliceable.IsSolid;
            sliceable.ReverseWireTriangles = originalSliceable.ReverseWireTriangles;
            sliceable.UseGravity = originalSliceable.UseGravity;
            //sliceable.OriginalVolume = originalVolume;

            meshGameObject.GetComponent<MeshRenderer>().materials = originalMaterial;

            meshGameObject.transform.localScale = originalObject.transform.localScale;
            meshGameObject.transform.rotation = originalObject.transform.rotation;
            meshGameObject.transform.position = originalObject.transform.position;

            meshGameObject.tag = originalObject.tag;

            return meshGameObject;
        }

        /// <summary>
        /// Add mesh collider and rigid body to game object
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="mesh"></param>
        private static void SetupCollidersAndRigidBodys(ref GameObject gameObject, Mesh mesh, bool useGravity)
        {                     
            MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
            meshCollider.convex = true;

            var rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = useGravity;
        }
    }
}
