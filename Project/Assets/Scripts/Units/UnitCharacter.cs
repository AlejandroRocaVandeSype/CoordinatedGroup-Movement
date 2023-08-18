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

    [SerializeField] private GameObject _selectionPrefab;
    private GameObject _selectionRing;

    public void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
    }

    public void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void Update()
    {
        if (_player != null)
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

            if(_selectionRing == null && _isSelected)
            {
                // Instantiate the prefab at the unit's position
                _selectionRing = Instantiate(_selectionPrefab, transform.position, _selectionPrefab.transform.rotation, transform.GetChild(0));
                _selectionRing.SetActive(value);
                Destroy(_selectionPrefab);          // Watch out with this
            }
            else
            {
                _selectionRing.SetActive(value);
            }
          
        }
    }


   
}
