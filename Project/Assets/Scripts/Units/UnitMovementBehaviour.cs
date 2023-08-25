using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// MOVEMENT FOR ALL UNITS ( EITHER IN FORMATION OR NOT)
// Uses NavMeshAgent to make them move around the World
[RequireComponent(typeof(NavMeshAgent))]        // Mandatory Component 
public class UnitMovementBehaviour : MonoBehaviour
{
    private Vector3 _previousTarget = Vector3.zero;
    private Vector3 _target = Vector3.zero;          // Where should the character move

    private float _moveSpeed = 10.0f;
    private const float MAX_ROTSPEED = 360.0f;
    private const float MIN_ROTSPEED = 15.0f;
    private float _acceleration = 30.0f;


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

        //if(_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        //{

        //    // Disable automatic rotation control by the NavMeshAgent
        //    _navMeshAgent.updateRotation = false;

        //    // Calculate the direction from the current position to the target position
        //   // Vector3 directionToTarget = _targetPosition - transform.position;

        //    // Make the GameObject look at the target position
        //    transform.rotation = Quaternion.LookRotation(_target);

        //    // Re-enable automatic rotation control by the NavMeshAgent
        //    _navMeshAgent.updateRotation = true;


        //}
    }


    private void HandleMovement()
    {
       if (_navMeshAgent == null)
            return;
        if (_target == Vector3.zero)  // No target = No movement
            return;

        _navMeshAgent.SetDestination(_target);
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
}
