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
                player.AssignHand(this.gameObject);
            }
            else if(!leftHand && playerNumber == 1)
            {
                player.AssignHand(this.gameObject);
            }
        }

        this.enabled = (localPlayer != null);
    }

    public void AttachToPlayer(GameObject player)
    {
        RpcAttachToPlayer(player);
    }

    [ClientRpc]
    void RpcAttachToPlayer(GameObject player)
    {
        player.GetComponent<PlayerHandMovement>().AssignHand(this.gameObject);
    }

}
