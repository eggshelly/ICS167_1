using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Hand
{
    Left,
    Right
}

public class GManager : MonoBehaviour
{
    public static GManager instance;

    [SerializeField] GameObject LeftHand;
    [SerializeField] GameObject RightHand;

    int numPlayers = 0;

    private void Awake()
    {
        instance = this; 
    }

    public int GetHand()
    {
        return ++numPlayers;
    }

    public GameObject AssignHand(Hand hand)
    {
        switch(hand)
        {
            case Hand.Left:
                return LeftHand;
            case Hand.Right:
                return RightHand;
            default:
                return null;

        }
    }

}
