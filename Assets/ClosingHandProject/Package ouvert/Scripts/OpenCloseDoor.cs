using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenCloseDoor : MonoBehaviour
{

    private bool isMoving = false;
    private bool isOpen = false;
    public GameObject repere;
    public float open;
    public float speed = 2.0f;
    private float angle_door = 0.0f;
    private enum axisRot { X, Y, Z };
    [SerializeField]
    private axisRot axis;
    private Quaternion rotInit;

    // Use this for initialization
    void Start()
    {
        rotInit = this.transform.rotation;
    }

    public void MoveDoor()
    {
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (isOpen)
            {
                angle_door -= speed;
                if (angle_door > 0)
                {
                    if (open > 0)
                    {
                        switch (axis)
                        {
                            case axisRot.X:
                                this.transform.RotateAround(repere.transform.position, repere.transform.right, -speed);
                                break;
                            case axisRot.Y:
                                this.transform.RotateAround(repere.transform.position, repere.transform.up, -speed);
                                break;
                            case axisRot.Z:
                                this.transform.RotateAround(repere.transform.position, repere.transform.forward, -speed);
                                break;
                        }
                    }
                    else if(open < 0)
                    {
                        switch (axis)
                        {
                            case axisRot.X:
                                this.transform.RotateAround(repere.transform.position, repere.transform.right, speed);
                                break;
                            case axisRot.Y:
                                this.transform.RotateAround(repere.transform.position, repere.transform.up, speed);
                                break;
                            case axisRot.Z:
                                this.transform.RotateAround(repere.transform.position, repere.transform.forward, speed);
                                break;
                        }
                    }
                    
                }
                else
                {
                    angle_door = 0.0f;
                    this.transform.rotation = rotInit;
                    isMoving = false;
                    isOpen = false;
                }
            }
            else
            {
                angle_door += speed;
                if (angle_door < Mathf.Abs(open))
                {
                    if (open > 0)
                    {
                        switch (axis)
                        {
                            case axisRot.X:
                                this.transform.RotateAround(repere.transform.position, repere.transform.right, speed);
                                break;
                            case axisRot.Y:
                                this.transform.RotateAround(repere.transform.position, repere.transform.up, speed);
                                break;
                            case axisRot.Z:
                                this.transform.RotateAround(repere.transform.position, repere.transform.forward, speed);
                                break;
                        }
                    }
                    else if(open < 0)
                    {
                        switch (axis)
                        {
                            case axisRot.X:
                                this.transform.RotateAround(repere.transform.position, repere.transform.right, -speed);
                                break;
                            case axisRot.Y:
                                this.transform.RotateAround(repere.transform.position, repere.transform.up, -speed);
                                break;
                            case axisRot.Z:
                                this.transform.RotateAround(repere.transform.position, repere.transform.forward, -speed);
                                break;
                        }
                    }
                }
                else
                {
                    angle_door = open;
                    this.transform.rotation = rotInit;
                    switch (axis)
                    {
                        case axisRot.X:
                            this.transform.RotateAround(repere.transform.position, repere.transform.right, open);
                            break;
                        case axisRot.Y:
                            this.transform.RotateAround(repere.transform.position, repere.transform.up, open);
                            break;
                        case axisRot.Z:
                            this.transform.RotateAround(repere.transform.position, repere.transform.forward, open);
                            break;
                    }
                    isMoving = false;
                    isOpen = true;
                }
            }
        }    
    }
}
