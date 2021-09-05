using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody playerRb;
    private float movementX, movementZ;
    public float normalSpeed = 8f;
    public float currentSpeed;
    public float crouchSpeedRatioToNormalSpeed = 3f;
    public float jumpForce = 350f;
    private Vector3 movementVector;
    public bool isCrouching = false;
    public bool isGrounded = true;
    public Transform playerFeet;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        currentSpeed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameCoordinator.instance.isPaused)
        {
            return;
        }

        if (!isGrounded)
        {
            return;
        }

        movementX = Input.GetAxisRaw("Horizontal");
        movementZ = Input.GetAxisRaw("Vertical");
        movementVector = transform.right * movementX + transform.forward * movementZ; //oyuncunun yüzünün dönük olduğu yer fark etmesin diye hep aynı yönde hareket edecek
        playerRb.MovePosition(playerRb.position + movementVector * currentSpeed * Time.deltaTime);


        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (!isCrouching)
            {
                crouch();
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            if (isCrouching)
            {
                getUp();
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                jump();
            }
        }
    }

    private void crouch()
    {
        Vector3 playerScale = transform.localScale;
        Vector3 playerPos = transform.position;

        transform.localScale = new Vector3(playerScale.x, playerScale.y / 2, playerScale.z);
        transform.position = new Vector3(playerPos.x, playerPos.y / 2, playerPos.z);
        currentSpeed = normalSpeed / crouchSpeedRatioToNormalSpeed;
        isCrouching = true;
    }

    private void getUp()
    {
        Vector3 playerScale = transform.localScale;
        Vector3 playerPos = transform.position;

        transform.localScale = new Vector3(playerScale.x, playerScale.y * 2, playerScale.z);
        transform.position = new Vector3(playerPos.x, playerPos.y * 2, playerPos.z);
        currentSpeed = normalSpeed;
        isCrouching = false;
    }

    private void jump()
    {
        Vector3 jumpVector = new Vector3(0f, jumpForce, 0f) + movementVector.normalized * jumpForce;
        playerRb.AddForce(jumpVector);
        isGrounded = false;
    }

    private void OnCollisionExit(Collision collision)
    {


        if (collision.gameObject.CompareTag("Ground"))
        {
            if (isGrounded)
            {
                isGrounded = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (!isGrounded)
            {
                isGrounded = true;
            }

        }
        /*Collider[] colliders = Physics.OverlapSphere(playerFeet.position, 0.1f);
        foreach (Collider ground in colliders)
        {
            if (!ground.CompareTag("Ground"))
            {
                return; 
            }
            isGrounded = true;
            currentSpeed = normalSpeed;
        }*/
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(playerFeet.position, 0.1f);
    }*/
    public void launchHookCall()
    {
        Camera.main.GetComponent<GrapplingHook>().setHookOnItsWay();
    }
}