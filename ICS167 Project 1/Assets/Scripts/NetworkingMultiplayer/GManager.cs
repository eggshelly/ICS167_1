using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public enum Hand
{
    Left,
    Right
}

public class GManager : NetworkBehaviour
{
    public static GManager instance = null;

    public int playerNumber = 0;

    private void Awake()
    {
        instance = this;
    }
    

}
