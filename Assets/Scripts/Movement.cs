using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float tiltSpeed = 5f;            // Speed at which the unicycle tilts left/right
    public float maxForwardSpeed = 4f;      // Maximum speed of the unicycle when moving forward
    public float acceleration = 1f;         // Rate of acceleration
    public float deceleration = 1.5f;       // Rate of deceleration when no input
    public float tiltAmount = 30f;          // Maximum tilt angle of the unicycle (left/right)
    public float fallThreshold = 25f;       // Tilt angle at which the unicycle will fall
    public float forwardTiltThreshold = 90f; // Tilt angle at which the unicycle will fall
    public float decaySpeed = 3f;           // Speed at which the accumulator decays (for smoother control)
    public bool isFalling = false;          // Tracks if the unicycle is falling
    public float respawnDelay = 2f;         // Delay before respawning after falling
    public float AccumulationLimit = 50;
    public float windEffect = 0f;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;

    private bool isSweating = false;
    private float horizontalInput;
    private float verticalInput;
    private float currentTilt = 0f;
    private float forwardTilt = 0f;
    private float currentForwardSpeed = 0f; // The current forward speed, will change based on inertia
    private Vector3 startPosition;    
    private Vector3 RespawnPosition;
    [SerializeField]
    private bool inWindZone = false;
    [SerializeField]
    private float windzoneZ;
    void Start()
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, 0f);
    
    }
    void Update()
    {   
        // Debug.Log("windEffect: " + windEffect);
        if (isFalling) return;  // If the unicycle is falling, stop any further movement 

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0.5f)
        {
            // currentForwardSpeed = Mathf.Min(currentForwardSpeed + acceleration * Time.deltaTime, maxForwardSpeed);
            forwardTilt = Mathf.Lerp(forwardTilt, tiltAmount, Time.deltaTime * tiltSpeed);
        }
        else
        {
            // currentForwardSpeed = Mathf.Max(currentForwardSpeed - deceleration * Time.deltaTime, 0f);
            forwardTilt = Mathf.Lerp(forwardTilt, 0f, Time.deltaTime * tiltSpeed);
        }

        Debug.Log(forwardTilt);

        // Tilting 
        if (inWindZone)
        {
            currentTilt = Mathf.Lerp(currentTilt, horizontalInput * tiltAmount + windEffect, Time.deltaTime * tiltSpeed);
        }
        else
        {
            currentTilt = Mathf.Lerp(currentTilt, horizontalInput * tiltAmount, Time.deltaTime * tiltSpeed);
        }
        // Debug.Log(currentTilt);

        transform.rotation = Quaternion.Euler(-forwardTilt, 0, -currentTilt);  
        
        FallingCheck();

        Vector3 newPosition = transform.position + Vector3.forward * currentForwardSpeed * Time.deltaTime;
        newPosition.x = startPosition.x; // Keep x position constant
        newPosition.y = startPosition.y; // Keep y position constant
        // transform.position = newPosition;
    }

    void TriggerFall()
    {   
        virtualCamera.enabled = false;
        isFalling = true;
        if (inWindZone)
        {
            RespawnPosition = new Vector3(startPosition.x, startPosition.y, windzoneZ);
        }
        else
        {
            RespawnPosition = new Vector3(startPosition.x, startPosition.y, transform.position.z);
        }
        // sound effect
        // unable the camera movement 
        // Fall
        transform.Rotate(Vector3.forward, 90f);  
        Invoke("Respawn", respawnDelay);
    }

    void Respawn()
    {   
        // sound effect
        // Reset the rotation
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.position = RespawnPosition;

        // Reset movement-related variables
        currentForwardSpeed = 0f;
        
        isFalling = false; 
        virtualCamera.enabled = true;
    }


    void FallingCheck()
    {   
        // if (Mathf.Abs(currentTilt) > fallThreshold-5 && Mathf.Abs(currentTilt) < fallThreshold)
        // {   
        //     if (!isSweating)
        //     {
        //         isSweating = true;
        //     }
        //     return;
        // }
        // else if (Mathf.Abs(currentTilt) > fallThreshold)
        if (Mathf.Abs(currentTilt) > fallThreshold)
        {
            TriggerFall();
            return;
        }

        if (Mathf.Abs(forwardTilt) > forwardTiltThreshold)
        {
            TriggerFall();
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("WindZone") || !isFalling)
        {
            inWindZone = true;
            windzoneZ = transform.position.z -.01f;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("WindZone"))
        {
            inWindZone = false;
            windEffect = 0f;
        }
    }

    private void Sweating()
    {   
        while(isSweating)
        {
            // sweating effect
        }
    }
}
