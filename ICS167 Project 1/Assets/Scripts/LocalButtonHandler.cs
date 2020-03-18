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
    [SerializeField] List<Sprite> actionSprite = new List<Sprite>();
    [SerializeField] List<Sprite> canActivateSpriteOn = new List<Sprite>();
    [SerializeField] Sprite canActivateSpriteOff, deactivatedSprite;

    [SerializeField] KeyCode InteractButton;

    [SerializeField] List<KeyCode> LeverKeys = new List<KeyCode>();


    private SpriteRenderer sprRend;
    private Sprite currentSprite;
    private AudioSource aud;
    private List<ActionType> ButtonActions;
    private int actionInt;

    void Awake()
    {
        ButtonActions = new List<ActionType>(actions.Count);
        state = State.deactivated;
        actionInt = 0;

        currentSprite = deactivatedSprite;

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
        sprRend.sprite = currentSprite;
        if (type == ButtonType.button && Input.GetKeyDown(InteractButton))
        {
            ButtonUpdate();
        }
        else if (type == ButtonType.lever && Input.GetKey(InteractButton))
        {
            LeverUpdate();
        }
    }

    void ButtonUpdate()
    {
        if (state == State.canActivate)
        {
            ButtonActions[0].Toggle();
            state = State.activated;
            currentSprite = canActivateSpriteOn[0];
            aud.PlayOneShot(aud.clip);
        }
        else if (state == State.activated)
        {
            ButtonActions[0].Toggle();
            state = State.canActivate;
            currentSprite = canActivateSpriteOff;
            aud.PlayOneShot(aud.clip);
        }
    }

    void LeverUpdate()
    {
        // Up or Left from Neutral State
        if (state == State.canActivate && Input.GetKeyDown(LeverKeys[0]))
        {
            ButtonActions[0].Toggle();
            state = State.activated;
            currentSprite = canActivateSpriteOn[actionInt];
            actionInt = 0;

        }
        // Down or Right from Neutral State
        else if (state == State.canActivate && Input.GetKeyDown(LeverKeys[1]))
        {
            ButtonActions[1].Toggle();
            state = State.activated;

            actionInt = 1;

            currentSprite = canActivateSpriteOn[actionInt];
        }
        else if (state == State.activated && Input.GetKeyDown(LeverKeys[1]))
        {
            if (actionInt == 0)
            {
                ButtonActions[0].Toggle();
                state = State.canActivate;

                currentSprite = canActivateSpriteOff;
            }
            else
            {
                state = State.canActivate;
                currentSprite = canActivateSpriteOff;
            }
        }
        else if (state == State.activated && Input.GetKeyDown(LeverKeys[0]))
        {
            print(actionInt);
            if (actionInt == 1)
            {
                ButtonActions[1].Toggle();
                state = State.canActivate;

                currentSprite = canActivateSpriteOff;
            }
            else
            {
                state = State.canActivate;
                currentSprite = canActivateSpriteOff;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (state != State.activated)
            {
                state = State.canActivate;
                currentSprite = canActivateSpriteOff;
            }
            else
            {
                currentSprite = canActivateSpriteOn[actionInt];
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player1") || other.gameObject.CompareTag("Player2"))
        {
            if (state != State.activated)
            {
                state = State.deactivated;
                currentSprite = deactivatedSprite;
            }
            else
            {
                currentSprite = actionSprite[actionInt];
            }
        }
    }
}