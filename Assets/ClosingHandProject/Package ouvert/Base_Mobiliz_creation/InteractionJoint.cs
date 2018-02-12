using UnityEngine;
using System.Collections;

public class InteractionJoint : MonoBehaviour {
	// Use this for initialization
	void Start () {
        this.GetComponent<FixedJoint>().connectedBody = new Rigidbody();
        this.GetComponent<FixedJoint>().breakForce = 100;
	}

    public void Join(GameObject objectToLink)
    {
        this.GetComponent<FixedJoint>().connectedBody = objectToLink.GetComponent<Rigidbody>();
    }

    public void DestroyJoin()
    {
        this.GetComponent<FixedJoint>().connectedBody = null;
        Destroy(this.GetComponent<FixedJoint>());
        this.gameObject.AddComponent<FixedJoint>().breakForce = 100;
    }

    void OnJointBreak(float breakForce)
    {
        this.gameObject.AddComponent<FixedJoint>().breakForce = 100;
    }

	// Update is called once per frame
	void Update () {
	    
	}
}
