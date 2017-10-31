using UnityEngine;
using System.Collections;


/// <summary>
///     Script to add in order to have a draggable object
/// </summary>
public class MouseDrag : MonoBehaviour {

    // The position of the cursor
    Vector3 screenPoint;

    // Taking in account the relative distance between the point clicked by the cursor, and the center of the object
    Vector3 offset;


    /// <summary>
    ///     Moves the object depending on the camera
    /// </summary>
    void OnMouseDown() {
        screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    /// <summary>
    ///     Allows the objects to be dragged using the mouse, teleporting it to the last known position of the cursor
    /// </summary>
    void OnMouseDrag() {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }
}
