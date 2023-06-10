using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("General Setup Settings")]
    [Tooltip("How fast ship moves up and down")]
    [SerializeField] float speed = 10f;
    [Tooltip("How far ship can move in screen x axis")]
    [SerializeField] float xRange = 10f;
    [Tooltip("How far ship can move in screen y axis")]
    [SerializeField] float yRange = 7f;

    [Header("Screen position based tuning")]
    [Tooltip("The factor for smooth position in pitch")]
    [SerializeField] float positionPitchFactor = 2f;
    [Tooltip("The factor for smooth position in yaw")]
    [SerializeField] float positionYawFactor = 2f;

    [Header("Player input based tuning")]
    [Tooltip("The factor for smooth control in pitch")]
    [SerializeField] float controlPitchFactor = 10f;
    [Tooltip("The factor for smooth control in roll")]
    [SerializeField] float controlRollFactor = 10f;

    [Header("Laser gun arrays")]
    [Tooltip("Laser Gameobjects in Player")]
    [SerializeField] GameObject[] lasers;

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
        ProcessFiring();
    }

    void ProcessMovement()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");

        Vector3 rawPose = transform.localPosition + new Vector3(xThrow * speed * Time.deltaTime, yThrow * speed * Time.deltaTime, 0);

        float clampedXPose = Mathf.Clamp(rawPose.x, -xRange, xRange);
        float clampedYPose = Mathf.Clamp(rawPose.y, -yRange, yRange);

        transform.localPosition = new Vector3(clampedXPose, clampedYPose,rawPose.z);
    }

    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * -positionPitchFactor;
        float pitchDueToControlThrow = yThrow * -controlPitchFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;

        float yaw = transform.localPosition.x * positionYawFactor;

        float rollhDueToControlThrow = xThrow * -controlRollFactor;

        float roll = rollhDueToControlThrow;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    void ProcessFiring() 
    {
        if (Input.GetButton("Fire1"))
        {
            SetLasersActivity(true);
        }
        else
        {
            SetLasersActivity(false);
        }
    }

    void SetLasersActivity(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;
            emissionModule.enabled = isActive;
        }
    }
}
