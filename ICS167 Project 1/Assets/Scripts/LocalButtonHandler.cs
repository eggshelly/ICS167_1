using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

//public enum ButtonType { button, lever };
//public enum State { deactivated, canActivate, activated };

public class LocalButtonHandler : MonoBehaviour
{
    [SerializeField] ButtonType type;
    [SerializeField] State state;
    [SerializeField] List<Action> actions = new List<Action>();

    [SerializeField] KeyCode InteractButton;

    [SerializeField] List<KeyCode> LeverKeys = new List<KeyCode>();

    public Sprite off, on;


    private SpriteRenderer sprRend;
    private AudioSource aud;
    private List<ActionType> ButtonActions;

    void Awake()
    {
        ButtonActions = new List<ActionType>(actions.Count);
        state = State.deactivated;

        sprRend = this.gameObject.GetComponent<SpriteRenderer>();
        aud = this.gameObject.GetComponent<AudioSource>();

        for (int i = 0; i < (actions.Count); i++)
        {
            ActionType temp = new ActionType(actions[i]);
            temp.SetAction(actions[i]);
            ButtonActions.Add(temp);
        }

        print(this.ToString() + "'s action: " + ButtonActions[0].GetAction());
    }

    void Update()
    {

        if (type == ButtonType.button && Input.GetKeyDown(InteractButton))
        {
            ButtonUpdate();
        }
        else if (type == ButtonType.lever && Input.GetKey(InteractButton))
        {
            if (state == State.canActivate && Input.GetKey(LeverKeys[0]))
            {
                ButtonActions[0].Toggle();
                state = State.activated;
            }
            else if (state == State.canActivate && Input.GetKey(LeverKeys[0]))
            {
                ButtonActions[1].Toggle();
                state = State.activated;
            }
            else if (state == State.activated && Input.GetKey(LeverKeys[0]))
            {
                ButtonActions[0].Toggle();
                state = State.canActivate;
            }
            else if (state == State.activated && Input.GetKey(LeverKeys[1]))
            {
                ButtonActions[1].Toggle();
                state = State.canActivate;
            }
        }
    }

    void ButtonUpdate()
    {
        if (state == State.canActivate)
        {
            ButtonActions[0].Toggle();
            state = State.activated;
            sprRend.sprite = on;
            aud.PlayOneShot(aud.clip);
        }
        else if (state == State.activated)
        {
            ButtonActions[0].Toggle();
            state = State.canActivate;
            sprRend.sprite = off;
            aud.PlayOneShot(aud.clip);
        }
    }

    void LeverUpdate()
    {
        if (state == State.canActivate && Input.GetKey(LeverKeys[0]))
        {
            ButtonActions[0].Toggle();
            state = State.activated;
        }
        else if (state == State.canActivate && Input.GetKey(LeverKeys[0]))
        {
            ButtonActions[1].Toggle();
            state = State.activated;
        }
        else if (state == State.activated && Input.GetKey(LeverKeys[0]))
        {
            ButtonActions[0].Toggle();
            state = State.canActivate;
        }
        else if (state == State.activated && Input.GetKey(LeverKeys[1]))
        {
            ButtonActions[1].Toggle();
            state = State.canActivate;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            state = State.canActivate;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (state != State.activated)
                state = State.deactivated;
        }
    }
}