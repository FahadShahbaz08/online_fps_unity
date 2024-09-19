using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // for camera within player
    [SerializeField] Transform ViewPortCamera;

    // for sensitivity
    public float mouseSensitivity = 5f;
    private float moveSpeed = 5f;
    // for storing vertical rotation
    private float verticalRotationStoring;

    // for storing mouse movement within x and y
    private Vector2 mouseinput;
    private Vector3 moveDirection;
    private Vector3 movement;

    void Start()
    {
        // hum ne yahan cursor ko get kia hai or us ka cursor lock kar dia ta ke wo screen mai nzr na aei
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        // Get raw mouse input and multiply by sensitivity
        mouseinput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;

        // for horizontal rotaion
        gameObject.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + mouseinput.x,transform.rotation.eulerAngles.z);

        // for vertical rotation
        // vertical move ko hum ne yahan + rakha hai jis ki wajha se ye invert rotate kare ga. is ko hum kisi bool se bhi set karwa sakte hain ta ke player choose kar sake ke us ne invert horizontal move karna hai ya normal
        verticalRotationStoring += mouseinput.y;
        // using mathf.clamp so that we can just rotate within certain points
        verticalRotationStoring = Mathf.Clamp(verticalRotationStoring, -60, 60);

        ViewPortCamera.transform.rotation = Quaternion.Euler(-verticalRotationStoring, ViewPortCamera.transform.rotation.eulerAngles.y, ViewPortCamera.transform.rotation.eulerAngles.z);

        moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        movement = ((transform.forward * moveDirection.z) + (transform.right * moveDirection.y));

        transform.position += movement * moveSpeed * Time.deltaTime;
    }
}
