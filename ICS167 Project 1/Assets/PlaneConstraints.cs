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
    [SerializeField] float ymax;
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
        Vector3 left = plane.transform.TransformPoint(new Vector3(center.x - c.size.x / 2, center.y, center.z + (c.size.z/2)));
        Vector3 middle = plane.transform.TransformPoint(new Vector3(center.x, center.y, center.z + (c.size.z / 2))); 
        Vector3 right = plane.transform.TransformPoint(new Vector3(center.x + c.size.x / 2, center.y, center.z + (c.size.z / 2)));

        Vector3 bleft = plane.transform.TransformPoint(new Vector3(center.x - c.size.x / 2, center.y, center.z - (c.size.z / 2)));
        Vector3 bmiddle = plane.transform.TransformPoint(new Vector3(center.x, center.y, center.z - (c.size.z / 2)));
        Vector3 bright = plane.transform.TransformPoint(new Vector3(center.x + c.size.x / 2, center.y, center.z - (c.size.z / 2)));

        Vector3 top = plane.transform.TransformPoint(center + transform.up * (c.size.y / 2));


        if (left.x < xmin || middle.x < xmin || right.x < xmin)
        {
            plane.transform.position += Vector3.right * (Mathf.Max(xmin - left.x, Mathf.Max(xmin - middle.x, xmin - right.x)) + shift);
            rb.velocity = Vector3.zero;
        }
        else if(left.x > xmax || middle.x > xmax || right.x > xmax)
        {
            plane.transform.position += Vector3.left * ( Mathf.Max(left.x - xmax, Mathf.Max(middle.x - xmax, right.x - xmax)) + shift);
            rb.velocity = Vector3.zero;
        }


        if (left.z < zmin || middle.z < zmin || right.z < zmin)
        {
            plane.transform.position += Vector3.forward * (Mathf.Max(zmin - left.z, Mathf.Max(zmin - middle.z, zmin - right.z)) + shift);
            rb.velocity = Vector3.zero;
        }
    
        else if (left.z > zmax || middle.z > zmax || right.z > zmax)
        {
            plane.transform.position += Vector3.back * (Mathf.Max(left.z - zmax, Mathf.Max(middle.z - zmax, right.z - zmax)) +  shift);
            rb.velocity = Vector3.zero;
        }


        if (bleft.x < xmin || bmiddle.x < xmin || bright.x < xmin)
        {
            plane.transform.position += Vector3.right * (Mathf.Max(xmin - bleft.x, Mathf.Max(xmin - bmiddle.x, xmin - bright.x)) + shift);
            rb.velocity = Vector3.zero;
        }
        else if (bleft.x > xmax || bmiddle.x > xmax || bright.x > xmax)
        {
            plane.transform.position += Vector3.left * (Mathf.Max(bleft.x - xmax, Mathf.Max(bmiddle.x - xmax, bright.x - xmax)) + shift);
            rb.velocity = Vector3.zero;
        }


        if (bleft.z < zmin || bmiddle.z < zmin || bright.z < zmin)
        {
            plane.transform.position += Vector3.forward * (Mathf.Max(zmin - bleft.z, Mathf.Max(zmin - bmiddle.z, zmin - bright.z)) + shift);
            rb.velocity = Vector3.zero;
        }

        else if (bleft.z > zmax || bmiddle.z > zmax || bright.z > zmax)
        {
            plane.transform.position += Vector3.back * (Mathf.Max(bleft.z - zmax, Mathf.Max(bmiddle.z - zmax, bright.z - zmax)) + shift);
            rb.velocity = Vector3.zero;
        }

        if(top.y > ymax)
        {
            plane.transform.position += Vector3.down * ( top.y - ymax + shift);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        }

    }


}
