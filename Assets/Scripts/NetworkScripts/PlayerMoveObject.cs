using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
///     Script to synchronise the transform of a scene object that a player moved
///     The object needs to have a NetworkIdentity and a Rigidbody
/// </summary>
public class PlayerMoveObject : NetworkBehaviour {

    // Synchronise the ID of the GameObject that we want to move
    [SyncVar] private GameObject objectID;

    private NetworkIdentity objNetId;

    [SerializeField]
    [Tooltip("The time taken to lerp to the final destination")]
    float lerpingTime;

    [SerializeField]
    [Tooltip("The time taken to slerp to the final orientation")]
    float slerpingTime;


    private void LerpPosition(GameObject objectToMove, Vector3 targetPosition) {
        // Translate smoothly the object
        objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, targetPosition, Time.deltaTime * lerpingTime);
    }

    private void SlerpRotation(GameObject objectToMove, Quaternion targetRotation) {
        // Rotate smoothly the object
        objectToMove.transform.rotation = Quaternion.Slerp(objectToMove.transform.rotation, targetRotation, Time.deltaTime * slerpingTime);
    }


    /// <summary>
    ///     The callable method that we need to call in order to synchronise an object movement
    /// </summary>
    public void MoveObject(GameObject objectToMove, Vector3 pos, Quaternion rot) {
        // Making sure that the call is made by a local player
        if (isLocalPlayer) {
            objectID = objectToMove;
            
            // Method called client side, to be executed server side
            CmdMove(objectID,pos,rot);
        }
    }

    public void SyncObjectKinematic(GameObject objectToSyncKinematic, bool isKinematicOn) {
        // Making sure that the call is made by a local player
        if (isLocalPlayer) {
            objectID = objectToSyncKinematic;

            // Method called client side, to be executed server side
            CmdSyncKinematic(objectID, isKinematicOn);
        }
    }


    /// <summary>
    ///     Method called server side, so that all clients execute this method
    /// </summary>
    [ClientRpc]
    void RpcMove(GameObject obj,Vector3 pos, Quaternion rot) {
        LerpPosition(obj, pos);
        SlerpRotation(obj, rot);
    }

    /// <summary>
    ///     Method called server side, so that all clients execute this method
    /// </summary>
    [ClientRpc]
    void RpcSyncKinematic(GameObject obj, bool isObjectKinematicOn) {
        obj.GetComponent<Rigidbody>().isKinematic = isObjectKinematicOn;
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

    /// <summary>
    ///     The client ask for the server the ownership of the GameObject for a short time, to apply modifications
    /// </summary>
    [Command]
    void CmdSyncKinematic(GameObject obj, bool isObjectKinematicOn) {
        objNetId = obj.GetComponent<NetworkIdentity>();             // Get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);         // Assign authority to the player who is changing a property
        RpcSyncKinematic(obj, isObjectKinematicOn);                 // Use a Client RPC function to modify the object on all clients
        objNetId.RemoveClientAuthority(connectionToClient);         // Remove the authority from the player who changed the property
    }
}
