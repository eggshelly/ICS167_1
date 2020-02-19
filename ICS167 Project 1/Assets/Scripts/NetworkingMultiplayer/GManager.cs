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



    public delegate void PressButtonDelegate(GameObject button);

    [SyncEvent]
    public static event PressButtonDelegate EventPressButton;

    [SerializeField] GameObject LeftHand;
    [SerializeField] GameObject RightHand;

    [SerializeField] bool leftAssigned = false;

    public int playerNumber = 0;

    private void Awake()
    {
        instance = this;
    }

    public static void InvokeEvent(GameObject button)
    {
        EventPressButton(button);
        Debug.Log("Here");
    }

    

}
