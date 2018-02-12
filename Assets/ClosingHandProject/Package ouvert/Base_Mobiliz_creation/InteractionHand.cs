using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CHSF;

public class InteractionHand : MonoBehaviour {

    bool isTriggered = false, _currentManipulatedObjectInTrigger = false;
    GameObject _lastTriggeredObject;
    List<GameObject> _triggeredObjects = new List<GameObject>();
    Vector3 oldPos, newPos;

    [SerializeField] float _breakForce = 500;
    FixedJoint _fixedJoint;

    public bool IsInManipulationMode { get { return _fixedJoint.connectedBody != null; } }
    GameObject LastTriggeredObject { get { return _triggeredObjects.Count > 0 ? _triggeredObjects[_triggeredObjects.Count - 1] : null; } }
    
    void SetOutlineOnLastTriggeredObject(bool b) {
        if (LastTriggeredObject != null) { LastTriggeredObject.GetComponent<Interactable>().Outline(b); }
    }

    void Awake() {
        _fixedJoint = GetComponent<FixedJoint>();
    }

    void Start() {
        _fixedJoint.connectedBody = null;
        _fixedJoint.breakForce = _breakForce;
        newPos = this.transform.position;
        oldPos = this.transform.position;
    }

    void OnTriggerEnter(Collider collider) {
        if (!IsInManipulationMode) {
            SetOutlineOnLastTriggeredObject(false);
            collider.gameObject.GetComponent<Interactable>().Outline(true);
        }
        _triggeredObjects.Add(collider.gameObject);
    }

    void OnTriggerExit(Collider collider) {
        collider.gameObject.GetComponent<Interactable>().Outline(false);
        _triggeredObjects.Remove(collider.gameObject);
        if (!IsInManipulationMode) {
           SetOutlineOnLastTriggeredObject(true);
        }
    }

    public void Join(GameObject objectToLink) {
        _fixedJoint.connectedBody = objectToLink.GetComponent<Rigidbody>();
        objectToLink.GetComponent<Interactable>().Outline(false);
    }

    public void DestroyJoin() {
        SetOutlineOnLastTriggeredObject(true);
        _fixedJoint.connectedBody = null;
    }

    void OnJointBreak(float breakForce) {
        SetOutlineOnLastTriggeredObject(true);
        _fixedJoint = gameObject.AddComponent<FixedJoint>();
        _fixedJoint.breakForce = _breakForce;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && LastTriggeredObject != null) {
            Join(LastTriggeredObject);
        }

        if (Input.GetMouseButtonUp(0) && IsInManipulationMode) {
            DestroyJoin();
            _lastTriggeredObject.GetComponent<Rigidbody>().velocity = (newPos - oldPos) / Time.deltaTime;
        }

        oldPos = newPos;
        newPos = transform.position;
    }
}
