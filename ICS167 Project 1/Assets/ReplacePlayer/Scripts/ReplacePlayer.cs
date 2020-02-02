using Mirror;
using UnityEngine;

namespace ReplacePlayerTest
{
    public class ReplacePlayer : NetworkBehaviour
    {
        public override void OnStartLocalPlayer()
        {
            Camera.main.transform.position = transform.position + new Vector3(0, 4f, -7f);
            Camera.main.transform.LookAt(transform);
        }

        void Update()
        {
            if (!isLocalPlayer) return;

            if (Input.GetKeyUp(KeyCode.A))
                CmdNewPlayer(0);

            if (Input.GetKeyUp(KeyCode.S))
                CmdNewPlayer(1);

            if (Input.GetKeyUp(KeyCode.D))
                CmdNewPlayer(2);
        }

        [Command]
        void CmdNewPlayer(int model)
        {
            Vector3 startPos = transform.position;

            GameObject newPlayer = Instantiate(NetworkManager.singleton.spawnPrefabs[model], startPos, Quaternion.identity);

            GameObject oldPlayer = gameObject;

            NetworkServer.ReplacePlayerForConnection(connectionToClient, newPlayer, true);

            NetworkServer.Destroy(oldPlayer);
        }

        void OnMouseUpAsButton()
        {
            Debug.LogError("OnMouseUpAsButton");
            NetworkClient.connection.identity.GetComponent<ReplacePlayer>().ReplaceMe(gameObject);
        }

        public void ReplaceMe(GameObject target)
        {
            Debug.LogError("ReplaceMe");
            CmdReplacePlayer(target);
        }

        [Command]
        public void CmdReplacePlayer(GameObject target)
        {
            Debug.LogError("CmdReplacePlayer");

            NetworkConnection owner = target.GetComponent<NetworkIdentity>().connectionToClient;

            if (owner != null && owner == connectionToClient)
            {
                Debug.LogError($"{target} | {target.GetComponent<NetworkIdentity>().connectionToClient} | {connectionToClient}");
                return;
            }

            NetworkServer.ReplacePlayerForConnection(connectionToClient, target);
        }
    }
}
