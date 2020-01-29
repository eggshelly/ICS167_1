using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public enum ButtonType { button, lever };
public enum State { deactivated, canActivate, activated };

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] ButtonType type;
    [SerializeField] State state;

    [SerializeField] KeyCode PressButton;
    [SerializeField] KeyCode LeverButton;

    void Awake()
    {
        state = State.deactivated;
    }

    void Update()
    {
        if (type == ButtonType.button && Input.GetKeyDown(PressButton))
        {
            if (state == State.canActivate)
            {
                PlaneController.instance.accelerateButton = true;
                state = State.activated;
            }
            else if (state == State.activated)
            {
                PlaneController.instance.accelerateButton = false;
                state = State.canActivate;
            }
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            state = State.canActivate;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hand"))
        {
            state = State.deactivated;
        }
    }
}
