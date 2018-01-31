using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {

    [SerializeField]
    Hand handScript;

    // Variable to know if we are in the tissue
    bool isContactTissue = false;

    public bool getIsContactTissue() {
        return isContactTissue;
    }

    public void InTissue() {
        isContactTissue = true;
    }

    public void LeaveTissue() {
        isContactTissue = false;
    }

    // Variable to know if we are in the table
    bool isContactTable = false;

    public bool getIsContactTable() {
        return isContactTable;
    }

    public void ContacteTable() {
        isContactTable = true;
    }

    public void LeaveTable() {
        isContactTable = false;
    }

    // The int value of the layer mask "Selectable"
    int layerSelectable;
    int layerPatient;
    int layerTable;

    void Start() {

        //Initiate the value of the layer "Selectable"
        layerSelectable = LayerMask.NameToLayer("Selectable");
        layerPatient = LayerMask.NameToLayer("Patient");
        layerTable = LayerMask.NameToLayer("Table");
        isContactTissue = false;
        isContactTable = false;
    }


    private void OnTriggerEnter(Collider other) {

        //If the object that collides with the hand is a selectable object...
        if (other.gameObject.layer == layerSelectable) {
            // ... tell the handScript that he can grab this object
            handScript.ObjectToSelect = other.gameObject;
        }

        //If the object that collides with the hand is a patient object...
        if (other.gameObject.layer == layerPatient) {
            InTissue();
        }

        //If the object that collides with the hand is a table object...
        if (other.gameObject.layer == layerTable) {
            ContacteTable();
        }

    }


    private void OnTriggerExit(Collider other) {

        //If the object no longer colliding with the hand is a selectable object...
        if (other.gameObject.layer == layerSelectable)
        {
            // ... tell the handscript that he can no longer grab it
            handScript.ObjectToSelect = null;
        }

        //If the object that collides with the hand is a patient object...
        if (other.gameObject.layer == layerPatient) {
            LeaveTissue();
        }

        //If the object that collides with the hand is a table object...
        if (other.gameObject.layer == layerTable) {
            LeaveTable();
        }
    }


}
