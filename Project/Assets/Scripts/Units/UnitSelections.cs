using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    private List<UnitCharacter> _unitsSelected;
    void Awake()
    {
        _unitsSelected= new List<UnitCharacter>();
    }

   
    // Deselect units
    // If unit == null means that all units need to be deselected
    // otherwise deselect the specified unit
    public void Deselect(UnitCharacter unit = null)
    {
        if(unit != null)
        {
            unit.IsSelected = false;
            _unitsSelected.Remove(unit);
        }
        else
        {
            // If no unit was provided -> Deselect all units
            foreach (UnitCharacter c in _unitsSelected)
            {
                if(c != null)
                {
                    c.IsSelected = false;
                }               
            }
            _unitsSelected.Clear();
        }
       
    }

    public void SelectSingleUnit(UnitCharacter unit)
    {
        Deselect();
        AddNewUnit(unit);
    }

    public void SelectMultipleUnits(UnitCharacter unit)
    {
        if (_unitsSelected.Contains(unit) == false)
        {
            // Add a new unit without removing the current one
            AddNewUnit(unit);
        }
        else
        {
            // If it is repeated then we just deselect it
            Deselect(unit);
        }
    }

    // Selection of units when player drag the mouse click
    public void DragSelection(UnitCharacter unit)
    {
        if (_unitsSelected.Contains(unit) == false)
        {
            AddNewUnit(unit);
        }
    }

    private void AddNewUnit(UnitCharacter unit)
    {
        _unitsSelected.Add(unit);
        unit.IsSelected = true;
    }


    public List<UnitCharacter> UnitsSelected
    {
        get { return _unitsSelected; }
    }

}
