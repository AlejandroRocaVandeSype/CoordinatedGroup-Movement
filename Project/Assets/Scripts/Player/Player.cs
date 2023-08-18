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


    UnitSelections _unitSelections;


    public void Start()
    {
        _unitSelections = GetComponent<UnitSelections>();
    }

    public void Update()
    {
       CheckMouseInput();
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButtonUp(MOUSE_RIGHT_CLICK))
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
                else
                {
                    _clickPosition = Vector3.zero;
                }

            }
        }

        // Unit Selection
        if (Input.GetMouseButtonUp(MOUSE_LEFT_CLICK))
        {          
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            Debug.Log("Entro");
            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                UnitCharacter unit = hitObject.GetComponent<UnitCharacter>();
                if (unit != null)
                {
                    // A unit was selected
                    _unitSelections.SelectSingleUnit(unit);
                }
                else
                {
                    // No unit selected
                    _unitSelections.DeselectAll();
                }
            }
        }

    }

    public Vector3 ClickPosition
    {
        get { return _clickPosition; }
    }
}
