using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Formation : MonoBehaviour
{

    private int _formationID = 0;
    private List<UnitCharacter> _unitsInFormation;

    [SerializeField] private GameObject _leaderPrefab;
    private GameObject _leaderUnit;                  // Invisible unit that serves as the leader for the formation
                                                     // Placed in the center of the formation
    private FormationLeader _leaderCP;

    // *****************************************++++++++
    //              NEW FORMATION
    // Give it an ID and add all units to this formation
    // Select a leader for this formation. Rest of units
    // will be normal units
    // *****************************************+++++++++
    public void Create(int formationID, List<UnitCharacter> unitsToAdd)
    {
        _formationID = formationID;
        _unitsInFormation = new List<UnitCharacter>(unitsToAdd.Count);
        Add(unitsToAdd);
        CreateLeader();
    }

    public void Add(List<UnitCharacter> unitsToAdd)
    {
        foreach(UnitCharacter newUnit in unitsToAdd)
        {
            // Don't add if unit is already in the formation
            if (Contains(newUnit))
                continue;

            // Not repeated
            _unitsInFormation.Add(newUnit);
            newUnit.InFormation = true;
        }
       
    }

    private void CreateLeader()
    {
        // Set the virtual leader at the center of the formation
         _leaderUnit = Instantiate(_leaderPrefab, CalculateCenter(), Quaternion.identity);
        _leaderUnit.transform.SetParent(transform, true);
        _leaderCP = _leaderUnit.GetComponent<FormationLeader>();
    }

    public void Remove(UnitCharacter unitToRemove)
    {
        if (unitToRemove == _leaderUnit)
            _leaderUnit = null;

        _unitsInFormation.Remove(unitToRemove);
    }

    public void RemoveAll()
    {
        foreach(UnitCharacter unit in _unitsInFormation)
        {
            unit.InFormation = false;
        }
        _unitsInFormation.Clear();
        _leaderUnit = null;
    }

    public bool Contains(UnitCharacter unit)
    {
        if(_unitsInFormation.Count != 0)
        {
            return _unitsInFormation.Contains(unit);
        }

        return false;
    }


    public void MoveOrder(Vector3 target)
    {
        // Leader receives the order of where to move
       if(_leaderCP != null)
        {
           _leaderCP.TargetPosition = target;
        }
    }

    // Calculate the center of the formation
    private Vector3 CalculateCenter()
    {
        Vector3 center = new Vector3();
        foreach (UnitCharacter unit in _unitsInFormation)
        {
            center += unit.transform.position;
        }
        center /= _unitsInFormation.Count;

        return center;
    }

    public int ID
    {
        get { return _formationID; }
    }


    public List<UnitCharacter> Units
    {
        get { return _unitsInFormation; }
    }

}
