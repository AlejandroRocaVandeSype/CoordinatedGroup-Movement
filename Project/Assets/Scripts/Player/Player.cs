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

    // Units to select/deselect
    UnitSelections _unitSelections;

    // Dragging
    Vector2 _dragEndPos;
    Vector2 _dragStartPos;
    Rect _selectionRect;
    [SerializeField] RectTransform _visualDragging;


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

            // Reset the positions of the visual box for dragging mouse input
            bool resetDragPos = true;
            DrawDraggingVisual(resetDragPos);

            if (Physics.Raycast(ray, out hitInfo))
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                UnitCharacter unit = hitObject.GetComponent<UnitCharacter>();
                if (unit != null)
                {
                    // A unit was selected

                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        // Adding multiple units
                        _unitSelections.SelectMultipleUnits(unit);
                    }
                    else
                    {
                        // Single unit
                        _unitSelections.SelectSingleUnit(unit);
                    }
                 
                    
                }
                else
                {
                    if(!Input.GetKey(KeyCode.LeftShift))
                    {
                        // No unit selected and leftshit wasn't being pressed -> Deselect all of them
                        _unitSelections.Deselect();
                    }
                   
                }
            }
        }

        // Dragging
        if(Input.GetMouseButtonDown(MOUSE_LEFT_CLICK))
        {
            _dragStartPos= Input.mousePosition;
        }

        if(Input.GetMouseButton(MOUSE_LEFT_CLICK))
        {
            // Hold left mouse click
            _dragEndPos= Input.mousePosition;
            DrawDraggingVisual();
        }

    }

    public Vector3 ClickPosition
    {
        get { return _clickPosition; }
    }


    public void DrawDraggingVisual(bool reset = false)
    {
        if(reset)
        {
            _dragStartPos = Vector2.zero;
            _dragEndPos = Vector2.zero;
        }

        // Center of the box
        _visualDragging.position = (_dragStartPos + _dragEndPos) / 2;
        _visualDragging.sizeDelta = new Vector2(Mathf.Abs(_dragStartPos.x - _dragEndPos.x),
            Mathf.Abs(_dragStartPos.y - _dragEndPos.y));


    }
}
