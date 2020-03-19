using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using Mirror;

public enum ButtonType { button, lever, none };
public enum State { deactivated, canActivate, activated };

public class ButtonHandler : NetworkBehaviour
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
    private AudioSource aud;
    private List<ActionType> ButtonActions;
    private int actionInt;

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

    public ButtonType GetButtonType()
    {
        return type;
    }


    public void ButtonUpdate()
    {
        Debug.Log(this.gameObject.name + "Hey");
        if (state == State.canActivate)
        {
            ButtonActions[0].Toggle();
            state = State.activated;
            sprRend.sprite = canActivateSpriteOn[0];
            aud.PlayOneShot(aud.clip);
        }
        else if (state == State.activated)
        {
            ButtonActions[0].Toggle();
            state = State.canActivate;
            sprRend.sprite = canActivateSpriteOff;
            aud.PlayOneShot(aud.clip);
        }
    }


    [ClientRpc]
    public void RpcButtonUpdate()
    {
        Debug.Log("Coming from RPC");
        ButtonUpdate();
    }

    [ClientRpc]
    public void RpcPullLever(int num)
    {
        LeverPulled(num);
    }


    public int LeverUpdate()
    {
        Debug.Log("Pressing Lever");
        // Up or Left from Neutral State
        if (state == State.canActivate && Input.GetKeyDown(LeverKeys[0]))
        {
            return 0;
        }
        // Down or Right from Neutral State
        else if (state == State.canActivate && Input.GetKeyDown(LeverKeys[1]))
        {

            return 1;
        }
        else if (state == State.activated && Input.GetKeyDown(LeverKeys[1]))
        {

            return 2;
        }
        else if (state == State.activated && Input.GetKeyDown(LeverKeys[0]))
        {

            return 3;
        }
        return -1;
    }

    public void LeverPulled(int num)
    {
        switch(num)
        {
            case 0:
                Debug.Log("Lever Key 0");
                ButtonActions[0].Toggle();
                state = State.activated;
                sprRend.sprite = canActivateSpriteOn[actionInt];
                actionInt = 0;
                break;
            case 1:
                Debug.Log("Lever Key 1");
                ButtonActions[1].Toggle();
                state = State.activated;

                actionInt = 1;

                sprRend.sprite = canActivateSpriteOn[actionInt];
                break;
            case 2:
                if (actionInt == 0)
                {
                    ButtonActions[0].Toggle();
                    state = State.canActivate;

                    sprRend.sprite = canActivateSpriteOff;
                }
                else
                {
                    state = State.canActivate;
                    sprRend.sprite = canActivateSpriteOff;
                }
                break;
            case 3:
                if (actionInt == 1)
                {
                    ButtonActions[1].Toggle();
                    state = State.canActivate;

                    sprRend.sprite = canActivateSpriteOff;
                }
                else
                {
                    state = State.canActivate;
                    sprRend.sprite = canActivateSpriteOff;
                }
                break;

        }
    }


    public void ChangeState(GameObject button, bool isInside)
    {
        Change(button, isInside);
    }

    [ClientRpc]
    public void RpcChangeState(GameObject button, bool isInside)
    {
        Change(button, isInside);
    }

    void Change(GameObject button, bool isInside)
    {
        if (button == this.gameObject)
        {
            if (isInside)
            {
                if (state != State.activated)
                {
                    state = State.canActivate;
                    sprRend.sprite = canActivateSpriteOff;
                }
                else
                {
                    sprRend.sprite = canActivateSpriteOn[actionInt];
                }
            }
            else
            {
                if (state != State.activated)
                {
                    state = State.deactivated;
                    sprRend.sprite = deactivatedSprite;
                }
                else
                {
                    sprRend.sprite = actionSprite[actionInt];
                }

            }
        }
    }
}
