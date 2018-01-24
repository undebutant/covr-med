using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
///     Script to synchronise the transform of all of the players avatars with smoothness
/// </summary>
public class SyncPlayerTransform : NetworkBehaviour {

    [SerializeField]
    [Tooltip("The transform of this specific prefab player for the translation")]
    Transform selfTransform;

    [SerializeField]
    [Tooltip("The transform of this specific avatar for the rotation")]
    Transform selfTransformCamera;

    [SerializeField]
    [Tooltip("The transform of this specific avatar for the rotation")]
    Transform selfTransformAvatar;

    [SerializeField]
    [Tooltip("The time taken to lerp to the final destination")]
    float lerpingTime;

    [SerializeField]
    [Tooltip("The time taken to slerp to the final orientation")]
    float slerpingTime;


    // Disclaimer : SyncVar means that everytime a change is made server side, it is automatically send to all clients
    // DOES NOT WORK WHEN A CHANGE IS MADE CLIENT SIDE

    [SyncVar]
    private Vector3 targetPosition;

    [SyncVar]
    private Quaternion targetRotation;

    [SyncVar]
    private Vector3 handPosition;

    private Vector3 handPositionLocalToBeSend;



    public void UpdateHandPosition(Vector3 newPos) {
        handPositionLocalToBeSend = newPos;
    }

    public Vector3 getHandPosition() {
        return handPosition;
    }

    private void FixedUpdate() {
        // Synchronise the position and rotation only if this avatar is not controlled locally
        if (!isLocalPlayer) {
            LerpPosition();
            SlerpRotation();
        }
        // Else we send the movement to the server
        else {
            TransmitPositionToServer();
        }
    }


    private void LerpPosition() {
        selfTransform.position = Vector3.Lerp(selfTransform.position, targetPosition, Time.deltaTime * lerpingTime);
    }

    private void SlerpRotation() {
        selfTransformAvatar.rotation = Quaternion.Slerp(selfTransformAvatar.rotation, targetRotation, Time.deltaTime * slerpingTime);
    }


    /// <summary>
    ///     The method called on the client side, used to push the new target position and target rotation to the server
    /// </summary>
    [Command]
    private void CmdProvidePositionToServer(Vector3 positionReceived, Quaternion rotationReceived, Vector3 newHandPosition) {
        targetPosition = positionReceived;
        targetRotation = new Quaternion(0, rotationReceived.y, 0, rotationReceived.w);  // Cancelling rotation on x and z axis to prevent weird moves of the avatar
        handPosition = newHandPosition;
    }

    /// <summary>
    ///     The method called on the client side to update the position of its own avatar on the server
    /// </summary>
    [ClientCallback]
    private void TransmitPositionToServer() {
        if (isLocalPlayer) {
            // Calling the command to synchronise the transform
            CmdProvidePositionToServer(selfTransform.position, selfTransformCamera.rotation, handPositionLocalToBeSend);
        }
    }
}
