using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
///     Script to synchronise the transform of a scene object that a player moved
///     The object needs to have a NetworkIdentity
/// </summary>
public class PlayerMoveObject : NetworkBehaviour {

    // Synchronise the ID of the GameObject that we want to move
    [SyncVar] private GameObject objectID;

    private NetworkIdentity objNetId;


    /// <summary>
    ///     The callable method that we need to call in order to synchronise an object movement
    /// </summary>
    public void moveObject(GameObject objectToMove, Vector3 pos, Quaternion rot) {
        // Making sure that the call is made by a local player
        // TODO deactivate the ObjectDrag script on the non local avatar
        if (isLocalPlayer) {
            objectID = objectToMove;
            
            // Method called client side, to be executed server side
            CmdMove(objectID,pos,rot);
        }
    }


    /// <summary>
    ///     Method called server side, so that all clients execute this method
    /// </summary>
    [ClientRpc]
    void RpcMove(GameObject obj,Vector3 pos, Quaternion rot) {
        obj.transform.position = pos;
        obj.transform.rotation = rot;
    }


    /// <summary>
    ///     The client ask for the server the ownership of the GameObject for a short time, to apply modifications
    /// </summary>
    [Command]
    void CmdMove(GameObject obj, Vector3 pos, Quaternion rot) {
        objNetId = obj.GetComponent<NetworkIdentity>();             // Get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);         // Assign authority to the player who is changing a property
        RpcMove(obj, pos, rot);                                     // Use a Client RPC function to modify the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);         // Remove the authority from the player who changed the property
    }
}
