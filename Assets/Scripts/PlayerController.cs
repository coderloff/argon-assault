using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*using UnityEngine.InputSystem;*/

public class PlayerController : MonoBehaviour
{
    /*[SerializeField] InputAction movement;*/
    [SerializeField] float speed = 10f;
    [SerializeField] float xRange = 10f;
    [SerializeField] float yRange = 7f;

    [SerializeField] float positionPitchFactor = 2f;
    [SerializeField] float controlPitchFactor = 10f;

    [SerializeField] float positionYawFactor = 2f;

    [SerializeField] float controlRollFactor = 10f;

    float xThrow, yThrow;

    /*void OnEnable()
    {
        movement.Enable();
    }

    void OnDisable()
    {
        movement.Disable();
    }*/

    void Update()
    {
        ProcessMovement();
        ProcessRotation();
    }

    void ProcessMovement()
    {
        /*float xThrow = movement.ReadValue<Vector2>().x;
        float yThrow = movement.ReadValue<Vector2>().y;*/
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        Vector3 rawPose = transform.localPosition + new Vector3(xThrow * speed * Time.deltaTime, yThrow * speed * Time.deltaTime, 0);

        float clampedXPose = Mathf.Clamp(rawPose.x, -xRange, xRange);
        float clampedYPose = Mathf.Clamp(rawPose.y, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPose, clampedYPose,rawPose.z);

        Debug.Log(xThrow);

        Debug.Log(yThrow);
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * -positionPitchFactor;
        float pitchDueToControlThrow = yThrow * -controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yawDueToPosition = transform.localPosition.x * -positionYawFactor;

        float yaw = yawDueToPosition;

        float rollhDueToControlThrow = xThrow * -controlRollFactor;

        float roll = rollhDueToControlThrow;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
}
