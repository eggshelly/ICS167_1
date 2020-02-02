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
        LocalPlayerAnnouncer.InvokeGetHand(netIdentity, this.playerNumber);
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
        if (hasAuthority)
        {
            Move();
        }
    }

    void Move()
    {
        if(hasAuthority)
        {
            transform.Translate(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0) * moveSpeed + Vector3.down * gravityScale);
        }
    }

    void AttachHand(GameObject player, GameObject hand)
    {
        hand.transform.SetParent(player.transform);
        player.gameObject.GetComponent<NetworkTransformChild>().target = hand.transform;
        hand.transform.localPosition = Vector3.zero;
    }



    [Command]
    public void CmdUpdateHandAttachment(GameObject player, GameObject hand)
    {
        AttachHand(player, hand);
        RpcUpdateHandAttachment(player, hand);
    }

    [ClientRpc]
    void RpcUpdateHandAttachment(GameObject player, GameObject hand)
    {
        Debug.Log(hand.name + " is attached to " + this.gameObject.name);
        AttachHand(player, hand);
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
