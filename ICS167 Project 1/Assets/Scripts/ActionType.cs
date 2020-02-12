using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Action
{
    accel,
    deccel,
    right,
    left,
    up,
    down,
    land
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

        else if (action == Action.deccel)
            PlaneController.instance.deccelerateButton = !PlaneController.instance.deccelerateButton;

        else if (action == Action.up)
            PlaneController.instance.vLeverUp = !PlaneController.instance.vLeverUp;

        else if (action == Action.down)
            PlaneController.instance.vLeverDown = !PlaneController.instance.vLeverDown;

        else if (action == Action.right)
            PlaneController.instance.hLeverRight = !PlaneController.instance.hLeverRight;

        else if (action == Action.left)
            PlaneController.instance.hLeverLeft = !PlaneController.instance.hLeverLeft;

        else if (action == Action.land)
            PlaneController.instance.landingActivated = !PlaneController.instance.landingActivated;

    }
};
