using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using Mirror;

public class LocalPlayerAnnouncer : NetworkBehaviour
{
    public delegate void GetHand(NetworkIdentity localPlayer, int playerNumber);
    public static event GetHand GetAssignedHand;

    void Start()
    {
        //base.OnStartLocalPlayer();
        //OnLocalPlayerUpdated?.Invoke(base.netIdentity);
    }

    public static void InvokeGetHand(NetworkIdentity localPlayer, int playerNumber)
    {
        GetAssignedHand.Invoke(localPlayer, playerNumber);
    }



}
