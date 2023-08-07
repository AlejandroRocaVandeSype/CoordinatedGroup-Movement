using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{

    private Vector3 _clickPosition = Vector3.zero;

    const int MOUSE_LEFT_CLICK = 0;
    const int MOUSE_RIGHT_CLICK = 1;

    public void Update()
    {
        if(Input.GetMouseButtonUp(MOUSE_LEFT_CLICK))
        {
            // Single click - Save the position where it was clicked
            // Use raycast to get the position where it hits on the ground
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                _clickPosition = hit.point;
            }     
        }
    }


    public Vector3 ClickPosition
    {
        get { return _clickPosition; }
    }
}
