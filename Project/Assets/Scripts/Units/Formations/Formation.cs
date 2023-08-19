using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation
{
    private List<UnitCharacter> _units;
    private Vector3 _leaderSlot;            // Center of the formation
    private List<Vector3> _groupSlots;      // Rest of the slots. Positioned based on the center slot (leader)

    private static int _formationID = 0;

    public void Create(List<UnitCharacter> units)
    {

        _formationID++;
    }

    public void Add(List<UnitCharacter> units)
    {
        // Check to not add repeated units
    }

    public void Remove(List<UnitCharacter> units)
    {

    }

    public bool Contains(UnitCharacter unit)
    {
        if(_units.Count != 0)
        {
            return _units.Contains(unit);
        }

        return false;
    }

    public static int ID
    {
        get { return _formationID; }
    }

}
