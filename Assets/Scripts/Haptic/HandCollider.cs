using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    [SerializeField]
    Hand handScript;

    // The int value of the layer mask "selectionable"
    int layerSelectable;

    void Start() {

        //Initiate the value of the layer "selectionable"
        layerSelectable = LayerMask.NameToLayer("selectionable");

    }


    private void OnTriggerEnter(Collider other) {

        //If the object that collides with the hand is a selectionable object...
        if (other.gameObject.layer == layerSelectable) {
            // ... tell the handScript that he can grab this object
            handScript.ObjectToSelect = other.gameObject;
        }
        
    }


    private void OnTriggerExit(Collider other) {

        //If the object no longer colliding with the hand is a selectionable object...
        if (other.gameObject.layer == layerSelectable)
        {
            // ... tell the handscript that he can no longer grab it
            handScript.ObjectToSelect = null;
        }
    }


}
