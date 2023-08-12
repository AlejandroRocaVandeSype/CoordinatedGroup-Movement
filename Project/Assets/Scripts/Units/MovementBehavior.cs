using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]        // Mandatory Component 
public class MovementBehavior : MonoBehaviour
{
    private Vector3 _previousTarget = Vector3.zero;
    private Vector3 _target = Vector3.zero;          // Where should the character move
                                             
    // Navegation through the World using Unity NavMesh
    // NavMesh uses A* internally
    private NavMeshAgent _navMeshAgent;

    public void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
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
        
        // Move to the target
        _navMeshAgent.SetDestination(_target);

    }

    public Vector3 Target
    {
        get { return _target; }
        set 
        {
            if(_previousTarget == Vector3.zero)
            {
                _target = value;
                _previousTarget = value;
                return;
            }
               
            // Don't change the target value if it is still the same target as before
            if (_previousTarget != value)
            {
                _target = value;
                _previousTarget = value;
            }
            
        }
    }
}
