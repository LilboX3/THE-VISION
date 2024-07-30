using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool smoothTransition = false;
    public float transitionSpeed = 3f;
    public float transitionRotationSpeed = 500f;

    public Slider healthBar;
    public Image healthImage;
    public float healthAmount = 7f;
    public Slider manaBar;
    public Image manaImage;
    public float manaAmount = 7f;

    private float minStopDistance = 0.05f;
    private float rotationAngle = 90f;
    //private float horizontalInput;
    private float verticalInput;
    private Vector3 moveVectorUp = new Vector3(0, 0.1f, 0);

    //private bool isHorizontalInUse = false;
    private bool isVerticalInUse = false;
    private bool isInCombat = false;

    Vector3 targetGridPos;
    Vector3 prevTargetGridPos;
    Vector3 targetRotation;

    private void Start()
    {
        targetGridPos = Vector3Int.RoundToInt(transform.position);
        StartCoroutine(LoseHealth());
    }

    IEnumerator LoseHealth()
    {
        for(int i=0; i<7; i++)
        {
            yield return new WaitForSeconds(1f);
            Debug.Log("took 1 damage");
            TakeDamage(1);
        }
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

            if (Input.GetButtonDown("Rotate left"))
            {
                RotateLeft();
            }
            if (Input.GetButtonDown("Rotate right"))
            {
                RotateRight();
            }
        } else
        {
            GameManager.Instance.UpdateGameState(GameState.Movement);
            Debug.Log("should move again");
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


    //TODO: raycast if enemy
    public bool IsWall(Vector3 intention)
    {
        Debug.Log("CHECKING IF WALL!!!!!!!!!");

        float rayLength = 1.0f;

        Debug.DrawRay(transform.position, intention * rayLength, Color.red, 1.0f);

        RaycastHit hitWall;
        bool isWall = false;

        if (Physics.Raycast(transform.position + moveVectorUp, intention, out hitWall, rayLength))
        {
            Debug.Log("Hitting something at distance: " + hitWall.distance);
            if (hitWall.collider.gameObject.CompareTag("Wall"))
            {
                Debug.Log("THERE IS WALL");
                isWall = true;
            }
            else if(hitWall.collider.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("Hit something, but it's not a wall. It's a " + hitWall.collider.gameObject.tag);
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
                (Vector3.Distance(transform.eulerAngles, targetRotation) < minStopDistance))
                return true;
            else
                return false;
        }
    }

    public void PlayerTurn()
    {

    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.value = healthAmount;
        Debug.Log("fill amount is now: "+healthBar.value);
    }

    public void DisableMovement()
    {
        isInCombat = true;
    }

    public void EnableMovement()
    {
        isInCombat |= false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Trigger combat if enemy trigger entered
            GameManager.Instance.UpdateGameState(GameState.CombatStart);
            isInCombat = true;
        }
    }
}
