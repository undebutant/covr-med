using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
///     Script to synchronise the transform of all of the players avatars
/// </summary>
public class SyncPlayerTransform : NetworkBehaviour {

    [SerializeField]
    [Tooltip("The transform of this specific avatar")]
    Transform selfTransform;

    [SerializeField]
    [Tooltip("The time taken to lerp to the final destination")]
    float lerpingTime;

    [SerializeField]
    [Tooltip("The time taken to slerp to the final orientation")]
    float slerpingTime;


    // The position sent by the client's GameObject, that we went to synchronise to
    [SyncVar]
    private Vector3 targetPosition;

    // The rotation sent by the client's GameObject, that we went to synchronise to
    [SyncVar]
    private Quaternion targetRotation;


    private void FixedUpdate() {
        // Update the position and rotation only if this avatar is not controlled locally
        if (!isLocalPlayer) {
            LerpPosition();
            SlerpRotation();
        }
        else {
            TransmitPositionToServer();
        }
    }


    private void LerpPosition() {
        selfTransform.position = Vector3.Lerp(selfTransform.position, targetPosition, Time.deltaTime * lerpingTime);
    }

    private void SlerpRotation() {
        selfTransform.rotation = Quaternion.Slerp(selfTransform.rotation, targetRotation, Time.deltaTime * slerpingTime);
    }


    /// <summary>
    ///     The method called on the server side, to update the target position and rotation for this avatar on every other instance of the application
    /// </summary>
    [Command]
    private void CmdProvidePositionToServer(Vector3 positionReceived, Quaternion rotationReceived) {
        targetPosition = positionReceived;
        targetRotation = new Quaternion(0, rotationReceived.y, 0, rotationReceived.w);  // Cancelling rotation on x and z axis to prevent weird moves of the avatar
    }

    /// <summary>
    ///     The method called on the client side by the avatar, in order to inform the server of its new position and rotation
    /// </summary>
    [ClientCallback]
    private void TransmitPositionToServer() {
        if (isLocalPlayer) {
            // Calling the server side command to synchronise the transform
            CmdProvidePositionToServer(selfTransform.position, selfTransform.rotation);
        }
    }
}
