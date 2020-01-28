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
                Debug.Log("Should be assigning the left hand");
                player.AssignHand(this.gameObject, leftHand);
            }
            else if(!leftHand && playerNumber == 1)
            {
                Debug.Log("Here!");
                player.AssignHand(this.gameObject, leftHand);
            }
        }

        this.enabled = (localPlayer != null);
    }

    public bool isLeftHand()
    {
        return leftHand;
    }
}
