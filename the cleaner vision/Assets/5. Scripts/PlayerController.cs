using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool smoothTransition = false;
    public float transitionSpeed = 3f;
    public float transitionRotationSpeed = 500f;

    private float minStopDistance = 0.05f;
    private float rotationAngle = 90f;
    private float verticalInput;
    private Vector3 moveVectorUp = new Vector3(0, 0.1f, 0);

    private bool isVerticalInUse = false;
    private bool lookingUp = false;

    Vector3 targetGridPos;
    Vector3 prevTargetGridPos;
    Vector3 targetRotation;

    private void Start()
    {
        // Make sure player starts position in 0.5 grid
        float posX = transform.position.x;
        float posZ = transform.position.z;
        if (posX % 1 != 0.5)
        {
            posX = posX < 0 ? (int)posX - 0.5f : (int)posX + 0.5f;
        }
        if (posZ % 1 != 0.5)
        {
            posZ = posZ < 0 ? (int)posZ - 0.5f : (int)posZ + 0.5f; 
        }
        targetGridPos = new Vector3(posX, transform.position.y, posZ);
        targetRotation = transform.rotation.eulerAngles;
    }
    private void Update()
    {
        SetInput();

        if (verticalInput < 0)
        {
            if (!isVerticalInUse)
            {
                if (lookingUp && AtRest)
                {
                    Debug.Log("looking down!!!");
                    RotateDown();
                    lookingUp = false;
                    isVerticalInUse = true;
                }
                else if (AtRest)
                {
                    Debug.Log("looking up!!!");
                    RotateUp();
                    lookingUp = true;
                    isVerticalInUse = true;
                }
            }
        }
        if (verticalInput > 0 && AtRest)
        {
            if (!isVerticalInUse)
            {
                MoveForward();
                isVerticalInUse = true;
            }
        }
        if (verticalInput == 0)
        {
            isVerticalInUse = false;
        }

        if (AtRest && Input.GetButtonDown("Look Left") && !lookingUp)
        {
            RotateLeft();
        }
        if (AtRest && Input.GetButtonDown("Look Right") && !lookingUp)
        {
            RotateRight();
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        prevTargetGridPos = targetGridPos;

        Vector3 targetPosition = targetGridPos;

        //avoid negative or >360 degrees
        if (targetRotation.y > 270f && targetRotation.y < 361f) targetRotation.y = 0f;
        if (targetRotation.y < 0f) targetRotation.y = 270f;

        if (!smoothTransition)
        {
            transform.position = targetPosition;
            transform.rotation = Quaternion.Euler(targetRotation);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * transitionSpeed);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * transitionRotationSpeed);
        }

    }

    private void SetInput()
    {
        if (GameControllers.connected)
        {
            Debug.Log("using controller!!!");
            verticalInput = Input.GetAxisRaw("Vertical DPAD");
        }
        else
        {
            verticalInput = Input.GetAxisRaw("Vertical");
        }
    }


    public bool IsWall(Vector3 intention)
    {

        float rayLength = 1.0f;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        Debug.DrawRay(origin, intention * rayLength, Color.red, 1.0f);

        RaycastHit hitWall;
        bool isWall = false;

        if (Physics.Raycast(transform.position + moveVectorUp, intention, out hitWall, rayLength))
        {
            if (hitWall.collider.gameObject.CompareTag("Wall"))
            {
                Debug.Log("HIT A WALL FR!!!!!!.");
                isWall = true;
            }
        }
        else
        {
            Debug.Log("Nothing hit by the ray.");
        }

        return isWall;
    }

    public void RotateLeft() { targetRotation -= Vector3.up * rotationAngle; }
    public void RotateRight() { targetRotation += Vector3.up * rotationAngle; }
    public void RotateUp() { targetRotation += Vector3.left * rotationAngle; }
    public void RotateDown() { targetRotation += Vector3.right * rotationAngle; }

    public void MoveForward()
    {
        if (!IsWall(transform.forward + moveVectorUp)) targetGridPos += transform.forward;
    }
    public void MoveBackward()
    {
        if (!IsWall(-transform.forward + moveVectorUp)) targetGridPos -= transform.forward;
    }

    private bool AtRest
    {
        get
        {
            //At rest when stopped moving or rotating, or when about to stop (distance small enough)
            if ((Vector3.Distance(transform.position, targetGridPos) < minStopDistance) &&
                (Quaternion.Angle(transform.rotation, Quaternion.Euler(targetRotation.x, targetRotation.y, targetRotation.z))
                < minStopDistance)) //TODO: fix atrest for fuck sake SHIT IS FIXED !!! 
                return true;
            else
                return false;
        }
    }

}
