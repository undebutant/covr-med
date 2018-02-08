using UnityEngine;
using System.Collections;

public class LineRendererRaycast : MonoBehaviour {
    private RaycastHit hitInfo;
    private Vector3 origin;
    private Vector3 endLine;
    private LineRenderer lr;
    
	// Use this for initialization
	void Start () {
        lr = this.GetComponentInChildren<LineRenderer>();
        origin = this.transform.position;
        endLine = origin + 1.0f * this.transform.forward;
        lr.SetPosition(0, origin);
        lr.SetPosition(1, endLine);

	}
	
	// Update is called once per frame
	void Update () {
        origin = this.transform.position;
        if (Physics.Raycast(GetComponentInChildren<Transform>().position, transform.TransformDirection(Vector3.forward), out hitInfo))
        {
            endLine = origin + hitInfo.distance * this.transform.forward;
        }
        else
        {
            endLine = origin + 1.0f*this.transform.forward;
        }
        lr.SetPosition(0, origin);
        lr.SetPosition(1, endLine);
	}
}
