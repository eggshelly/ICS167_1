using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmMovement : MonoBehaviour
{
    [SerializeField] public GameObject leftArmTarget;

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
    }

    void Move()
    {
        float horizontalModifier = Input.GetAxisRaw("Horizontal");
        float verticalModifier = Input.GetAxisRaw("Vertical");

        Debug.Log(horizontalModifier);

        leftTransform.Translate(horizontalModifier * (Time.deltaTime * 20), verticalModifier * (Time.deltaTime * 20), 0);
    }
}
