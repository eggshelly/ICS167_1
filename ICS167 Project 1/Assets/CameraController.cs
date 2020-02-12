using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float offset;

    void Awake()
    {
        
    }

    void Update()
    {
        transform.position = new Vector3(target.position.x, target.position.y, target.position.z + offset);
    }
}
