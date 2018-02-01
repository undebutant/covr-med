using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


/// <summary>
///     Setup script used at the creation of the local player avatar.
///     Heritates from NetworkBehaviour
///     TODO use network menu as a way to select the role
/// </summary>
public class NetworkAvatarSetup : NetworkBehaviour {

    // The surgeon spawner that we need to disable once the first player is connected
    private GameObject surgeonSpawner;

    // The nurse spawner that we need to enable once the first player is connected
    private GameObject nurseSpawner;

    /// <summary>
    ///     The ConfigInitializer component containing all the global setup variables
    /// </summary>
    ConfigInitializer configInitializer;

    /// <summary>
    ///     The player's role, read on the configuration file by the ConfigInitialization script
    /// </summary>
    PlayerRole playerRole;

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

    public GameObject GetPlayerCamera() {
        return playerCamera;
    }


    private void Start() {

        surgeonSpawner = GameObject.FindGameObjectWithTag("SurgeonSpawner");
        nurseSpawner = GameObject.FindGameObjectWithTag("NurseSpawner");
        configInitializer = FindObjectOfType<ConfigInitializer>();
        playerRole = configInitializer.GetPlayerRole();

        if (isLocalPlayer) {
            // Enabling the avatar's camera
            playerCamera.SetActive(true);

            // Enabling the player's collider
            playerCollider.enabled = true;

            // Enabling the player's input manager
            playerInputManager.enabled = true;

            // Disabling the player's avatar
            playerAvatar.SetActive(false);

            if (isLocalPlayer) {
                if (playerRole == PlayerRole.Surgeon) {
                    // Positionning the surgeon
                    this.transform.position = surgeonSpawner.transform.position;
                    this.transform.rotation = surgeonSpawner.transform.rotation;

                    // Playing the sit animation for the surgeon avatar
                    playerAvatar.GetComponent<Animation>().Play("M_Sit_Idle_2");
                } else {
                    // Positionning the nurse
                    this.transform.position = nurseSpawner.transform.position;
                    this.transform.rotation = nurseSpawner.transform.rotation;
                }
            } else {
                if (playerRole == PlayerRole.Surgeon)
                {
                    // Positionning the nurse
                    this.transform.position = nurseSpawner.transform.position;
                    this.transform.rotation = nurseSpawner.transform.rotation;
                } else {
                    // Positionning the surgeon
                    this.transform.position = surgeonSpawner.transform.position;
                    this.transform.rotation = surgeonSpawner.transform.rotation;

                    // Playing the sit animation for the surgeon avatar
                    playerAvatar.GetComponent<Animation>().Play("M_Sit_Idle_2");
                }
            }

        // Disabling the global camera of the scene
            GameObject.FindWithTag("SceneCamera").SetActive(false);
        }
    }


    private void Update() {
        if (isLocalPlayer && playerRole == PlayerRole.Surgeon) {
            // Playing the sit animation for the surgeon avatar
            playerAvatar.GetComponent<Animation>().PlayQueued("M_Sit_Idle_2");
        }
        else if (!isLocalPlayer && playerRole != PlayerRole.Surgeon) {
            // Playing the sit animation for the surgeon avatar
            playerAvatar.GetComponent<Animation>().PlayQueued("M_Sit_Idle_2");
        }
    }
}
