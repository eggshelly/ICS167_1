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
        if (arm != null)
        {
            if (Input.GetKey(inputKey) && hasAuthority)
            {
                arm.CheckInput();
                CmdCheckInput();
            }
            else
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
    }

    [Command]
    public void CmdNoInput()
    {
        this.arm.NoInput();
    }

    public void Pressed(bool p)
    {
        CmdPressed(p);
    }

    [Command]
    public void CmdPressed(bool p)
    {
        arm.Pressed(p);
    }


    public void Grabbed(bool g)
    {
        CmdGrabbed(g);
    }

    [Command]
    public void CmdGrabbed(bool g)
    {
        arm.Grabbed(g);
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
