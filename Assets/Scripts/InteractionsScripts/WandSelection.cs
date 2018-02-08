using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CHSF;
// TODO use ifdef maybe ?
// using MiddleVR_Unity3D;

enum SelectionState {
    Ray,
    Hand,
}

enum AnimatedHandState {
    Opened,
    Closed,
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

    [SerializeField]
    GameObject animatedHand;
    HandLerp animatedHandHandLerp;

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

    // Object drag's script of the prefab player
    ObjectDrag objectDrag;

    SoundManager soundManager;

    private SelectionState currentSelectionState = SelectionState.Ray;
    private AnimatedHandState currentAnimationHandState = AnimatedHandState.Opened;


    /// <summary>
    ///     Finds the local prefab player
    /// </summary>
    IEnumerator FindPrefabPlayer() {
        List<GameObject> playerPrefabs = new List<GameObject>();

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
        objectDrag = prefabPlayer.GetComponentInChildren<ObjectDrag>();

        // Do not show the hand's mesh
        // Find the hand's mesh
        avatarsHand.GetHandMesh().SetActive(false);

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
        systemCenterNode = GameObject.Find("VRManager").GetComponent<VRManagerScript>().VRSystemCenterNode;
        wandCube = GameObject.Find("WandCube");
        wandRay = GameObject.Find("WandRay");

        // Disable the animated hand by default
        if (animatedHand != null) {
            animatedHand.SetActive(false);
            animatedHandHandLerp = animatedHand.GetComponent<HandLerp>();
        }   
    }

    // TODO see TODO above, need workaround for non MiddleVR devices
    /*
    void Update () {
        // Update the prefab player hand's transform
        UpdatePrefabPlayerHand();

        Vector3 laserForward = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        // Updated the animated hand's position
        if (animatedHand != null) {
            animatedHand.transform.position = transform.position;
            animatedHand.transform.rotation = transform.rotation;
        }
            
        if (MiddleVR.VRDeviceMgr != null) {

            // Get the state of primary wand buttons
            bool isWandButtonPressed2 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(2);
            bool isWandButtonPressed0 = MiddleVR.VRDeviceMgr.IsWandButtonPressed(0);
                
            // Animate the animated hand
            if (isWandButtonPressed0) {
                if (currentAnimationHandState == AnimatedHandState.Opened) {
                    currentAnimationHandState = AnimatedHandState.Closed;
                    animatedHandHandLerp.Play();
                }
            } else {
                if (currentAnimationHandState == AnimatedHandState.Closed) {
                    currentAnimationHandState = AnimatedHandState.Opened;
                    animatedHandHandLerp.Revert();
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
                        animatedHand.SetActive(false);
                        break;
                    case SelectionState.Ray :
                        currentSelectionState = SelectionState.Hand;
                        // Deactivate the wand cube and the wand ray
                        wandCube.SetActive(false);
                        wandRay.SetActive(false);
                        animatedHand.SetActive(true);
                        break;
                    default:
                        currentSelectionState = SelectionState.Hand;
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

                    if (isWandButtonPressed0 && !isClicked) {
                        isClicked = true;
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
                    if (isWandButtonPressed0 && !isClicked) {
                        isClicked = true;
                        mainMenuManager.OnHitButton(hit.collider.gameObject);
                    }

                    // Release object
                } else if (isWandButtonPressed0 && !isClicked && isObjectSelected) {
                        isClicked = true;
                        isObjectSelected = false;
                        selectedObject = null;
                        objectDrag.ReleaseObject();

                        // Playing the selection sound effect
                        soundManager.PlayDropSound(hit.collider.gameObject.transform.position);
                }

                // Set isHoveringSelectableObject to false if not hovering
                if (hit.collider.gameObject.layer != selectableObjectsLayer && !isObjectSelected)
                    isHoveringSelectableObject = false;

                // If a navigation zone was selected, a teleportation toward this zone is operated
                if (hit.collider.gameObject.tag == "NavigationZone") {
                    GameObject zone = hit.collider.gameObject;
                    if (isWandButtonPressed0 && !isClicked) {
                        isClicked = true;
                        systemCenterNode.transform.position = new Vector3(zone.transform.position.x, systemCenterNode.transform.position.y, zone.transform.position.z);                 
                    }
                }
            }
            if (!isWandButtonPressed0)
                isClicked = false;
            if (!isWandButtonPressed2)
                isClickedButton2 = false;
        }
    }
    */
}
