using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
#region Singleton
    public static World Instance
    {
        get
        {
            if (_instance == null && !_isAppQuiting)
            {
                _instance = FindObjectOfType<World>();
                if (_instance == null)
                {
                    // No Instance found of this class : Create a new one
                    GameObject NewGameManager = new GameObject("Singleton_GameManager");
                    _instance = NewGameManager.AddComponent<World>();
                }
            }

            return _instance;
        }
    }

    public void OnApplicationQuit()
    {
        _isAppQuiting = true;
    }
    private static World _instance;
    private static bool _isAppQuiting = false;

    #endregion


    List<UnitCharacter> _allUnits;
    
    void Start()
    {
        AddAllUnitsFromScene();
    }

    void AddAllUnitsFromScene()
    {

        GameObject unitsGO = gameObject.transform.Find("Units").gameObject;

        UnitCharacter[] unitsFound = unitsGO.GetComponentsInChildren<UnitCharacter>();

        if (unitsFound.Length > 0)
        {
            _allUnits = new List<UnitCharacter>(unitsFound.Length);

            foreach (UnitCharacter unit in unitsFound)
            {
                _allUnits.Add(unit);
            }
        }
    }


    public List<UnitCharacter> UnitsInScene
    {
        get { return _allUnits; }
    }
}
