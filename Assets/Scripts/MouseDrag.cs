using UnityEngine;
using System.Collections;

public class MouseDrag : MonoBehaviour
{

    /*  float distance = 10;

      void OnMouseDrag()
      {
          Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
          Vector3 objectPosition = Camera.main.ScreenToWorldPoint(mousePosition);

          transform.position = objectPosition;
      }*/

    Vector3 offset;
    Vector3 screenPoint;
    void OnMouseDown()
    {

        screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }

}
