using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHandMovement : NetworkBehaviour
{
    [SerializeField] float gravityScale;
    [SerializeField] float moveSpeed;
    [SerializeField] public int playerNumber = -1;


    Rigidbody rb;

    public override void OnStartClient()
    {
        base.OnStartClient();
        playerNumber = GManager.instance.playerNumber++;
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        LocalPlayerAnnouncer.InvokeGetHand(base.netIdentity, this.playerNumber);
    }


    public void AssignNumber(int number)
    {
        playerNumber = number;
    }


    public void AssignPlayerNumber(int number)
    {
        playerNumber = number;   
    }


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void AssignHand(GameObject hand, bool isLeft)
    {
        if (hasAuthority)
        {
            CmdAssignHand(isLeft);
            CmdUpdateHandAttachment(this.gameObject, hand);
        }   
    }


    private void Update()
    {
        if (base.hasAuthority)
        {
            Move();
        }
    }

    void Move()
    {
        if(base.hasAuthority)
        {
            transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed + Vector3.down * gravityScale);
        }
    }

    void AttachHand(GameObject hand)
    {
        hand.transform.SetParent(this.transform);
        this.gameObject.GetComponent<NetworkTransformChild>().target = hand.transform;
        hand.transform.localPosition = Vector3.zero;
    }



    [Command]
    public void CmdUpdateHandAttachment(GameObject player, GameObject hand)
    {
        AttachHand(hand);
        RpcUpdateHandAttachment(player, hand);
    }

    [ClientRpc]
    void RpcUpdateHandAttachment(GameObject player, GameObject hand)
    {
        AttachHand(hand);
    }


    [Command]
    public void CmdAssignHand(bool leftHand)
    {
        if (leftHand)
        {
            GManager.instance.UpdateHandAssignment();
            RpcUpdateAssigned();
        }
    }


    [ClientRpc]
    void RpcUpdateAssigned()
    {
        GManager.instance.UpdateHandAssignment();
    }

}
