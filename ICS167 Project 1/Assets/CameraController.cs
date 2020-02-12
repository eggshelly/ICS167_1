using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float yOffset;
    [SerializeField] float zOffset;


    void Awake()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y - yOffset, target.position.z + zOffset);
    }
}
