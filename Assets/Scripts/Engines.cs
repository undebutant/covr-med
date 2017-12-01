using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engines : MonoBehaviour
{
    private Vector3 speed;

    public Vector3 Speed
    {
        get
        {
            return this.speed;
        }
        set
        {
            this.speed = value;
        }
    }


    void Update()
    {

        transform.Translate(speed * this.GetComponent<ObjectAvatar>().MaxSpeed * Time.deltaTime);


    }
}