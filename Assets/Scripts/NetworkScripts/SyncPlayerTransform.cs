﻿using System.Collections;
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
    [Tooltip("The transform of this specific camera for the rotation")]
    Transform selfTransformCamera;

    [SerializeField]
    [Tooltip("The transform of this specific avatar for the rotation")]
    Transform selfTransformAvatar;

    [SerializeField]
    [Tooltip("The transform of this specific hand of the prefab")]
    Transform selfTransformHand;

    private float journeyLengthLerpAvatar;
    private float journeyLengthLerpHand;

    [SerializeField]
    [Tooltip("The speed to lerp to the final destination")]
    float lerpingSpeed;

    [SerializeField]
    [Tooltip("The speed to slerp to the final destination")]
    float slerpingSpeed;


    // Disclaimer : SyncVar means that everytime a change is made server side, it is automatically send to all clients
    // DOES NOT WORK WHEN A CHANGE IS MADE CLIENT SIDE

    [SyncVar]
    private Vector3 targetPosition;

    [SyncVar]
    private Quaternion targetRotation;

    [SyncVar]
    private Vector3 handPosition;

    [SyncVar]
    private Quaternion handRotation;


    private void FixedUpdate() {
        // Synchronise the position and rotation of the avatar and the hand only if this avatar is not controlled locally
        if (!isLocalPlayer) {
            journeyLengthLerpAvatar = Vector3.Distance(selfTransformAvatar.position, targetPosition);
            journeyLengthLerpHand = Vector3.Distance(selfTransformHand.position, handPosition);
            LerpPosition();
            SlerpRotation();
        }
        // Else we send the movement to the server
        else {
            TransmitPositionToServer();
        }
    }


    private void LerpPosition() {
        // Translate the parent
        float distanceCovered = Time.deltaTime * lerpingSpeed;
        float fractJourney = distanceCovered / journeyLengthLerpAvatar;
        selfTransform.position = Vector3.Lerp(selfTransform.position, targetPosition, fractJourney);
        // Translate the hand
        fractJourney = distanceCovered / journeyLengthLerpHand;
        selfTransformHand.position = Vector3.Lerp(selfTransformHand.position, handPosition, fractJourney);
    }

    private void SlerpRotation() {
        float fractJourney = Time.deltaTime * slerpingSpeed;
        // Rotate the avatar
        selfTransformAvatar.rotation = Quaternion.Slerp(selfTransformAvatar.rotation, targetRotation, fractJourney);
        // Rotate the hand
        selfTransformHand.rotation = Quaternion.Slerp(selfTransformHand.rotation, handRotation, fractJourney);
    }


    /// <summary>
    ///     The method called on the client side, used to push the new target position and target rotation to the server
    /// </summary>
    [Command]
    private void CmdProvidePositionToServer(Vector3 positionReceived, Quaternion rotationReceived, Vector3 newHandPosition, Quaternion newHandRotation) {
        // Update target for the avatar position and rotation
        targetPosition = positionReceived;
        targetRotation = new Quaternion(0, rotationReceived.y, 0, rotationReceived.w);      // Cancelling rotation on x and z axis to prevent weird moves of the avatar

        // Update target for the hand position and rotation
        handPosition = newHandPosition;
        handRotation = newHandRotation;
    }

    /// <summary>
    ///     The method called on the client side to update the position of its own avatar on the server
    /// </summary>
    [ClientCallback]
    private void TransmitPositionToServer() {
        if (isLocalPlayer) {
            // Calling the command to synchronise the transform (position of the parent and the rotation of the camera)
            CmdProvidePositionToServer(selfTransform.position, selfTransformCamera.rotation, selfTransformHand.position, selfTransformHand.rotation);
        }
    }
}
