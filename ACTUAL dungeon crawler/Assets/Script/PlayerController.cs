using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool smoothTransition = false;
    public float transitionSpeed = 3f;
    public float transitionRotationSpeed = 500f;

    public float range = 2;

    private float minStopDistance = 0.05f;
    private float rotationAngle = 90f;
    private Vector3 moveVectorUp = new Vector3(0, 0.1f, 0);

    Vector3 targetGridPos;
    Vector3 prevTargetGridPos;
    Vector3 targetRotation;

    private void Start()
    {
        targetGridPos = Vector3Int.RoundToInt(transform.position);
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(horizontalInput < 0)
        {
            MoveLeft();
        }
        if (horizontalInput > 0)
        {
            MoveRight();
        }
        if(verticalInput < 0)
        {
            MoveBackward();
        }
        if (verticalInput > 0)
        {
            MoveForward();
        }
        if (Input.GetButtonUp("Rotate left"))
        {
            RotateLeft();
        }
        if (Input.GetButtonUp("Rotate right"))
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
        if (true) //Platzhalter
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
        else
        {
            targetGridPos = prevTargetGridPos;
        }

    }

    public bool IsWall(Vector3 intention)
    {
        Debug.Log("CHECKING IF WALL!!!!!!!!!");

        float rayLength = 1.0f;

        Debug.DrawRay(transform.position, intention * rayLength, Color.red, 1.0f);

        RaycastHit hitWall;
        bool isWall = false;

        if (Physics.Raycast(transform.position+moveVectorUp, intention, out hitWall, rayLength))
        {
            Debug.Log("Hitting something at distance: " + hitWall.distance);
            isWall = hitWall.collider.gameObject.CompareTag("Wall");
            if (isWall)
            {
                Debug.Log("THERE IS WALL");
            }
            else
            {
                Debug.Log("Hit something, but it's not a wall. It's a " + hitWall.collider.gameObject.tag);
            }
        }
        else
        {
            Debug.Log("Nothing hit by the ray.");
        }

        return isWall;
    }

    public void RotateLeft() { if (AtRest) targetRotation -= Vector3.up * rotationAngle; }
    public void RotateRight() { if (AtRest) targetRotation += Vector3.up * rotationAngle; }

    public void MoveForward()
    {
        if (AtRest && !IsWall(transform.forward + moveVectorUp)) targetGridPos += transform.forward;
    }
    public void MoveBackward()
    {
        if (AtRest && !IsWall(-transform.forward + moveVectorUp)) targetGridPos -= transform.forward;
    }
    public void MoveLeft()
    {
        if (AtRest && !IsWall(-transform.right + moveVectorUp)) targetGridPos -= transform.right;
    }
    public void MoveRight()
    {
        if (AtRest && !IsWall(transform.right + moveVectorUp)) targetGridPos += transform.right;
    }



    bool AtRest
    {
        get
        {
            //At rest when stopped moving or rotating, or when about to stop (distance small enough)
            if ((Vector3.Distance(transform.position, targetGridPos) < minStopDistance) &&
                (Vector3.Distance(transform.eulerAngles, targetRotation) < minStopDistance))
                return true;
            else
                return false;
        }
    }
}
