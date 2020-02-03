using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    accel,
    right,
    left,
    up,
    down
};

public class ActionType
{
    private Action action;
    
    public ActionType(Action a)
    {
        a = action;
    }

    public void Toggle()
    {
        if (action == Action.accel)
            PlaneController.instance.accelerateButton = !PlaneController.instance.accelerateButton;

        else if (action == Action.up)
            PlaneController.instance.vLeverUp = !PlaneController.instance.vLeverUp;

        else if (action == Action.down)
            PlaneController.instance.vLeverDown = !PlaneController.instance.vLeverDown;


    }
};
