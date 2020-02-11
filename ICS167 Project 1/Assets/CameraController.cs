using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;

    void Awake()
    {
        
    }

    void Update()
    {
        transform.position = target.position;
    }
}
