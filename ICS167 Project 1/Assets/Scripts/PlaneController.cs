using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{
    public static PlaneController instance;

    [SerializeField] GameObject plane;

    [SerializeField] float speed = 10f;
    [SerializeField] float speedcap = 20f;
    [SerializeField] float verticalTorque = 1f;
    [SerializeField] float horizontalTorque = 1f;
    [SerializeField] float maxSpeedForLanding;
    //[SerializeField] float torqueCap = 5f;

    private Rigidbody m_planerb;
    public float currentZvelocity;

    public bool buttonPushed = false;
    private Transform startTransform;

    [SerializeField] TextMeshProUGUI speedTxt;
    [SerializeField] TextMeshProUGUI altitudeTxt;


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
        startTransform = m_planerb.transform;

        speedTxt.text = currentZvelocity.ToString();
        altitudeTxt.text = m_planerb.transform.position.y.ToString();
    }

    private void Update()
    {
        if (!buttonPushed)
            StallPlane();
        UpdateText();
        Move();
    }

    private void StallPlane()
    {
        if (m_planerb.transform.position.y <= 4.5f)
            m_planerb.AddForce(startTransform.up * speed * 5);
    }

    private void Move()
    {
        if (accelerateButton)
            accel();
        else if (deccelerateButton)
            deccel();
        else
            m_planerb.velocity = new Vector3(m_planerb.velocity.x, m_planerb.velocity.y, currentZvelocity);
        if (hLeverRight)
            tiltRight();
        if (hLeverLeft)
            tiltLeft();
        if (vLeverUp)
            tiltUp();
        if (vLeverDown)
            tiltDown();
    }

    private void UpdateText()
    {
        speedTxt.text    = currentZvelocity.ToString("0.0");
        altitudeTxt.text = m_planerb.transform.position.y.ToString("0.0");
    }

    private void accel()
    {
        buttonPushed = true;
        Vector3 accelVector = transform.forward * speed;
        if (accelVector.z > speedcap)
            accelVector = new Vector3(accelVector.x, accelVector.y, speedcap);
        
        m_planerb.AddForce(transform.forward * speed);
        currentZvelocity = m_planerb.velocity.z;
    }

    private void deccel()
    {
        buttonPushed = true;
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
        buttonPushed = true;
        m_planerb.AddForce(transform.right * speed);
        plane.transform.Rotate(0, horizontalTorque, 0);
    }

    private void tiltLeft()
    {
        buttonPushed = true;
        m_planerb.AddForce(transform.right * -speed);
        plane.transform.Rotate(0, -horizontalTorque, 0);
    }

    private void tiltUp()
    {
        buttonPushed = true;
        m_planerb.AddForce(transform.up * speed);
        plane.transform.Rotate(-verticalTorque,0,0);
    }

    private void tiltDown()
    {
        buttonPushed = true;
        m_planerb.AddForce(transform.up * -speed);
        plane.transform.Rotate(verticalTorque, 0, 0);
    }

}
