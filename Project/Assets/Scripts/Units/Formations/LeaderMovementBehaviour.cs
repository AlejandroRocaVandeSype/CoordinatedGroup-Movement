using UnityEngine;
using UnityEngine.AI;

// MOVEMENT FOR THE VIRTUAL LEADER IN THE FORMATION

public class LeaderMovementBehaviour : MonoBehaviour
{
    // Simple struct to store info about the TARGET
    private struct Target
    {
        public Vector3 position;                    // Where does he have to go
        public Vector3 direction;                 // In which direction is the target
    }

    // Allow us to have more control over the process of calculating the path
    private NavMeshPath _pathToTarget;
    private Target _target;
    private bool _isMoving = false;
    private float _speed = 0f;
    private float _maxSpeed = 10f;
    private float _stopDistance;

    private Rigidbody _rigidBody;                       // To control the angular speed of the leader
    private Vector3 _currentPos;


    // Start is called before the first frame update
    void Start()
    {
        _pathToTarget = new NavMeshPath();
        _target.position = transform.position;       // At start the target is the position where it is located
        _target.direction = transform.position;
        _stopDistance = 1f;

        _rigidBody = GetComponent<Rigidbody>();
    }

   public void HandleMovement()
    {
        // Leader's current position
        _currentPos = transform.position;

        // 1º Check if we have arrive at the target
        float distanceToTarget = Vector3.Distance(_target.position, _currentPos);
        if (distanceToTarget < _stopDistance)
        {
            // Target reached -> STOP MOVEMENT/ROTATION
            StopMovement();
        }
        else
        {
            CalculatePath();
        }

        // Move leader to the target if there is any and if he hasn't reached yet
        if (_isMoving)
        {
            MoveToTarget();
        }
    }

    private void CalculatePath()
    {
        // Calculate the path from current's leader position to the target and store it 
        // in our path variable
        if (NavMesh.CalculatePath(_currentPos, _target.position, NavMesh.AllAreas, _pathToTarget))
        {
            // Path was found
            // The next corner in the path will determine in which direction the it has to go
            // End position - start position = vector towards end position 
            _target.direction = (_pathToTarget.corners[1] - _currentPos).normalized;
            _speed = _maxSpeed;
            if (_maxSpeed != 0f)
                _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }


    private void MoveToTarget()
    {
        // Move towards the indicated direction
        // Aline the the forward direction of the leader with the target
        // this way it correctly rotates towards the target
        _rigidBody.angularVelocity = new Vector3(0f, Vector3.Cross(_target.direction, transform.forward).y, 0f) * -10f;

        // Update position using speed
        transform.position += (_speed * transform.forward * Time.fixedDeltaTime);
    }
    private void StopMovement()
    {
        _speed = 0f;
        _rigidBody.angularVelocity = Vector3.zero;
        _isMoving = false;
    }

    public Vector3 TargetPosition
    {
        get { return _target.position; }
        set { _target.position = value; }
    }
}
