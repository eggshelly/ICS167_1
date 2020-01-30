using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMovement : MonoBehaviour
{
    [SerializeField] public GameObject armTarget;
    [SerializeField] public GameObject handDefault;
    [SerializeField] public GameObject handGrab;
    [SerializeField] public GameObject handPress;
    
    [SerializeField] public int speed = 20;

    private Transform armTransform;

    // Start is called before the first frame update
    void Start()
    {
        armTransform = armTarget.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GetInput();
    }

    void Move()
    {
        //float horizontalModifier = Input.GetAxisRaw("Horizontal");
        //float verticalModifier = Input.GetAxisRaw("Vertical");

        if (this.CompareTag("Player1"))
        {
            if (Input.GetKey(KeyCode.A))
                armTransform.Translate(-1 * (Time.deltaTime * speed), 0, 0);
            if (Input.GetKey(KeyCode.D))
                armTransform.Translate(1 * (Time.deltaTime * speed), 0, 0);
            if (Input.GetKey(KeyCode.S))
                armTransform.Translate(0, -1 * (Time.deltaTime * speed), 0);
            if (Input.GetKey(KeyCode.W))
                armTransform.Translate(0, 1 * (Time.deltaTime * speed), 0);
        }
        else if (this.CompareTag("Player2"))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                armTransform.Translate(-1 * (Time.deltaTime * speed), 0, 0);
            if (Input.GetKey(KeyCode.RightArrow))
                armTransform.Translate(1 * (Time.deltaTime * speed), 0, 0);
            if (Input.GetKey(KeyCode.DownArrow))
                armTransform.Translate(0, -1 * (Time.deltaTime * speed), 0);
            if (Input.GetKey(KeyCode.UpArrow))
                armTransform.Translate(0, 1 * (Time.deltaTime * speed), 0);
        }
        
        //armTransform.Translate(horizontalModifier * (Time.deltaTime * speed), verticalModifier * (Time.deltaTime * speed), 0);
    }

    void GetInput() //testing method, and change arm states dependent on type of button pressed(?)
    {
        if (Input.GetKey(KeyCode.Z))
        {
            handDefault.SetActive(false);
            handPress.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            handDefault.SetActive(false);
            handGrab.SetActive(true);
        }
        else
        {
            handPress.SetActive(false);
            handGrab.SetActive(false);
            handDefault.SetActive(true);
        }
    }

    void Interact()
    {

    }
}
