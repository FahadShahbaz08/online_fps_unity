using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform ViewPortCamera;

    public float mouseSensitivity = 5f;
    private float moveSpeed = 5f;
    private float runSpeed = 10f;
    public float jumpForce = 3f; // Adjust jump force for better height
    public float gravityPull = 9.8f; // Standard gravity
    public float fallMultiplier = 2.5f; // To fall faster

    private float verticalRotationStoring;

    private Vector2 mouseinput;
    private Vector3 moveDirection;
    private Vector3 movement;

    private float activeMoveSpeed;

    private Camera camera;
    public CharacterController characterController;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        camera = Camera.main;
    }

    void Update()
    {
        // Handle Mouse Input for Camera Rotation
        mouseinput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        // Update horizontal rotation
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseinput.x, transform.rotation.eulerAngles.z);

        // Update vertical rotation (looking up and down)
        verticalRotationStoring += mouseinput.y;
        verticalRotationStoring = Mathf.Clamp(verticalRotationStoring, -60, 60);
        ViewPortCamera.transform.rotation = Quaternion.Euler(-verticalRotationStoring, ViewPortCamera.transform.rotation.eulerAngles.y, ViewPortCamera.transform.rotation.eulerAngles.z);

        // Sprint Handling
        if (Input.GetKey(KeyCode.LeftShift))
        {
            activeMoveSpeed = runSpeed;
        }
        else
        {
            activeMoveSpeed = moveSpeed;
        }

        // Movement Input
        float yValue = movement.y;
        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        movement = ((transform.forward * moveDirection.z) + (transform.right * moveDirection.x)) * activeMoveSpeed;
        movement.y = yValue;

        // Jumping and gravity handling
        if (characterController.isGrounded)
        {
            movement.y = -2f;  // Keeps player grounded properly

            if (Input.GetButtonDown("Jump"))
            {
                movement.y = jumpForce;
            }
        }

        // Apply gravity while falling
        if (!characterController.isGrounded)
        {
            if (movement.y < 0)
            {
                movement.y += Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;  // Faster fall
            }
            else
            {
                movement.y += Physics.gravity.y * Time.deltaTime;  // Regular gravity pull when going up
            }
        }

        // Move the character controller
        characterController.Move(movement * Time.deltaTime);
    }

    private void LateUpdate()
    {
        // Sync the camera to the player's viewpoint
        camera.transform.position = ViewPortCamera.position;
        camera.transform.rotation = ViewPortCamera.rotation;
    }
}
