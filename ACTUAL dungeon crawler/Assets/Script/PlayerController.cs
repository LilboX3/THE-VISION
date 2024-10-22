using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool smoothTransition = false;
    public float transitionSpeed = 3f;
    public float transitionRotationSpeed = 500f;
    public EquipmentBase[] items = new EquipmentBase[3];

    public Slider healthBar;
    public Image healthImage;
    public float healthAmount = 7f;
    public Slider manaBar;
    public Image manaImage;
    public float manaAmount = 7f;

    public ElementType element;
    public int elementStat;

    private float minStopDistance = 0.05f;
    private float rotationAngle = 90f;
    //private float horizontalInput;
    private float verticalInput;
    private Vector3 moveVectorUp = new Vector3(0, 0.1f, 0);

    //private bool isHorizontalInUse = false;
    private bool isVerticalInUse = false;
    private bool isInCombat = false;
    private bool lookingUp = false;

    Vector3 targetGridPos;
    Vector3 prevTargetGridPos;
    Vector3 targetRotation;

    List<EquipmentBase> equipment;

    List<EquipmentBase> activeEquipment;

    private void Start()
    {
        int targetX = (int)transform.position.x;
        int targetZ = (int)transform.position.z;
        targetGridPos = new Vector3(targetX, transform.position.y, targetZ);
        equipment = new List<EquipmentBase>();
        activeEquipment = new List<EquipmentBase>();
    }
    private void Update()
    {
        if (!isInCombat)
        {
            SetInput();

            /*if (horizontalInput < 0)
            {
                if (!isHorizontalInUse)
                {
                    MoveLeft();
                    isHorizontalInUse = true;
                }
            }
            if (horizontalInput > 0)
            {
                if (!isHorizontalInUse)
                {
                    MoveRight();
                    isHorizontalInUse = true;
                }
            }
            if (horizontalInput == 0)
            {
                isHorizontalInUse = false;
            }*/

            if (verticalInput < 0)
            {
                if (!isVerticalInUse)
                {
                    MoveBackward();
                    isVerticalInUse = true;
                }
            }
            if (verticalInput > 0)
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

            if (Input.GetButtonDown("Rotate left") && !lookingUp)
            {
                RotateLeft();
            }
            if (Input.GetButtonDown("Rotate right") && !lookingUp)
            {
                RotateRight();
            }
            if (Input.GetButtonDown("Look up"))
            {
                if (lookingUp)
                {
                    RotateDown();
                    lookingUp = false;
                }
                else
                {
                    RotateUp();
                    lookingUp = true;
                }
            }
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
            //horizontalInput = Input.GetAxisRaw("Horizontal DPAD");
            verticalInput = Input.GetAxisRaw("Vertical DPAD");
        }
        else
        {
            //horizontalInput = Input.GetAxisRaw("Horizontal");
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
            else if (hitWall.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Hit something, but it's not a wall. It's a " + hitWall.collider.gameObject.tag);
                GameManager.Instance.enemyObject = hitWall.collider.gameObject;
                GameManager.Instance.UpdateGameState(GameState.CombatStart);
                isInCombat = true;
                isWall = true; //dont walk into enemy
            }
        }
        else
        {
            Debug.Log("Nothing hit by the ray.");
        }

        return isWall;
    }

    /*public bool IsWallBehindPlayer()
    {
        float rayLength = 1.0f;
        Vector3 direction = -transform.forward;

        Debug.DrawRay(transform.position, direction * rayLength, Color.red, 1.0f);

        RaycastHit hitWall;
        bool isWall = false;

        if (Physics.Raycast(transform.position + moveVectorUp, direction, out hitWall, rayLength))
        {
            if (hitWall.collider.gameObject.CompareTag("Wall"))
            {
                isWall = true;
            }
        }
        else
        {
            Debug.Log("Nothing hit by the ray.");
        }

        return isWall;
    }*/

    public void RotateLeft() { if (AtRest) targetRotation -= Vector3.up * rotationAngle; }
    public void RotateRight() { if (AtRest) targetRotation += Vector3.up * rotationAngle; }
    public void RotateUp() { if (AtRest) targetRotation += Vector3.left * rotationAngle; }
    public void RotateDown() { targetRotation.x = 0; }

    public void MoveForward()
    {
        if (AtRest && !IsWall(transform.forward + moveVectorUp)) targetGridPos += transform.forward;
    }
    public void MoveBackward()
    {
        if (AtRest && !IsWall(-transform.forward + moveVectorUp)) targetGridPos -= transform.forward;
    }
    /*public void MoveLeft()
    {
        if (AtRest && !IsWall(-transform.right + moveVectorUp)) targetGridPos -= transform.right;
    }
    public void MoveRight()
    {
        if (AtRest && !IsWall(transform.right + moveVectorUp)) targetGridPos += transform.right;
    }*/

    private bool AtRest
    {
        get
        {
            //At rest when stopped moving or rotating, or when about to stop (distance small enough)
            if ((Vector3.Distance(transform.position, targetGridPos) < minStopDistance) &&
                (Vector3.Distance(transform.eulerAngles, targetRotation) < minStopDistance)) //TODO: fix atrest for fuck sake
                return true;
            else
                return false;
        }
    }

    public bool IsPlayerDead()
    {
        return healthAmount <= 0;
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.value = healthAmount;
    }

    public void LoseMana(float manaLost)
    {
        manaAmount -= manaLost;
        manaBar.value = manaAmount;
    }

    public void DisableMovement()
    {
        isInCombat = true;
    }

    public void EnableMovement()
    {
        Debug.Log("NOW MOVE");
        isInCombat = false;
    }

    public void RestoreHealth(int amount)
    {
        healthAmount += amount;
        healthBar.value = healthAmount;
    }

    public void GetJumpscared()
    {
        //TODO: Any extra info/mechanics for jumpscare enemy?
        GameManager.Instance.UpdateGameState(GameState.CombatStart);
        isInCombat = true;
    }

    public void RemoveEquipment(EquipmentBase toBeRemoved)
    {
        equipment.Remove(toBeRemoved);
    }

    public void PickUpEquipment(EquipmentBase toBeAdded)
    {
        equipment.Add(toBeAdded);
    }

}
