using System.Drawing;
using UnityEngine;

public class UnitCharacter : MonoBehaviour
{
    private Player _player;
    private MovementBehavior _movementBehavior;
    private Vector3 _goalPosition;                // Where does the Unit have to move

    private bool _isSelected;
    private bool _inFormation;
    
    // GameObject with a simple image to represent unit selection
    private GameObject _selectionRing = null;
    private string UNIT_SELECTION = "Unit_Selection";

    // To change the color of the unit when selected
    private MeshRenderer _meshRenderer;
    private UnityEngine.Color _unitSelectedColor = UnityEngine.Color.red;         // Selected color

    public void Awake()
    {
        _movementBehavior = GetComponent<MovementBehavior>();
        // Get the gameObject with ring selection image in order to activa/deactivate with selection
        _selectionRing = transform.GetChild(0).Find(UNIT_SELECTION).gameObject;
        
        _isSelected = false;
        _inFormation = false;

        // The child contains the visuals of the character
        _meshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
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

    public bool InFormation
    {
        get { return _inFormation; }
        set
        {
            _inFormation = value;
            if (_meshRenderer != null)
            {
                // Modify the material's color directly
                _meshRenderer.material.color = _unitSelectedColor;
            }
        }
    }
   
}
