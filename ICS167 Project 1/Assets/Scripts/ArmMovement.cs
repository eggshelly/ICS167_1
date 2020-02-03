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
        //Debug.Log("Arm position: (" + armTransform.position.x +", "+ armTransform.position.y +")");
        Move();
        GetInput();
    }

    void Move()
    {
        if (armTransform.position.x >= -.9f && armTransform.position.x <= .9f &&
            armTransform.position.y >= .5f && armTransform.position.y <= 1.5f) //Arm target constraints
        {
            //Debug.Log("True");
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
        }

        //Reset position because of how transform pushes objects past bounds sometimes
        if (armTransform.position.x > .9f)
            armTransform.position = new Vector2(0.8999f, armTransform.position.y);
        if (armTransform.position.x < -.9f)
            armTransform.position = new Vector2(-0.8999f, armTransform.position.y);
        if (armTransform.position.y > 1.5f)
            armTransform.position = new Vector2(armTransform.position.x, 1.4999f);
        if (armTransform.position.y < .5f)
            armTransform.position = new Vector2(armTransform.position.x, 0.5001f);
        
    }

    void GetInput()
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
}
