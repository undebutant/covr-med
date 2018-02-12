using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if MIDDLEVR_BUILD
using MiddleVR_Unity3D;
#endif

enum HandState {
    Opened,
    Closed,
}

enum SelectionState {
    Ray,
    Hand,
}


public class WandSelection : MonoBehaviour {

    [SerializeField]
    private string selectableObjectsLayerName = "Selectable";

    [SerializeField]
    private string buttonObjectsLayerName = "Button";

    [SerializeField]
    MainMenuManager mainMenuManager;

    [SerializeField]
    string mainSceneName = "OR_Room";

    GameObject wandCube;
    GameObject wandRay;

    GameObject systemCenterNode;

    int selectableObjectsLayer;
    int buttonObjectsLayer;

    bool isClicked = false;
    bool isClickedButton2 = false;

    bool isHoveringSelectableObject = false;

    bool isObjectSelected = false;
    GameObject selectedObject;

    // Prefab of the local player
    public GameObject prefabPlayer;

    // The remote position
    GameObject wand;

    // The hand script of the local player avatar prefab
    Hand avatarsHand;

    HandCollider avatarsHandCollider;

    private HandState currentHandState = HandState.Opened;

    // Object drag's script of the prefab player
    ObjectDrag objectDrag;

    SoundManager soundManager;

    private SelectionState currentSelectionState = SelectionState.Ray;


    /// <summary>
    ///     Finds the local prefab player
    /// </summary>
    IEnumerator FindPrefabPlayer() {
        while (prefabPlayer == null) {
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject gameObject in gameObjects) {
                // Player prefab are the only one having a NetworkAvatarSetup script, hence they are the only one kept
                if (gameObject.GetComponent<NetworkAvatarSetup>() != null) {
                    // If this player prefab is local
                    if(gameObject.GetComponent<NetworkAvatarSetup>().isLocalPlayer) {
                        prefabPlayer = gameObject;
                    }
                }
            }

            // Do the operation over and over until the player prefabs are found
            if (prefabPlayer == null)
                yield return new WaitForSeconds(1);
        }
        avatarsHand = prefabPlayer.GetComponent<Hand>();
        avatarsHandCollider = avatarsHand.GetComponentInChildren<HandCollider>();
        objectDrag = prefabPlayer.GetComponentInChildren<ObjectDrag>();

        // Wand by default
        avatarsHand.SetHandMeshActive(false);

        yield return null;
    }


    /// <summary>
    ///     Updates the prefab player's hand transform with the wand's transform
    /// </summary>
    void UpdatePrefabPlayerHand() {
        if (prefabPlayer != null) {
            avatarsHand.SetHandTransform(wand.transform.position, wand.transform.rotation);
        }
    }

	// Use this for initialization
	void Start () {
        prefabPlayer = null;

        wand = GameObject.Find("HandNode");

        selectableObjectsLayer =  LayerMask.NameToLayer(selectableObjectsLayerName);
        buttonObjectsLayer = LayerMask.NameToLayer(buttonObjectsLayerName);
        selectedObject = null;

        // Looking for the local player prefab avatar
        if (SceneManager.GetActiveScene().name == mainSceneName) {
            StartCoroutine(FindPrefabPlayer());
        }

        soundManager = GameObject.FindObjectOfType<SoundManager>();

        // Initialize system center node
#if MIDDLEVR_BUILD
        systemCenterNode = GameObject.Find("VRManager").GetComponent<VRManagerScript>().VRSystemCenterNode;
#endif
        wandCube = GameObject.Find("WandCube");
        wandRay = GameObject.Find("WandRay");

        // Initialize the hand
        currentHandState = HandState.Opened;
    }
    
