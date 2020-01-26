using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerHandMovement : NetworkBehaviour
{
    [SerializeField] NetworkTransformChild hand;
    [SerializeField] float maxVelocity = 20f;

    Rigidbody rb;

    int playerNumber;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    public void SetPlayerNumber(int i)
    {
        playerNumber = i;
    }

    public void AssignHand()
    {
        hand.target = GManager.instance.AssignHand( playerNumber == 0 ? Hand.Left : Hand.Right).transform;
        hand.target.SetParent(this.transform);
        hand.target.transform.localPosition = Vector3.zero;
    }


    private void Update()
    {
        Move();
    }

    void Move()
    {
        rb.AddForce(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0), ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }
}
