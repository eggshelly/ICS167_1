using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneLanding : MonoBehaviour
{
    [SerializeField] GameObject plane;
    [SerializeField] GameObject EndPanel;
    [SerializeField] float maxSpeedForLanding;

    Rigidbody m_planerb;

    bool landingGearDeployed = false;

    // Start is called before the first frame update
    void Start()
    {
        m_planerb = plane.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DeployLandingGear()
    {
        //do smth else here;
        landingGearDeployed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            if (m_planerb.velocity.magnitude < maxSpeedForLanding && landingGearDeployed)
            {
                m_planerb.constraints = RigidbodyConstraints.FreezeAll;
            }
            else
            {
                Debug.Log("You ded lul");
            }
        }
    }
}
