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
        Vector3 leftOffset = -Vector3.right * _spacing;
        Vector3 rightOffset = Vector3.right * _spacing;

        for (int i = 0; i < _formation.Units.Count; i++)
        {
            GameObject unitGO = Instantiate(_unitsSlotsVisual, transform);

            // Calculate local position based on the leader's position and orientation
            Vector3 localPosition = Vector3.zero;

            if (i == 0) localPosition = leftOffset * 2;
            else if (i == 1) localPosition = leftOffset * 1;
            else if (i == 2) localPosition = rightOffset * 1;
            else if (i == 3) localPosition = rightOffset * 2;

            unitGO.transform.localPosition = localPosition;

            _unitsTransforms.Add(unitGO.transform); // Add the Transform component to the list
        }

    }

    public void FixedUpdate()
    {
        if(_leaderMovement != null )   // Handle leader's movement
            _leaderMovement.HandleMovement();

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
