using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    List<UnitCharacter> _unitsSelected;
    void Awake()
    {
        _unitsSelected= new List<UnitCharacter>();
    }

   
    public void DeselectAll()
    {
        foreach (UnitCharacter c in _unitsSelected)
        {
            c.IsSelected = false;
        }

        _unitsSelected.Clear();
    }

    public void SelectSingleUnit(UnitCharacter unit)
    {
        if(!IsRepeated(unit))
        {
            DeselectAll();
            AddNewUnit(unit);
        }
       
    }

    public void SelectMultipleUnits(UnitCharacter unit)
    {
        if (!IsRepeated(unit))
        {
            // Add a new unit without removing the current one
            AddNewUnit(unit);
        }
    }


    private void AddNewUnit(UnitCharacter unit)
    {
        _unitsSelected.Add(unit);
        unit.IsSelected = true;
    }


    private bool IsRepeated(UnitCharacter newUnit)
    {
        UnitCharacter unitFound = _unitsSelected.Find(unit => unit.GetInstanceID() == newUnit.GetInstanceID());

        if (unitFound != null)
            return true;

        return false;
    }
}
