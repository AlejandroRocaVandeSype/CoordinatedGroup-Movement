using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    World _world;
    FormationManager _formationManager;
    private Vector3 _clickPosition = Vector3.zero;

    const int MOUSE_LEFT_CLICK = 0;
    const int MOUSE_RIGHT_CLICK = 1;

    const string GROUND_LAYER = "Ground";
    const string OBSTACLE_LAYER = "Obstacle";

    // Component with the units to select/deselect
    UnitSelections _unitSelectionsCP;

    // Mouse click dragging
    Vector2 _dragEndPos;
    Vector2 _dragStartPos;
    Rect _selectionRect;                                // To check if we are selecting units from the world
    [SerializeField] RectTransform _visualDragging;     // Visual representation of the rectangle selection


    public void Start()
    {
        _unitSelectionsCP = GetComponent<UnitSelections>();
        _world = World.Instance;
        _formationManager = _world.FormationManager;
    }

    public void Update()
    {
       PlayerInput();
    }

    private void PlayerInput()
    {
        // Units Movement
        if (Input.GetMouseButtonUp(MOUSE_RIGHT_CLICK))
        {
            MoveUnits();
        }

        // Units Selection
        if (Input.GetMouseButtonUp(MOUSE_LEFT_CLICK))
        {
            SelectUnits();
        }

        // START OF MOUSE CLICK DRAGGING
        if(Input.GetMouseButtonDown(MOUSE_LEFT_CLICK))
        {
            _dragStartPos= Input.mousePosition;
            _selectionRect = new Rect();
        }
        // MOUSE CLICK HOLD -> Draw a visual box
        if(Input.GetMouseButton(MOUSE_LEFT_CLICK))
        {
            // Hold left mouse click
            _dragEndPos= Input.mousePosition;
            DrawDraggingVisual();
            CreateSelectionRect();
        }

        // CTRL + G -> Try to create a formation based on the units selected
        if(Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.G))
        {
            // Check if enough units are selected
            if(_unitSelectionsCP.UnitsSelected.Count > 1)
                _world.FormationManager.CreateFormation(_unitSelectionsCP.UnitsSelected);
        }

    }

    //      ******* UNITS MOVEMENT *********
    // REGISTER WHERE THE PLAYER CLICKED WITH THE MOUSE
    private void MoveUnits()
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
                StartCoroutine(ResetClickPos());
            }
            else
            {
                _clickPosition = Vector3.zero;
            }

            // Send the movement order to the selected formation
            _formationManager.SendMovementOrder(_clickPosition);
        }
    }

    //               ****** UNITS SELECTION ******
    // WITH LEFT SHIT -> Allows Multiple selection (or deselection if unit already selected)
    // WITHOUT LEFT SHIFT -> Single unit selection
    private void SelectUnits()
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
                // NO UNIT WAS SELECTED
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    // If LEFT SHIFT wasn't being pressed -> Deselect all of them
                    _unitSelectionsCP.Deselect();
                }

            }
        }

        // When button is up we also check if a unit was selected with the visual dragging box
        DraggingSelectionUnits();
    }

    private void DrawDraggingVisual(bool reset = false)
    {
        if(reset)
        {
            // Reset box visual
            _dragStartPos = Vector2.zero;
            _dragEndPos = Vector2.zero;
        }

        // Calculate the positions of the box based on the mouse positions
        _visualDragging.position = (_dragStartPos + _dragEndPos) / 2;
        _visualDragging.sizeDelta = new Vector2(Mathf.Abs(_dragStartPos.x - _dragEndPos.x),
            Mathf.Abs(_dragStartPos.y - _dragEndPos.y));

    }

    // Set the rectangle positions in order to check later if any unit was inside this
    // selection
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

        // Y
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
    private void DraggingSelectionUnits()
    {
        List<UnitCharacter> unitsInScene = _world.UnitsInScene;
        foreach (UnitCharacter unit in unitsInScene)
        {
            Vector3 unitPos = unit.transform.position;
   
            if(_selectionRect.Contains(Camera.main.WorldToScreenPoint(unitPos)))
            {
                _unitSelectionsCP.DragSelection(unit);
            }
        }
    }

    private IEnumerator ResetClickPos()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        _clickPosition = Vector3.zero;
    }

    public Vector3 ClickPosition
    {
        get { return _clickPosition; }
    }

}
