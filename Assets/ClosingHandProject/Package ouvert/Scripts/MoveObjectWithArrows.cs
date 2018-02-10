using UnityEngine;
using System.Collections;

public class MoveObjectWithArrows : MonoBehaviour {

    public float rotSpeed = 1.0f;
    public GameObject repere;
    private Quaternion rotInit;
    private Vector3 posRepInit;
    private Vector3 posObjInit;
	// Use this for initialization
	void Start () {
        rotInit = this.transform.rotation;
        posRepInit = repere.transform.position;
        posObjInit = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.RotateAround(this.transform.position, repere.transform.forward, rotSpeed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.RotateAround(this.transform.position, repere.transform.forward, -rotSpeed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.RotateAround(repere.transform.position, repere.transform.up, rotSpeed);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.RotateAround(repere.transform.position, repere.transform.up, -rotSpeed);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            this.transform.parent.GetComponent<Transform>().localScale += new Vector3(0.1f*this.transform.localScale.x, 0.1f*this.transform.localScale.y, 0.1f*this.transform.localScale.z);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            this.transform.parent.GetComponent<Transform>().localScale -= new Vector3(0.1f * this.transform.localScale.x, 0.1f * this.transform.localScale.y, 0.1f * this.transform.localScale.z);
        }
        if (Input.GetMouseButtonDown(0))
        {
            this.transform.rotation = rotInit;
            this.transform.position = posObjInit;
            repere.transform.position = posRepInit;
        }
	}
}