using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmMovement : MonoBehaviour
{
    [SerializeField] public GameObject armTarget;
    [SerializeField] public GameObject handDefault;
    [SerializeField] public GameObject handGrab;
    [SerializeField] public GameObject handPress;
    
    [SerializeField] public int speed = 3;

    private Transform armTransform;
    private KeyCode inputKey;

    Vector2 savedPos = Vector2.zero;
    public bool press;
    public bool grab;

    // Start is called before the first frame update
    void Start()
    {
        armTransform = armTarget.transform;

        if (this.CompareTag("Player1"))
            inputKey = KeyCode.Space;
        else if (this.CompareTag("Player2"))
            inputKey = KeyCode.Period;
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

        //Clamp arm targets to current camera space
        float cameraOffset = armTransform.position.z - Camera.main.transform.position.z;

        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraOffset)).x;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, cameraOffset)).x;
        float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraOffset)).y;
        float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, cameraOffset)).y;

        armTransform.position = new Vector3( //restrict arm target to camera border bounds
            Mathf.Clamp(armTransform.position.x, leftBorder, rightBorder),
            Mathf.Clamp(armTransform.position.y, topBorder, bottomBorder),
            armTransform.position.z
        ); 
    }

    void GetInput()
    {
        if (Input.GetKey(inputKey))
        {
            if (press)
            {
                handDefault.SetActive(false);
                handPress.SetActive(true);
            }
            else if (grab)
            {
                handDefault.SetActive(false);
                handGrab.SetActive(true);
            }
        }
        else
        {
            handPress.SetActive(false);
            handGrab.SetActive(false);
            handDefault.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PressButton"))
            press = true;
        else if (other.gameObject.CompareTag("GrabButton"))
            grab = true;
    }

    private void OnTriggerExit(Collider other)
    {
        press = false;
        grab = false;
    }
}
