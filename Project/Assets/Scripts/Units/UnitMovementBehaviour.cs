using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;


// MOVEMENT FOR ALL UNITS ( EITHER IN FORMATION OR NOT)
// Uses NavMeshAgent to make them move around the World
[RequireComponent(typeof(NavMeshAgent))]        // Mandatory Component 
public class UnitMovementBehaviour : MonoBehaviour
{
    private Vector3 _previousTarget = Vector3.zero;
    private Vector3 _target = Vector3.zero;          // Where should the character move

    private float _moveSpeed = 0f;
    private const float MAX_SPEED = 40f;
    private const float MIN_SPEED = 6f;
    private const float SPEED_MODIFIER = 4f;
    private const float MAX_ROTSPEED = 360.0f;
    private const float MIN_ROTSPEED = 15.0f;
    private float _acceleration = 100.0f;


    // Navegation through the World using Unity NavMesh
    // NavMesh uses A* internally
    private NavMeshAgent _navMeshAgent;

    public void Awake()
    {
        SetNavMeshAgent();       
    }

    public void FixedUpdate()
    {
        HandleMovement();
    }


    private void HandleMovement()
    {
        if (_navMeshAgent == null)
            return;
        if (_target == Vector3.zero)  // No target = No movement
            return;

        _navMeshAgent.SetDestination(_target);
        AdjustSpeed();
    }

    private void AdjustSpeed()
    {
        UnitCharacter unitCP = gameObject.GetComponent<UnitCharacter>();
        if (unitCP.IsSelected == true && unitCP.Formation == null && _target != Vector3.zero)
        {
            // If not in a formation he goes at his min speed
            _navMeshAgent.speed = MIN_SPEED;
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                _target = Vector3.zero;
        }
        else
        {
            // In formation -> Moves based on the distance 
            float calculatedSpeed = _navMeshAgent.remainingDistance * SPEED_MODIFIER;
            _moveSpeed = Mathf.Clamp(calculatedSpeed, MIN_SPEED, MAX_SPEED);

            _navMeshAgent.speed = _moveSpeed;
        }
       
    }
    public Vector3 Target
    {
        get { return _target; }
        set 
        {
            // Don't change the target value if it is still the same target as before or if its zero
            if (_previousTarget == Vector3.zero || _previousTarget != value)
            {
                _target = value;
                _previousTarget = value;
                return;
            } 
        }
    }

    // All units with movement will use the same values
    private void SetNavMeshAgent()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = _moveSpeed;
        _navMeshAgent.angularSpeed = MAX_ROTSPEED;
        _navMeshAgent.acceleration = _acceleration;
        _navMeshAgent.stoppingDistance = 0.3f;

    }


    public float AngularSpeed
    {
        get { return _navMeshAgent.angularSpeed; }
        set { _navMeshAgent.angularSpeed = value; }
    }

    public float MaxAngularSpeed
    {
        get { return MAX_ROTSPEED; }
    }

    public float MinAngularSpeed
    {
        get { return MIN_ROTSPEED; }
    }

    public float Speed
    {
        set { _navMeshAgent.speed = value; }
    }
}
