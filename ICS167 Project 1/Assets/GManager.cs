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

    [SerializeField] GameObject LeftHand;
    [SerializeField] GameObject RightHand;

    [SerializeField] bool leftAssigned = false;

    public int playerNumber = 0;

    private void Awake()
    {
        instance = this;
    }




    public bool isLeftAssigned()
    {
        return leftAssigned;
    }

    public void UpdateHandAssignment()
    {
        leftAssigned = true;
    }

}