#if MIDDLEVR_BUILD
    void Update () {
        // Update the prefab player hand's transform
        UpdatePrefabPlayerHand();

        Vector3 laserForward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
            
        if (MiddleVR.VRDeviceMgr != null) {

            // Get the state of primary wand buttons
            bool isWandButtonPressed2 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(2);
            bool isWandButtonPressed3 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(3);
                
            // Animate the animated hand
            if (isWandButtonPressed3) {
                if (currentHandState == HandState.Opened) {
                    currentHandState = HandState.Closed;
                    avatarsHand.CloseHand();
                }
            } else {
                if (currentHandState == HandState.Closed) {
                    currentHandState = HandState.Opened;
                    avatarsHand.OpenHand();
                }
            }

            // Switch between the laser and the hand
            if (isWandButtonPressed2 && !isClickedButton2 && SceneManager.GetActiveScene().name == mainSceneName) {
                isClickedButton2 = true;
                switch (currentSelectionState) {
                    case SelectionState.Hand :
                        currentSelectionState = SelectionState.Ray;
                        // Activate the wand cube and the wand ray
                        wandCube.SetActive(true);
                        wandRay.SetActive(true);
                        avatarsHand.SetHandMeshActive(false);
                        break;
                    case SelectionState.Ray :
                        currentSelectionState = SelectionState.Hand;
                        // Deactivate the wand cube and the wand ray
                        wandCube.SetActive(false);
                        wandRay.SetActive(false);
                        avatarsHand.SetHandMeshActive(true);
                        break;
                }
            }

            // Set the ray color when manipulating an object
            if(isObjectSelected)
                GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);

            // The laser forward raycast
            if (Physics.Raycast(transform.position, laserForward , out hit)) {

                // Select an object to drag and drop
                if (hit.collider.gameObject.layer == selectableObjectsLayer && !isObjectSelected) {
                    // Set the color of the wand's ray
                    GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);

                    // Plays the hover sound 
                    if(!isHoveringSelectableObject)
                        soundManager.PlayHoverSound(new Vector3(0, 0, 0));

                    isHoveringSelectableObject = true;

                    if (currentHandState == HandState.Closed && !isObjectSelected && currentSelectionState == SelectionState.Ray) {
                        isObjectSelected = true;
                        selectedObject = hit.collider.gameObject;
                        objectDrag.SelectObject(wand, selectedObject, Vector3.Distance(wand.transform.position, selectedObject.transform.position));
                        // Playing the selection sound effect
                        soundManager.PlaySelectionSound(hit.collider.gameObject.transform.position);
                    }

                    // Click on a menu's button
                } else if (hit.collider.gameObject.layer == buttonObjectsLayer) {
                    // Set the color of the wand's ray
                    GetComponent<VRWand>().SetRayColor(GetComponent<VRRaySelection>().HoverColor);
                    // Click on the button only once
                    if (isWandButtonPressed3 && !isClicked) {
                        isClicked = true;
                        mainMenuManager.OnHitButton(hit.collider.gameObject);
                    }
                }

                // Set isHoveringSelectableObject to false if not hovering
                if (hit.collider.gameObject.layer != selectableObjectsLayer && !isObjectSelected)
                    isHoveringSelectableObject = false;

                // If a navigation zone was selected, a teleportation toward this zone is operated
                if (hit.collider.gameObject.tag == "NavigationZone" && currentSelectionState == SelectionState.Ray) {
                    GameObject zone = hit.collider.gameObject;
                    if (isWandButtonPressed3 && !isClicked) {
                        isClicked = true;
                        systemCenterNode.transform.position = new Vector3(zone.transform.position.x, systemCenterNode.transform.position.y, zone.transform.position.z);                 
                    }
                }
            }
            if (!isWandButtonPressed3)
                isClicked = false;
            if (!isWandButtonPressed2)
                isClickedButton2 = false;
            if (avatarsHandCollider.isOnTriggerStay && isObjectSelected == false && currentHandState == HandState.Closed && currentSelectionState == SelectionState.Hand) {
                isObjectSelected = true;
                selectedObject = avatarsHandCollider.collidedObject;
                objectDrag.SelectObject(wand, selectedObject, 0.0f);
                // Playing the selection sound effect
                soundManager.PlaySelectionSound(selectedObject.transform.position);
            }
        }

        // Release Object (if one is selected) whenever the hand is opened
        if (currentHandState == HandState.Opened && isObjectSelected) {
            isObjectSelected = false;
            objectDrag.ReleaseObject();

            // Playing the selection sound effect
            soundManager.PlayDropSound(selectedObject.transform.position);
            selectedObject = null;
        }
    }
#endif
}
