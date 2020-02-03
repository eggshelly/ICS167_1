using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHandMovement : NetworkBehaviour
{
    [SerializeField] float gravityScale;
    [SerializeField] float moveSpeed;
    [SerializeField] public int playerNumber = -1;

    [SyncVar(hook = nameof(AttachHand))]
    GameObject hand;

    Rigidbody rb;

    public override void OnStartClient()
    {
        base.OnStartClient();
        playerNumber = GManager.instance.playerNumber++;
        LocalPlayerAnnouncer.InvokeGetHand(netIdentity, this.playerNumber);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
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
            this.hand = hand;
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

    public void AttachHand(GameObject hand)
    {
        hand.transform.SetParent(this.transform);
        hand.transform.localPosition = Vector3.zero;
        CmdUpdateHandAttachment(this.gameObject, hand);
    }



    [Command]
    public void CmdUpdateHandAttachment(GameObject player, GameObject hand)
    {
        this.hand = hand;
        hand.transform.SetParent(player.transform);
        hand.transform.localPosition = Vector3.zero;
        hand.GetComponent<HandScript>().AttachToPlayer(player);
    }
}
