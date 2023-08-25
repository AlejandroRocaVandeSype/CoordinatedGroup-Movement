using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private float _cameraSpeed = 30.0f;

    private void FixedUpdate()
    {
        float verticalMovement = Input.GetAxis("VerticalMov");
        float horizontalMovement = Input.GetAxis("HorizontalMov");

        Vector3 movement = horizontalMovement * Vector3.right + verticalMovement * Vector3.forward;
        movement = movement.normalized;
        movement *= _cameraSpeed * Time.deltaTime;


        transform.position += movement;
   
    }
}
