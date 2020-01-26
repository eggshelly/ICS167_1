using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoarding : MonoBehaviour
{

    [SerializeField] bool isBillBoard = false;
    private Camera m_Camera;
 

    private void Awake()
    {
        m_Camera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        if (isBillBoard)
        {
            Vector3 temp = new Vector3(m_Camera.transform.position.x, -90, m_Camera.transform.position.z);
            transform.LookAt(temp);
        }

    }
}
