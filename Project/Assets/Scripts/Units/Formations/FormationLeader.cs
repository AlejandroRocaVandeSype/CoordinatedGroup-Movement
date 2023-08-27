using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class FormationLeader : MonoBehaviour
{
    LeaderMovementBehaviour _leaderMovement;         // Handle everything related with leader's movement

    [SerializeField] GameObject _leaderSlotVisual;      // For debug purposes to see where are the positions located in the world
    [SerializeField] GameObject _unitsSlotsVisual;
    private List<Transform> _unitsTransforms;           // Where normal units need to move
    private float _spacing;                             // Space between units
    
    private Formation _formation;

    public void Start()
    {
        _formation = gameObject.transform.parent.GetComponent<Formation>();
        _unitsTransforms = new List<Transform>(_formation.Units.Count);
        _spacing = 2f;

        _leaderMovement = GetComponent<LeaderMovementBehaviour>();
        DefineUnitPositions();
    }

    // Define where the units will be positioned based on the virtual leader position
    public void DefineUnitPositions()
    {
        // Define the positions for all the units relative to center (leader position)
        int totalRows = 4;
        int totalUnitsPerRow = 5;
        int totalUnits = _formation.Units.Count;
        int unitsPositioned = 0;

        // Start positioning the units at the left side of the center (2 units to the left)
        Vector3 leftOffset = -Vector3.right * _spacing;
        Vector3 startPos = transform.position + (2 * leftOffset);

        // Calculate all positions in the formation
        for(int rowIdx = 0; rowIdx < totalRows; ++rowIdx)
        {
            for(int colIdx = 0; colIdx < totalUnitsPerRow; ++colIdx)
            {
                if(unitsPositioned < totalUnits)
                {
                    // Still units to add
                    GameObject unitGO = Instantiate(_unitsSlotsVisual, transform);

                    Vector3 unitPosition = startPos + (rowIdx * _spacing * Vector3.back) + (colIdx * _spacing * Vector3.right);
                    unitGO.transform.position = unitPosition;
                    _unitsTransforms.Add(unitGO.transform);

                    ++unitsPositioned;
                }
            }
        }
    }

    public void FixedUpdate()
    {
        if(_leaderMovement != null )   // Handle leader's movement
        {          
            _leaderMovement.HandleMovement();
        }
           
        // Make the units move to their positions in the formation
        List<UnitCharacter> units = _formation.Units;
        for (int unitIdx = 0; unitIdx < units.Count; ++unitIdx)
        {
            units[unitIdx].Target = _unitsTransforms[unitIdx].position;
        }
       
    }

    public Vector3 TargetPosition
    {
        get { return _leaderMovement.TargetPosition; }
        set { _leaderMovement.TargetPosition = value; }
    }

}
