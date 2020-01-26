using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmMovement : MonoBehaviour
{
    [SerializeField] public GameObject leftArmTarget;
    [SerializeField] public GameObject leftDefault;
    [SerializeField] public GameObject leftPress;
    [SerializeField] public GameObject leftGrab;

    private Transform leftTransform;
    private int speed = 5;

    // Start is called before the first frame update
    void Start()
    {
        leftTransform = leftArmTarget.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        GetInput();
    }

    void Move()
    {
        float horizontalModifier = Input.GetAxisRaw("Horizontal");
        float verticalModifier = Input.GetAxisRaw("Vertical");

        //Debug.Log(horizontalModifier);

        leftTransform.Translate(horizontalModifier * (Time.deltaTime * 20), verticalModifier * (Time.deltaTime * 20), 0);
    }

    void GetInput()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            leftDefault.SetActive(false);
            leftPress.SetActive(true);
        }
        else if (Input.GetKey(KeyCode.X))
        {
            leftDefault.SetActive(false);
            leftGrab.SetActive(true);
        }
        else
        {
            leftPress.SetActive(false);
            leftGrab.SetActive(false);
            leftDefault.SetActive(true);
        }
    }
}
