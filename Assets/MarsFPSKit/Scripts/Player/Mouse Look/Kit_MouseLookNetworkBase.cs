using Mirror;
using UnityEngine;

namespace MarsFPSKit
{
    public abstract class Kit_MouseLookNetworkBase : NetworkBehaviour
    {
        [SyncVar]
        /// <summary>
        /// The network id of the player object that this belongs to
        /// </summary>
        public uint ownerPlayerNetworkId;

        public override void OnStartServer()
        {
            if (NetworkServer.spawned.ContainsKey(ownerPlayerNetworkId))
            {
                Kit_PlayerBehaviour pb = NetworkServer.spawned[ownerPlayerNetworkId].GetComponent<Kit_PlayerBehaviour>();
                pb.customMouseLookData = this;
            }
        }

        public override void OnStartClient()
        {
            if (NetworkClient.spawned.ContainsKey(ownerPlayerNetworkId))
            {
                Kit_PlayerBehaviour pb = NetworkClient.spawned[ownerPlayerNetworkId].GetComponent<Kit_PlayerBehaviour>();
                pb.customMouseLookData = this;

                //Initialize movement
                pb.looking.InitializeClient(pb);
            }
        }
    }
}