using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class HandScript : NetworkBehaviour
{
    [SerializeField] bool leftHand;

    private void Awake()
    {
        LocalPlayerAnnouncer.GetAssignedHand += PlayerUpdated;
    }

    private void OnDestroy()
    { 
        LocalPlayerAnnouncer.GetAssignedHand -= PlayerUpdated;
    }

    void PlayerUpdated(NetworkIdentity localPlayer, int playerNumber)
    {
        if (localPlayer != null)
        {
            PlayerHandMovement player = localPlayer.GetComponent<PlayerHandMovement>();
            if (leftHand && playerNumber == 0)
            {
                player.AssignHand(this.gameObject, leftHand);
            }
            else if(!leftHand && playerNumber == 1)
            {
                player.AssignHand(this.gameObject, leftHand);
            }
        }

        this.enabled = (localPlayer != null);
    }

    public void AttachToPlayer(GameObject player)
    {
        Debug.Log(this.name + " is attached");
        this.transform.SetParent(player.transform);
        this.transform.localPosition = Vector3.zero;
        RpcAttachToPlayer(player, this.gameObject);
    }

    [ClientRpc]
    void RpcAttachToPlayer(GameObject player, GameObject hand)
    {
        Debug.Log(hand.name + " has received the command.");
        player.GetComponent<PlayerHandMovement>().AttachHand(hand);
    }


    public bool isLeftHand()
    {
        return leftHand;
    }
}
