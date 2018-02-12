using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OpenCloseDrawer : MonoBehaviour
{

    private bool isMoving = false;
    private bool isOpen = false;
    public GameObject repere;
    public float open;
    public float speed = 2.0f;
    private enum axisTrans { X, Y, Z };
    [SerializeField]
    private axisTrans axis;
    private float pos;
    private Vector3 posInit;

    // Use this for initialization
    void Start()
    {
        pos = 0.0f; //Equals 0 when the drawer is closed
        posInit = this.transform.position;
    }

    public void MoveDrawer()
    {
        if (isMoving)
        {
            return;
        }

        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving){
            if (isOpen){
                pos -= speed;
                switch (axis)
                {
                    case axisTrans.X:
                        if (pos > 0)
                        {
                            if (open < 0)
                            {
                                this.transform.Translate(new Vector3(speed, 0, 0));
                            }
                            else if (open > 0)
                            {
                                this.transform.Translate(new Vector3(-speed, 0, 0));
                            }
                        }
                        else
                        {
                            pos = 0.0f;
                            this.transform.position = posInit;
                            isMoving = false;
                            isOpen = false;
                        }
                        break;
                    case axisTrans.Y:
                        if (pos != 0)
                        {
                            if (open < 0)
                            {
                                this.transform.Translate(new Vector3(0, speed, 0));
                            }
                            else if (open > 0)
                            {
                                this.transform.Translate(new Vector3(0, -speed, 0));
                            }
                        }
                        else
                        {
                            pos = 0.0f;
                            this.transform.position = posInit;
                            isMoving = false;
                            isOpen = false;
                        }
                        break;
                    case axisTrans.Z:
                        if (pos != 0)
                        {
                            if (open < 0)
                            {
                                this.transform.Translate(new Vector3(0, 0, speed));
                            }
                            if (open > 0)
                            {
                                this.transform.Translate(new Vector3(0, 0, -speed));
                            }
                        }
                        else
                        {
                            pos = 0.0f;
                            this.transform.position = posInit;
                            isMoving = false;
                            isOpen = false;
                        }
                        break;
                }
            }
            else
            {
                pos += speed;
                switch (axis)
                {
                    case axisTrans.X:
                        if (pos < Mathf.Abs(open))
                        {
                            if (open < 0)
                            {
                                this.transform.Translate(new Vector3(-speed, 0, 0));
                            }
                            if (open > 0)
                            {
                                this.transform.Translate(new Vector3(speed, 0, 0));
                            }
                        }
                        else
                        {
                            pos = Mathf.Abs(open);
                            this.transform.position = posInit + new Vector3(open, 0, 0);
                            isMoving = false;
                            isOpen = true;
                        }
                        break;
                    case axisTrans.Y:
                        if (pos < Mathf.Abs(open))
                        {
                            if (open < 0)
                            {
                                this.transform.Translate(new Vector3(0, -speed, 0));
                            }
                            if (open > 0)
                            {
                                this.transform.Translate(new Vector3(0, speed, 0));
                            }
                        }
                        else
                        {
                            pos = Mathf.Abs(open);
                            this.transform.position = posInit + new Vector3(0, open, 0);
                            isMoving = false;
                            isOpen = true;
                        }
                        break;
                    case axisTrans.Z:
                        if (pos < Mathf.Abs(open))
                        {
                            this.transform.Translate(new Vector3(0, 0, -speed));
                        }
                        else
                        {
                            pos = Mathf.Abs(open);
                            this.transform.position = posInit + new Vector3(0, 0, open);
                            isMoving = false;
                            isOpen = true;
                        }
                        break;
                }
            }
        }
    }
}
