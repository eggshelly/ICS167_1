using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlaneController : MonoBehaviour
{

    public static PlaneController instance;

    [SerializeField] GameObject plane;

    [SerializeField] float speed = 10f;
    [SerializeField] float verticalTorque = 1f;
    [SerializeField] float horizontalTorque = 1f;
    [SerializeField] float maxSpeedForLanding;
    //[SerializeField] float torqueCap = 5f;
    private Rigidbody m_planerb;
    private float currentZvelocity;


    //TEST
    public bool accelerateButton;
    public bool deccelerateButton;
    public bool hLeverRight;
    public bool hLeverLeft;
    public bool vLeverUp;
    public bool vLeverDown;


    public bool landingActivated;
    
    private void Awake()
    {
        currentZvelocity = 0;

        if (instance == null)
            instance = this;
        m_planerb = plane.GetComponent<Rigidbody>();
       
    }
    private void Start()
    {
            
    }

    private void Update()
    {
        print(m_planerb.velocity.z);
        Move();
    }

    private void Move()
    {

        if (accelerateButton)
            accel();
        //else
        if (deccelerateButton)
            deccel();
        //else
        //    m_planerb.velocity = new Vector3(m_planerb.velocity.x, m_planerb.velocity.y, currentZvelocity);

        if (hLeverRight)
            tiltRight();
        if (hLeverLeft)
            tiltLeft();
        if (vLeverUp)
            tiltUp();
        if (vLeverDown)
            tiltDown();
    }

    private void accel()
    {
        //m_planerb.velocity = Vector3.forward * speed;
        m_planerb.AddForce(transform.forward * speed);
        currentZvelocity = m_planerb.velocity.z;
    }

    private void deccel()
    {
        Vector3 deccelVector = transform.forward * -speed;
        if (deccelVector.z < 0)
            deccelVector = new Vector3(deccelVector.x, deccelVector.y, 0);
        m_planerb.AddForce(deccelVector);
        currentZvelocity = m_planerb.velocity.z;

        //Double check
        if (currentZvelocity < 0)
            currentZvelocity = 0;
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
