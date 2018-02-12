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

    private float journeyLengthLerp;

    private IEnumerator slerpCoroutine;
    private IEnumerator lerpCoroutine;

    [SerializeField]
    [Tooltip("The speed to lerp to the final destination")]
    float lerpingSpeed;

    [SerializeField]
    [Tooltip("The speed to slerp to the final destination")]
    float slerpingSpeed;

    [SerializeField]
    [Tooltip("Snap's lerp duraction")]
    float lerpDuration;

    [SerializeField]
    [Tooltip("Snap's slerp duraction")]
    float slerpDuration;

    [SerializeField]
    [Tooltip("Maximum allowed distance between the object and its target")]
    float snapDistanceThreshold;

    [SerializeField]
    [Tooltip("Maximum allowed angle between the object and its target")]
    float snapAngleThreshold;



    private void LerpPosition(GameObject objectToMove, Vector3 targetPosition) {
        // Translate smoothly the object
        float distanceCovered = Time.deltaTime * lerpingSpeed;
        float fractJourney = distanceCovered / journeyLengthLerp;
        objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, targetPosition, fractJourney);
    }

    IEnumerator LerpPositionCoroutine(GameObject objectToMove, Vector3 targetPosition) {
        float timeSpent = 0;
        float startingTime = Time.timeSinceLevelLoad;
        float fractJourney = 0;
        while (Vector3.Distance(objectToMove.transform.position, targetPosition) > snapDistanceThreshold) {
            timeSpent = Time.timeSinceLevelLoad - startingTime;
            fractJourney = timeSpent / lerpDuration;
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, targetPosition, fractJourney);
            yield return new WaitForSeconds(.011f);
        }
        yield return null;
    }

    private void SlerpRotation(GameObject objectToMove, Quaternion targetRotation) {
        // Rotate smoothly the object
        float fractJourney = Time.deltaTime * slerpingSpeed;
        objectToMove.transform.rotation = Quaternion.Slerp(objectToMove.transform.rotation, targetRotation, fractJourney);
    }

    IEnumerator SlerpRotationCoroutine(GameObject objectToMove, Quaternion targetRotation) {
        float timeSpent = 0;
        float startingTime = Time.timeSinceLevelLoad;
        float fractJourney = 0;
        while (Quaternion.Angle(objectToMove.transform.rotation, targetRotation) > snapAngleThreshold) {
            timeSpent = Time.timeSinceLevelLoad - startingTime;
            fractJourney = timeSpent / slerpDuration;
            objectToMove.transform.rotation = Quaternion.Slerp(objectToMove.transform.rotation, targetRotation, fractJourney);
            yield return new WaitForSeconds(.011f);
        }
        yield return null;
    }


    /// <summary>
    ///     The callable method that we need to call in order to synchronise an object movement
    /// </summary>
    public void MoveObject(GameObject objectToMove, Vector3 pos, Quaternion rot, bool isObjectReleased) {
        // Making sure that the call is made by a local player
        if (isLocalPlayer) {
            objectID = objectToMove;
            
            // Method called client side, to be executed server side
            if (isObjectReleased) {
                CmdMoveWhenReleased(objectID, pos, rot);
            } else {
                CmdMove(objectID, pos, rot);
            }
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
        journeyLengthLerp = Vector3.Distance(obj.transform.position, pos);
        LerpPosition(obj, pos);
        SlerpRotation(obj, rot);
    }

    [ClientRpc]
    void RpcMoveWhenReleased(GameObject obj, Vector3 pos, Quaternion rot) {
        journeyLengthLerp = Vector3.Distance(obj.transform.position, pos);
        lerpCoroutine = LerpPositionCoroutine(obj, pos);
        slerpCoroutine = SlerpRotationCoroutine(obj, rot);
        StartCoroutine(lerpCoroutine);
        StartCoroutine(slerpCoroutine);
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
    void CmdMoveWhenReleased(GameObject obj, Vector3 pos, Quaternion rot) {
        objNetId = obj.GetComponent<NetworkIdentity>();             // Get the object's network ID
        objNetId.AssignClientAuthority(connectionToClient);         // Assign authority to the player who is changing a property
        RpcMoveWhenReleased(obj, pos, rot);                                     // Use a Client RPC function to modify the object on all clients
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
