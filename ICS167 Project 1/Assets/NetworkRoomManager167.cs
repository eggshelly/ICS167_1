using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class NetworkRoomManager167 : NetworkRoomManager
{

    [SerializeField] NetworkManagerHUD hud;


    public override bool OnRoomServerSceneLoadedForPlayer(GameObject roomPlayer, GameObject gamePlayer)
    {
        //gamePlayer.GetComponent<PlayerHandMovement>().AssignNumber(roomPlayer.GetComponent<NetworkRoomPlayer>().index);
        return true;
    }


    bool showStartButton;

    public override void OnRoomServerPlayersReady()
    {
        // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
        if (isHeadless)
            base.OnRoomServerPlayersReady();
        else
            showStartButton = true;
    }

    public override void OnGUI()
    {
        base.OnGUI();

        if (allPlayersReady && showStartButton && GUI.Button(new Rect(150, 300, 120, 20), "START GAME"))
        {
            // set to false to hide it in the game scene
            showStartButton = false;
            ServerChangeScene(GameplayScene);
            hud.showGUI = false;
        }
    }

    public override void ServerChangeScene(string sceneName)
    {
        base.ServerChangeScene(sceneName);
        hud.showGUI = true;
    }
}

