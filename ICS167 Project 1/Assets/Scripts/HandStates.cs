using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandStates : MonoBehaviour
{
    [SerializeField] public GameObject handDefault;
    [SerializeField] public GameObject handGrab;
    [SerializeField] public GameObject handPress;

    public bool press = false;
    public bool grab = false;

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (this.CompareTag("Player1") && Input.GetKey(KeyCode.Space))
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
        else if (this.CompareTag("Player2") && Input.GetKey(KeyCode.Period))
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
