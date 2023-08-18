using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitCharacter : MonoBehaviour
{
    private Player _player;
    private MovementBehavior _movementBehavior;
    private Vector3 _goalPosition;                // Where does the Unit have to move

    private bool _isSelected;
    
    // GameObject with a simple image to represent unit selection
    private GameObject _selectionRing = null;
    private string UNIT_SELECTION = "Unit_Selection";

    public void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        // Get the gameObject with ring selection image in order to activa/deactivate with selection
        _selectionRing = transform.GetChild(0).Find(UNIT_SELECTION).gameObject;
        
    }

    public void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void Update()
    {
        if (_player != null && IsSelected)
        {
            _goalPosition = _player.ClickPosition;

            if ( _goalPosition != Vector3.zero && _movementBehavior != null)
            {
                _movementBehavior.Target = _goalPosition;
            }
                
        }

    }

    public bool IsSelected
    {
        get { return _isSelected; }
        set 
        { 
            _isSelected = value;
            if (_selectionRing != null)
                _selectionRing.SetActive(value);
          
        }
    }

   
   
}
