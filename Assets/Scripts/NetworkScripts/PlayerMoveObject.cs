using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMoveObject : NetworkBehaviour {


    [SyncVar] private GameObject objectID;
    private NetworkIdentity objNetId;

    public void moveObject(GameObject objectToMove,Vector3 pos, Quaternion rot) {
        if (isLocalPlayer) {

            objectID = objectToMove;
            CmdMove(objectID,pos,rot);
            
        }
    }

    [ClientRpc]
    void RpcMove(GameObject obj,Vector3 pos, Quaternion rot) {
        obj.transform.position = pos;
        obj.transform.rotation = rot;
    }

    [Command]
    void CmdMove(GameObject obj, Vector3 pos, Quaternion rot) {
        objNetId = obj.GetComponent<NetworkIdentity>();        // get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);    // assign authority to the player who is changing the color
        RpcMove(obj, pos,rot);                                    // usse a Client RPC function to "paint" the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);    // remove the authority from the player who changed the color
    }
}
