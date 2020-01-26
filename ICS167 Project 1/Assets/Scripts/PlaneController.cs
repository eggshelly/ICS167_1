using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [SerializeField] GameObject plane;
    [SerializeField] GameObject horizontalLever;
    [SerializeField] GameObject verticalLever;

    [SerializeField] float speed = 10f;
    [SerializeField] float verticalTorque = 1f;
    [SerializeField] float horizontalTorque = 1f;
    //[SerializeField] float torqueCap = 5f;
    private Rigidbody m_planerb; 
    

    //TEST
    public bool isForward;
    public bool isRight;
    public bool isLeft;
    public bool isUp;
    public bool isDown;
    
    private void Awake()
    {
        m_planerb = plane.GetComponent<Rigidbody>();
    }
    private void Start()
    {
        
    }

    private void Update()
    {
        Test();

    }

    private void Test()
    {
        if (isForward)
            moveForward();
        if (isRight)
            tiltRight();
        if (isLeft)
            tiltLeft();
        if (isUp)
            tiltUp();
        if (isDown)
            tiltDown();
    }

    private void moveForward()
    {
        //m_planerb.velocity = Vector3.forward * speed;
        m_planerb.AddForce(transform.forward * speed);
    }

    private void tiltRight()
    {
        m_planerb.AddForce(transform.right * speed);
        plane.transform.Rotate(0, horizontalTorque, 0);
    }

    private void tiltLeft()
    {
        m_planerb.AddForce(transform.right * -speed);
        plane.transform.Rotate(0, -horizontalTorque, 0);
    }

    private void tiltUp()
    {
        m_planerb.AddForce(transform.up * speed);
        plane.transform.Rotate(-verticalTorque,0,0);
    }

    private void tiltDown()
    {
        m_planerb.AddForce(transform.up * -speed);
        plane.transform.Rotate(verticalTorque, 0, 0);
    }
}
