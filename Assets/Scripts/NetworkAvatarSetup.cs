﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
///     Setup script used at the creation of the local player avatar.
///     Heritates from NetworkBehaviour
/// </summary>
public class NetworkAvatarSetup : NetworkBehaviour {

    // The surgeon spawner that we need to disable once the first player is connected
    private GameObject surgeonSpawner;

    // The nurse spawner that we need to enable once the first player is connected
    private GameObject nurseSpawner;


    [SerializeField]
    [Tooltip("The camera of the player avatar prefab, to set active on creation of the avatar")]
    GameObject playerCamera;

    [SerializeField]
    [Tooltip("The avatar rendering of the player avatar prefab, to set active on creation of the avatar")]
    GameObject playerAvatar;

    [SerializeField]
    [Tooltip("The collider of the player avatar prefab, to set active on creation of the avatar")]
    Collider playerCollider;

    [SerializeField]
    [Tooltip("The input manager of the player avatar prefab, to set active on creation of the avatar")]
    InputManager playerInputManager;


    private void Start() {

        surgeonSpawner = GameObject.FindGameObjectWithTag("SurgeonSpawner");
        nurseSpawner = GameObject.FindGameObjectWithTag("NurseSpawner");


        if (isLocalPlayer) {
            // Enabling the avatar's camera
            playerCamera.SetActive(true);

            // Enabling the player's collider
            playerCollider.enabled = true;

            // Enabling the player's input manager
            playerInputManager.enabled = true;

            // Disabling the player's avatar
            playerAvatar.SetActive(false);


            if (isServer && isLocalPlayer) {
                // Positionning the surgeon
                this.transform.position = surgeonSpawner.transform.position;
                this.transform.rotation = surgeonSpawner.transform.rotation;

                // Playing the sit animation for the surgeon avatar
                playerAvatar.GetComponent<Animation>().Play("M_Sit_Idle_2");
            }
            else if (!isServer && !isLocalPlayer) {
                // Positionning the surgeon
                this.transform.position = surgeonSpawner.transform.position;
                this.transform.rotation = surgeonSpawner.transform.rotation;

                // Playing the sit animation for the surgeon avatar
                playerAvatar.GetComponent<Animation>().Play("M_Sit_Idle_2");
            }
            else if (isServer && !isLocalPlayer) {
                // Positionning the nurse
                this.transform.position = nurseSpawner.transform.position;
                this.transform.rotation = nurseSpawner.transform.rotation;
            }
            else if (!isServer && isLocalPlayer) {
                // Positionning the nurse
                this.transform.position = nurseSpawner.transform.position;
                this.transform.rotation = nurseSpawner.transform.rotation;
            }


            // Disabling the global camera of the scene
            GameObject.FindWithTag("SceneCamera").SetActive(false);
        }
    }


    private void Update() {
        if (isServer && isLocalPlayer) {
            // Playing the sit animation for the surgeon avatar
            playerAvatar.GetComponent<Animation>().PlayQueued("M_Sit_Idle_2");
        }
        else if (!isServer && !isLocalPlayer) {
            // Playing the sit animation for the surgeon avatar
            playerAvatar.GetComponent<Animation>().PlayQueued("M_Sit_Idle_2");
        }
    }
}
