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

    // Component with the units to select/deselect
    UnitSelections _unitSelectionsCP;

    // Mouse click dragging
    Camera _myMainCamera;
    Vector2 _dragEndPos;
    Vector2 _dragStartPos;
    Rect _selectionRect;                                // To check if we are selecting units from the world
    [SerializeField] RectTransform _visualDragging;     // Visual representation of the rectangle selection


    public void Start()
    {
        _unitSelectionsCP = GetComponent<UnitSelections>();
        _myMainCamera = Camera.main;
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
                        _unitSelectionsCP.SelectMultipleUnits(unit);
                    }
                    else
                    {
                        // Single unit
                        _unitSelectionsCP.SelectSingleUnit(unit);
                    }
                 
                    
                }
                else
                {
                    if(!Input.GetKey(KeyCode.LeftShift))
                    {
                        // No unit selected and leftshit wasn't being pressed -> Deselect all of them
                        _unitSelectionsCP.Deselect();
                    }
                   
                }
            }

            // Now check if we selected some units with the mouse click dragging
            SelectUnitsDragging();
        }

        // Dragging
        if(Input.GetMouseButtonDown(MOUSE_LEFT_CLICK))
        {
            _dragStartPos= Input.mousePosition;

            _selectionRect = new Rect();
        }

        if(Input.GetMouseButton(MOUSE_LEFT_CLICK))
        {
            // Hold left mouse click
            _dragEndPos= Input.mousePosition;
            DrawDraggingVisual();
            CreateSelectionRect();
        }

    }

    private void DrawDraggingVisual(bool reset = false)
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

    private void CreateSelectionRect()
    {
        // X
        if(Input.mousePosition.x < _dragStartPos.x)
        {
            // dragging left
            _selectionRect.xMin = Input.mousePosition.x;
            _selectionRect.xMax = _dragStartPos.x;
        }
        else
        {
            // dragging right
            _selectionRect.xMin = _dragStartPos.x;
            _selectionRect.xMax = Input.mousePosition.x;
        }


        if(Input.mousePosition.y < _dragStartPos.y)
        {
            // Dragging down
            _selectionRect.yMin = Input.mousePosition.y;
            _selectionRect.yMax = _dragStartPos.y;
        }
        else
        {
            _selectionRect.yMin = _dragStartPos.y;
            _selectionRect.yMax = Input.mousePosition.y;
        }

    }

    // Get all the units from the scene and check if they have been selected by dragging the mouse
    private void SelectUnitsDragging()
    {
        List<UnitCharacter> unitsInScene = World.Instance.UnitsInScene;
        foreach (UnitCharacter unit in unitsInScene)
        {
            Vector3 unitPos = unit.transform.position;
   
            if(_selectionRect.Contains(_myMainCamera.WorldToScreenPoint(unitPos)))
            {
                _unitSelectionsCP.DragSelection(unit);
            }
        }
    }

    public Vector3 ClickPosition
    {
        get { return _clickPosition; }
    }

}
