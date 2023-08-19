using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    private List<Formation> _formations;

    void Awake()
    {
        _formations = new List<Formation>();       
    }

    public void CreateFormation(List<UnitCharacter> units)
    {
        if(_formations.Count == 0)
        {
            // Still no formations -> Create a new one
            _formations.Add(new Formation());
            _formations[0].Create(units);
            return;
        }

        // There is at least one formation in the world
        // First check if the selected units are already in a formation

        List<int> _formationIndexes = new List<int>();
        for(int formationIdx = 0; formationIdx < _formations.Count; ++formationIdx)
        {
            foreach (UnitCharacter unit in units)
            {
                if (_formations[formationIdx].Contains(unit))
                {
                    // This formation contains this unit already
                    _formationIndexes.Add(formationIdx);
                }
            }
        }
        
        if(_formationIndexes.Count == 0)
        {
            // All units are not in a formation yet -> Create a new one
            _formations.Add(new Formation());
            _formations[_formations.Count - 1].Create(units);
            return;
        }

      
         // There are at least some units that are already in a formation
         // Group all units in a unique formation
         _formations[_formationIndexes[0]].Add(units);
        
        // Need to remove units from previous 

    }
}
