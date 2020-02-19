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


    GameObject armTarget;
    NetworkArmMovement arm;

   

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

            armTarget.transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * speed);



            //Clamp arm targets to current camera space
            float cameraOffset = armTarget.transform.position.z - Camera.main.transform.position.z;

            float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraOffset)).x;
            float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, cameraOffset)).x;
            float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraOffset)).y;
            float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, cameraOffset)).y;

            armTarget.transform.position = new Vector3( //restrict arm target to camera border bounds
                Mathf.Clamp(armTarget.transform.position.x, leftBorder, rightBorder),
                Mathf.Clamp(armTarget.transform.position.y, topBorder, bottomBorder),
                armTarget.transform.position.z
            );

            CmdUpdateArmLocation(armTarget, armTarget.transform.position);
        }
    }

    [Command]
    public void CmdUpdateArmLocation(GameObject aT, Vector3 pos)
    {
        if(this.armTarget == aT)
        {
            this.armTarget.transform.position = pos;
            RpcUpdateArmLocation(aT, pos);
        }
    }

    [ClientRpc]
    public void RpcUpdateArmLocation(GameObject aT, Vector3 pos)
    {
        if (this.armTarget == aT)
        {
            this.armTarget.transform.position = pos;
        }
    }

    #endregion

    #region Input


    void GetInput()
    {
        if (arm != null && hasAuthority)
        {
            if (Input.GetKeyDown(inputKey))
            {
                if (arm.CheckInput())
                {
                    arm.PressButton();
                    CmdCheckInput();
                    PressButton();
                    Debug.Log("Arm is over button");
                }
            }
            else if(Input.GetKeyUp(inputKey))
            {
                arm.PressButton();
                arm.NoInput();
                CmdNoInput();
                PressButton();
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

    public void PressButton()
    {
        GameObject button = arm.GetButton();
        if(button != null)
        {
            button.GetComponent<ButtonHandler>().ButtonUpdate();
            CmdPressButton(button);
        }
    }

    [Command]
    public void CmdPressButton(GameObject button)
    {
        button.GetComponent<ButtonHandler>().ButtonUpdate();
        button.GetComponent<ButtonHandler>().RpcButtonUpdate();
    }

    public void ChangeButtonState(GameObject button, bool isInside)
    {
        if (button != null)
        {
            CmdChangeButtonState(button, isInside);
        }
    }

    [Command]
    public void CmdChangeButtonState(GameObject button, bool isInside)
    {
        button.GetComponent<ButtonHandler>().ChangeState(button, isInside);
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
