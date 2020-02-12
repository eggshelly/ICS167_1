﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneLanding : MonoBehaviour
{
    [SerializeField] GameObject plane;
    [SerializeField] GameObject WinPanel;
    [SerializeField] GameObject LosePanel;
    [SerializeField] float maxSpeedForLanding;

    Rigidbody m_planerb;

    bool landingGearDeployed = false;

    // Start is called before the first frame update
    void Start()
    {
        m_planerb = plane.GetComponent<Rigidbody>();
    }


    public void DeployLandingGear()
    {
        landingGearDeployed = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            m_planerb.constraints = RigidbodyConstraints.FreezeAll;
            if (m_planerb.velocity.magnitude < maxSpeedForLanding && landingGearDeployed)
            {
                WinPanel.SetActive(true);
            }
            else
            {
                LosePanel.SetActive(true);
            }
        }
    }
}
