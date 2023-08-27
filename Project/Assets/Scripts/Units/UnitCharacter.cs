using System.Drawing;
using UnityEngine;

public class UnitCharacter : MonoBehaviour
{
    private Player _player;
    private UnitMovementBehaviour _movementBehavior;
    private Vector3 _targetPosition;                // Where does the Unit have to move

    private bool _isSelected;
    private Formation _formation;                   // Info about the formation the unit is in
    
    // GameObject with a simple image to represent unit selection
    private GameObject _selectionRing = null;
    private string UNIT_SELECTION = "Unit_Selection";

    // To change the color of the unit when selected
    private MeshRenderer _meshRenderer;
    private UnityEngine.Color _originalColor;
    private UnityEngine.Color _formationColor;         // Color when unit in formation

    public void Awake()
    {
        _movementBehavior = GetComponent<UnitMovementBehaviour>();
        // Get the gameObject with ring selection image in order to activa/deactivate with selection
        _selectionRing = transform.GetChild(0).Find(UNIT_SELECTION).gameObject;
        
        _isSelected = false;

        // The child contains the visuals of the character
        _meshRenderer = gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
        _originalColor = _meshRenderer.material.color;
        _formationColor = UnityEngine.Color.red;
    }

    public void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void Update()
    {
        if (_targetPosition != Vector3.zero)
        {
            _movementBehavior.Target = _targetPosition;
        }

        if(_isSelected == true && _formation == null)
        {
            // If it is selected and not in a formation
            if(_player.ClickPosition != Vector3.zero) 
                _movementBehavior.Target = _player.ClickPosition;
        }
    }


    private void ChangeColor(bool inFormation)
    {

        if (_formation != null)
        {
            // Modify the material's color directly
            _meshRenderer.material.color = _formationColor;
        }
        else
        {
            _meshRenderer.material.color = _originalColor;
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

            if(_formation != null)
                _formation.IsSelected = value;        
        }
    }

    public Formation Formation
    {
        get { return _formation; }
        set
        {
            _formation = value;
            if (_meshRenderer != null)
            {
                ChangeColor(value);
            }
        }
    }

    public Vector3 Target
    {
        set { _targetPosition = value; }
    }
    public UnitMovementBehaviour MovementBehavior
    {
        get { return _movementBehavior; }
    }
   
}
