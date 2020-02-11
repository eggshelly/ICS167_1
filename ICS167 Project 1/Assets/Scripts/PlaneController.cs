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


    //TEST
    public bool accelerateButton;
    public bool hLeverRight;
    public bool hLeverLeft;
    public bool vLeverUp;
    public bool vLeverDown;


    public bool landingActivated;

    bool landingGearDeployed = false;
    
    private void Awake()
    {
        if (instance == null)
            instance = this;
        m_planerb = plane.GetComponent<Rigidbody>();
       
    }
    private void Start()
    {
            
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (accelerateButton)
            moveForward();
        if (hLeverRight)
            tiltRight();
        if (hLeverLeft)
            tiltLeft();
        if (vLeverUp)
            tiltUp();
        if (vLeverDown)
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

    void DeployLandingGear()
    {
        //do smth else here;
        landingGearDeployed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Road"))
        {
            if(m_planerb.velocity.magnitude < maxSpeedForLanding && landingGearDeployed)
            {
                //probably initiate the clapping sound and display a you win screen
            }
            else
            {
                Debug.Log("You ded lul");
            }
        }
    }
}
