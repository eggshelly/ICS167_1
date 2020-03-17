using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkHandTarget : NetworkBehaviour
{
    
    public void Move(float vert, float hor, float speed)
    {
        if(vert != 0 || hor != 0)
        {
            this.transform.Translate(new Vector3(hor, vert) * speed);
            ClampPos();
        }
    }



    void ClampPos()
    {
        //Clamp arm targets to current camera space
        float cameraOffset = this.transform.position.z - Camera.main.transform.position.z;

        float leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraOffset)).x;
        float rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, cameraOffset)).x;
        float topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, cameraOffset)).y;
        float bottomBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, cameraOffset)).y;

        this.transform.position = new Vector3( //restrict arm target to camera border bounds
            Mathf.Clamp(this.transform.position.x, leftBorder, rightBorder),
            Mathf.Clamp(this.transform.position.y, topBorder, bottomBorder),
            this.transform.position.z
        );
    }
}
