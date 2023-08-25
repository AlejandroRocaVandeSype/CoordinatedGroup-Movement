using System.Collections.Generic;
using UnityEngine;

public class FormationManager : MonoBehaviour
{
    [SerializeField] private GameObject _formationPrefab;
    private List<Formation> _formations;
    private int _nextFormationID;           // Each formation will have a different ID 

    void Awake()
    {
        _formations = new List<Formation>();       
    }

    public void CreateFormation(List<UnitCharacter> units)
    {
        if(_formations.Count == 0)
        {
            // Still no formations -> Create a new one
            _formations.Add(Instantiate(_formationPrefab, transform).GetComponent<Formation>());
            _formations[0].Create(_nextFormationID, units);
            ++_nextFormationID;
            return;
        }

        // There is at least one formation in the world
        // First check if the selected units are already in a formation

        List<int> formationIndexes = new List<int>();
        for(int formationIdx = 0; formationIdx < _formations.Count; ++formationIdx)
        {
            foreach (UnitCharacter unit in units)
            {
                if (_formations[formationIdx].Contains(unit))
                {
                    // This formation contains this unit already
                    formationIndexes.Add(_formations[formationIdx].ID);
                }
            }
        }
        
        if(formationIndexes.Count == 0)
        {
            // All units are not in a formation yet -> Create a new one
            _formations.Add(Instantiate(_formationPrefab, transform).GetComponent<Formation>());
            _formations[_formations.Count - 1].Create(_nextFormationID, units);
            ++_nextFormationID;
            return;
        }


        // There are at least some units that are already in a formation

        // First remove from other formations the units
        RemoveFromFormations(formationIndexes);

        // Group all units in a unique formation
        AddToFormation(formationIndexes[0], units);

    }

    private void AddToFormation(int formationID, List<UnitCharacter> unitsToAdd)
    {
        Formation foundFormation = _formations.Find(formation =>formation.ID == formationID);

        if (foundFormation != null)
            foundFormation.Add(unitsToAdd);
    }


    private void RemoveFromFormations(List<int> formationIndexes)
    {
        int notToRemoveID = formationIndexes[0];
        // First formation is ignored since we will group units there
        for (int index = 1; index < formationIndexes.Count; ++index)
        {
            int formationID = formationIndexes[index];
            if(formationID != notToRemoveID)
            {
                Formation foundFormation = _formations.Find(formation => formation.ID == formationID);
                if (foundFormation != null)
                {
                    // Remove all units from this formation
                    foundFormation.RemoveAll();
                    _formations.Remove(foundFormation);
                }
            }
          
        }
    }

    // Send the movement order 
    public void SendMovementOrder(Vector3 target)
    {
        foreach(var formation in _formations)
        {
            formation.MoveOrder(target);
        }
    }
}
