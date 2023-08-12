using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitCharacter : MonoBehaviour
{
    private Player _player;

    private MovementBehavior _movementBehavior;
    private Vector3 _goalPosition;                // Where does the Unit have to move

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
                _movementBehavior.Target = _goalPosition;
        }
           

       
    }
}
