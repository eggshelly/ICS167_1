using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHandMovement : NetworkBehaviour
{
    [SerializeField] float gravityScale;
    [SerializeField] float speed;
    [SerializeField] public int playerNumber = -1;

    [SyncVar(hook = nameof(SyncHand))]
    GameObject hand = null;


    NetworkHandTarget armTarget;
    NetworkArmMovement arm;

    bool pressing;
   

    KeyCode inputKey = KeyCode.Space;

    Rigidbody rb;


    public override void OnStartClient()
    {
        base.OnStartClient();
        playerNumber = GManager.instance.playerNumber++;
        LocalPlayerAnnouncer.InvokeGetHand(netIdentity, this.playerNumber);
    }

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }


    private void Update()
    {
        if (hasAuthority)
        {
            Move();
            GetInput();
        }
    }

    #region Movement

    void Move()
    {
        if (hasAuthority && armTarget != null)
        {
            float vert = Input.GetAxisRaw("Vertical");
            float hor = Input.GetAxisRaw("Horizontal");
            armTarget.Move(vert, hor, speed);

            CmdUpdateArmLocation(armTarget.gameObject, vert, hor, speed);
        }
    }

    [Command]
    public void CmdUpdateArmLocation(GameObject aT, float vert, float hor, float speed)
    {
        if(this.armTarget.gameObject == aT)
        {
            this.armTarget.Move(vert, hor, speed);
            RpcUpdateArmLocation(aT, vert, hor, speed);
        }
    }

    [ClientRpc]
    public void RpcUpdateArmLocation(GameObject aT, float vert, float hor, float speed)
    {
        this.armTarget.Move(vert, hor, speed);
    }

    #endregion

    #region Input


    void GetInput()
    {

        if (arm != null && hasAuthority)
        {
            ButtonType t = arm.GetButtonType();
            if (t == ButtonType.button && Input.GetKeyDown(inputKey))
            {
                if (arm.CheckInput())
                { 
                    arm.PressButton();
                    CmdCheckInput();
                    CmdPressButton(0, -1);
                    Debug.Log("Arm is over button");
                }
            }
            else if(t == ButtonType.lever && Input.GetKey(inputKey))
            { 
                if (arm.CheckInput())
                {
                    CmdCheckInput();
                    int num;
                    if ((num = arm.CheckLeverPull()) >= 0)
                    {
                        arm.PullLever(num);
                        CmdPressButton(1, num);
                    }
                }
            }
            else if(Input.GetKeyUp(inputKey))
            {
                arm.NoInput();
                CmdNoInput();
            }
        }
    }

    [Command]
    public void CmdCheckInput()
    {
        this.arm.CheckInput();
        this.arm.RpcCheckInput();
    }

    [Command]
    public void CmdNoInput()
    {
        this.arm.NoInput();
        this.arm.RpcNoInput();
    }


    [Command]
    public void CmdPressButton(int type, int num)
    {
        if (type == 0)
        {
            arm.PressButton();
            arm.RpcPressButton();
        }
        else if(type == 1)
        {
            arm.PullLever(num);
            arm.RpcPullLever(num);
        }
    }

    public void ChangeButtonState(GameObject button, bool isInside)
    {
        if (button != null)
        {
            button.GetComponent<ButtonHandler>().ChangeState(button, isInside);
            arm.AttachButton(isInside ? button : null);
            CmdChangeButtonState(button, isInside);
        }
    }

    [Command]
    public void CmdChangeButtonState(GameObject button, bool isInside)
    {
        ButtonHandler b = button.GetComponent<ButtonHandler>();
        b.ChangeState(button, isInside);
        b.RpcChangeState(button, isInside);
        arm.AttachButton(isInside ? button : null);
        arm.RpcAttachButton(isInside ? button : null);
    }


    public void UpdateBools(GameObject arm, bool p, bool g)
    {
        CmdUpdateBools(arm, p, g);
    }

    [Command]
    public void CmdUpdateBools(GameObject arm, bool p, bool g)
    {
        arm.GetComponent<NetworkArmMovement>().SetBools(arm, p, g);
    }

    #endregion


    #region Attaching/Syncing hand object

    public void AssignHand(GameObject hand)
    {
        if (hasAuthority && this.hand != hand)
        {
            this.hand = hand;
            armTarget = hand.GetComponent<NetworkArmMovement>().GetArmTarget();
            arm = hand.GetComponent<NetworkArmMovement>();
            AttachHand(this.hand);
            hand.GetComponent<NetworkArmMovement>().SetNetworkHand(this);
        }
    }

    void SyncHand(GameObject oldHand, GameObject newHand)
    {
        AttachHand(newHand);
    }


    public void AttachHand(GameObject hand)
    {
        if (hand != null)
        {
            this.armTarget = hand.GetComponent<NetworkArmMovement>().GetArmTarget();
            this.arm = hand.GetComponent<NetworkArmMovement>();
            if (hasAuthority)
                CmdUpdateHandAttachment(hand);
        }
        else
        {
            Debug.Log("Oopsies");
        }
    }
    [Command]
    public void CmdUpdateHandAttachment(GameObject hand)
    {
        this.armTarget = hand.GetComponent<NetworkArmMovement>().GetArmTarget();
        this.arm = hand.GetComponent<NetworkArmMovement>();
        hand.GetComponent<HandScript>().AttachToPlayer(this.gameObject);
    }
    #endregion
}

