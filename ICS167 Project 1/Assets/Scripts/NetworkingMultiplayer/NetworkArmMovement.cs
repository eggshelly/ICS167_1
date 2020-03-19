using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class NetworkArmMovement : NetworkBehaviour
{
    [SerializeField] public NetworkHandTarget armTarget;
    [SerializeField] public GameObject handDefault;
    [SerializeField] public GameObject handGrab;
    [SerializeField] public GameObject handPress;

    PlayerHandMovement networkHand;

    private KeyCode inputKey = KeyCode.Space;

    public bool press;
    public bool grab;

    GameObject button;


    public void SetNetworkHand(PlayerHandMovement p)
    {
        networkHand = p;
    }

    public NetworkHandTarget GetArmTarget()
    {
        return armTarget;
    }

    public ButtonType GetButtonType()
    {
        return (button == null ? ButtonType.none : button.GetComponent<ButtonHandler>().GetButtonType());   
    }

    public bool CheckInput()
    {
        if (press)
        {
            handDefault.SetActive(false);
            handPress.SetActive(true);
            return true;
        }
        else if (grab)
        {
            handDefault.SetActive(false);
            handGrab.SetActive(true);
            return true;
        }
        return false;
    }

    public void NoInput()
    {
        handPress.SetActive(false);
        handGrab.SetActive(false);
        handDefault.SetActive(true);
    }

    private void OnTriggerStay(Collider other)
    {
        if(networkHand != null)
        {
            if (other.gameObject.CompareTag("PressButton"))
            {
                press = true;
                button = other.gameObject;
                networkHand.ChangeButtonState(button, true);
            }
            else if (other.gameObject.CompareTag("GrabButton"))
            {
                grab = true;
                button = other.gameObject;
                networkHand.ChangeButtonState(button, true);
            }
            UpdateBools();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(networkHand != null)
        {
            Debug.Log("Leaving Button");
            press = false;
            grab = false;
            UpdateBools();
            networkHand.ChangeButtonState(button, false);
        }
    }

    public void AttachButton(GameObject button)
    {
        this.button = button;
    }

    [ClientRpc]
    public void RpcAttachButton(GameObject button)
    {
        this.button = button;
    }


    void UpdateBools()
    {
        if (networkHand != null)
        {
            networkHand.UpdateBools(this.gameObject, press, grab);
        }
    }

    public void SetBools(GameObject arm, bool p, bool g)
    {
        if(arm == this.gameObject)
        {
            press = p;
            grab = g;
            RpcSetBools(arm, p, g);
        }
    }

    [ClientRpc]
    public void RpcCheckInput()
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


    [ClientRpc]
    public void RpcNoInput()
    {
        handPress.SetActive(false);
        handGrab.SetActive(false);
        handDefault.SetActive(true);
    }

    [ClientRpc]
    public void RpcSetBools(GameObject arm, bool p, bool g)
    {
        if (arm == this.gameObject)
        {
            press = p;
            grab = g;
        }
    }

    public GameObject GetButton()
    {
        return button;
    }

    public void PressButton()
    {
        Debug.Log(this.gameObject.name);
        button.GetComponent<ButtonHandler>().ButtonUpdate();
    }
    
    public int CheckLeverPull()
    {
        return button.GetComponent<ButtonHandler>().LeverUpdate();
    }

    public void PullLever(int num)
    {
        Debug.Log(this.gameObject.name);
        button.GetComponent<ButtonHandler>().LeverPulled(num);
    }

    [ClientRpc]
    public void RpcPressButton()
    {
        button.GetComponent<ButtonHandler>().ButtonUpdate();
    }

    [ClientRpc]
    public void RpcPullLever(int num)
    {
        Debug.Log(this.gameObject.name);
        button.GetComponent<ButtonHandler>().LeverPulled(num);
    }
}
