using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation
{
    private List<UnitCharacter> _unitsInFormation;
    private Vector3 _leaderSlot;            // Center of the formation
    private List<Vector3> _groupSlots;      // Rest of the slots. Positioned based on the center slot (leader)

    private int _formationID = 0;

    public void Create(int formationID, List<UnitCharacter> unitsToAdd)
    {
        _formationID = formationID;
        _unitsInFormation = new List<UnitCharacter>(unitsToAdd.Count);
        Add(unitsToAdd);
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

    public void Remove(UnitCharacter unitToRemove)
    {
        _unitsInFormation.Remove(unitToRemove);
    }

    public void RemoveAll()
    {
        foreach(UnitCharacter unit in _unitsInFormation)
        {
            unit.InFormation = false;
        }
        _unitsInFormation.Clear();
    }

    public bool Contains(UnitCharacter unit)
    {
        if(_unitsInFormation.Count != 0)
        {
            return _unitsInFormation.Contains(unit);
        }

        return false;
    }

    public int ID
    {
        get { return _formationID; }
    }

}
