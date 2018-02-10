using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class KeyboardandWiimoteController : MonoBehaviour {

    [SerializeField] float speed = 1.25f;
    [SerializeField] float rotationspeed = 50f;

    Transform cyclopTransform;
    Rigidbody _rigidbody;

    Vector3 CyclopForward { get { return new Vector3(cyclopTransform.forward.x, 0, cyclopTransform.forward.z);  } }

    void Awake() {
        cyclopTransform = GetComponent<Base_Mobilyz>().Cyclop.transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

	void Update() {
        /*
		if (Input.GetKey(KeyCode.Z)) {
            transform.localPosition += CyclopForward * Time.deltaTime * speed;
		}
		if (Input.GetKey(KeyCode.S)) {
            transform.localPosition -= CyclopForward * Time.deltaTime * speed;
		}
        */
        if (Input.GetKey(KeyCode.Q)) {
            transform.RotateAround(cyclopTransform.position, Vector3.up, -rotationspeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.RotateAround(cyclopTransform.position, Vector3.up, rotationspeed * Time.deltaTime);
        }
        /*
        if (Input.GetKeyDown(KeyCode.P)) {
            if (speed < 8.0f) {
                speed += .25f;
                rotationspeed += 5f;
            }
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            if (speed > .25f) {
                speed -= .25f;
                rotationspeed -= 5f;
            }
        }
         * 
         * * */
    }

    void FixedUpdate() {
        if (Input.GetKey(KeyCode.Z) && contactPoints.All(cp => Vector3.Dot(cp.normal, CyclopForward) >= 0)) {
            _rigidbody.MovePosition(transform.position + (CyclopForward * Time.deltaTime * speed));
        }
        if (Input.GetKey(KeyCode.S) && contactPoints.All(cp => Vector3.Dot(cp.normal, -CyclopForward) >= 0)) {
            _rigidbody.MovePosition(transform.position - (CyclopForward * Time.deltaTime * speed));
        }
    }

    List<ContactPoint> contactPoints = new List<ContactPoint>();
    void OnCollisionEnter(Collision collision) {
        contactPoints.AddRange(collision.contacts);
    }
    void OnCollisionExit(Collision collision) {
        contactPoints.Clear();
    }
}
