using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneConstraints : MonoBehaviour
{
    [SerializeField] GameObject plane;
    [SerializeField] float zmax;
    [SerializeField] float zmin;
    [SerializeField] float xmax;
    [SerializeField] float xmin;
    [SerializeField] float shift;


    Rigidbody rb;
    BoxCollider c;

    // Start is called before the first frame update
    void Start()
    {
        rb = plane.GetComponent<Rigidbody>();
        c = plane.GetComponent<BoxCollider>();
    }

    private void LateUpdate()
    {
        CheckBoundaries();
    }

    void CheckBoundaries()
    {
        Vector3 center = c.center;
        Vector3 left = plane.transform.TransformPoint(new Vector3(center.x - c.size.x / 2, center.y, center.z + c.size.y));
        Vector3 middle = plane.transform.TransformPoint(new Vector3(center.x, center.y, center.z + c.size.y)); 
        Vector3 right = plane.transform.TransformPoint(new Vector3(center.x + c.size.x / 2, center.y, center.z + c.size.y));
        
        
        if(left.x < xmin || middle.x < xmin || right.x < xmin)
        {
            plane.transform.position += Vector3.right * (Mathf.Max(xmin - left.x, Mathf.Max(xmin - middle.x, xmin - right.x)) + shift);
        }
        else if(left.x > xmax || middle.x > xmax || right.x > xmax)
        {
            plane.transform.position += Vector3.left * ( Mathf.Max(xmax - left.x, Mathf.Max(xmax - middle.x, xmax - right.x)) + shift);
        }


        if (left.z < zmin || middle.z < zmin || right.z < zmin)
        {
            plane.transform.position += Vector3.forward * (Mathf.Max(zmin - left.z, Mathf.Max(zmin - middle.z, zmin - right.z)) + shift);
        }
    
        else if (left.z > zmax || middle.z > zmax || right.z > zmax)
        {
            plane.transform.position += Vector3.back * (Mathf.Max(zmax - left.z, Mathf.Max(zmax - middle.z, zmax - right.z)) +  shift);
        }
   

    }


}
