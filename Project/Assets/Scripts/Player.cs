using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{

    private Vector3 _clickPosition = Vector3.zero;

    const int MOUSE_LEFT_CLICK = 0;
    const int MOUSE_RIGHT_CLICK = 1;


    const string GROUND_LAYER = "Ground";
    const string OBSTACLE_LAYER = "Obstacle";


    public void Update()
    {
       CheckMouseInput();
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButtonUp(MOUSE_LEFT_CLICK))
        {
            // Single click - Save the position where it was clicked
            // Use raycast to get the position where it hits on the ground

            // Ray from the Camera to mousePosition
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, LayerMask.GetMask(GROUND_LAYER)))
            {
                // Only if the hit collider is not an Obstacle then we save the clicked position
                if (hitInfo.collider.gameObject.layer != LayerMask.NameToLayer(OBSTACLE_LAYER))
                {
                    _clickPosition = hitInfo.point;
                }
            }
        }
    }

    public Vector3 ClickPosition
    {
        get { return _clickPosition; }
    }
}
