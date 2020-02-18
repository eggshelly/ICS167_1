using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkArmMovement : NetworkBehaviour
{
    [SerializeField] public GameObject armTarget;
    [SerializeField] public GameObject handDefault;
    [SerializeField] public GameObject handGrab;
    [SerializeField] public GameObject handPress;

    PlayerHandMovement networkHand;

    private KeyCode inputKey = KeyCode.Space;

    public bool press;
    public bool grab;




    public void SetNetworkHand(PlayerHandMovement p)
    {
        networkHand = p;
    }

    public GameObject GetArmTarget()
    {
        return armTarget;
    }

    public void CheckInput()
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

    public void NoInput()
    {
        handPress.SetActive(false);
        handGrab.SetActive(false);
        handDefault.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("PressButton"))
        {
            press = true;
        }
        else if (other.gameObject.CompareTag("GrabButton"))
        {
            grab = true;
        }
        UpdateBools();
    }

    void UpdateBools()
    {
        if (networkHand != null)
        {
            networkHand.Pressed(press);
            networkHand.Grabbed(grab);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        press = false;
        grab = false;
        UpdateBools();
    }

    public void Pressed(bool p)
    {
        RpcPressed(p);
    }

    public void Grabbed(bool g)
    {
        RpcGrabbed(g);
    }

    [ClientRpc]
    public void RpcPressed(bool p)
    {
        this.press = p;
    }

    [ClientRpc]
    public void RpcGrabbed(bool g)
    {
        this.grab = g;
    }
}
