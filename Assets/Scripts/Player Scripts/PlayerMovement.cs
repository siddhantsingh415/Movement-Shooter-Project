using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 35.0f;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private Camera mainCamera;
    [SerializeField] private MouseSensitivity mouseSensitivity;
    private CamerRotation cameraRotation;
    [SerializeField] private CameraAngle cameraAngle;
    bool canDash = true;
    bool isDashing = false;
    public float dashingPower = 200f;
    private float dashDuration = 0.3f;
    private float dashCooldown = 1f;


    // Start is called once before the first execution of Update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        mainCamera = Camera.main;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        look();
        dodgeDash();
    }

    void movePlayer()
    {
        // Create a new Vector3 for movement
        Vector3 movement = new Vector3(playerInput.actions["Move"].ReadValue<Vector2>().x, 0, playerInput.actions["Move"].ReadValue<Vector2>().y);
        transform.Translate(movement * speed * Time.deltaTime);
    }

    void look()
    {
        Vector2 mousePos = playerInput.actions["Look"].ReadValue<Vector2>();
        cameraRotation.pitch -= mousePos.y * mouseSensitivity.vertical * Time.deltaTime;
        cameraRotation.yaw += mousePos.x * mouseSensitivity.horizontal * Time.deltaTime;
        cameraRotation.pitch = Mathf.Clamp(cameraRotation.pitch, cameraAngle.min, cameraAngle.max);
        mainCamera.transform.eulerAngles = new Vector3(cameraRotation.pitch, cameraRotation.yaw, 0.0f);
        transform.eulerAngles = new Vector3(0.0f, cameraRotation.yaw, 0.0f);
    }
    private void dodgeDash()
    {
        if (canDash)
        {
            float dashinput = playerInput.actions["Dash"].ReadValue<float>();
            if (dashinput > 0)
            {
                StartCoroutine(Dash());
            }
        }
    }
    // redo tomorrow to have two coroutienes 1 to control the dash and one to control cooldown for dash charges
    private IEnumerator Dash()
    {
        Vector3 movement = new Vector3(playerInput.actions["Move"].ReadValue<Vector2>().x, 0, playerInput.actions["Move"].ReadValue<Vector2>().y);
        canDash = false;
        isDashing = true;
        rb.linearVelocity += (transform.TransformDirection(movement) * dashingPower);

        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector3.zero;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

[System.Serializable]
public struct MouseSensitivity
{
    public float horizontal;
    public float vertical;
}

public struct CamerRotation
{
    public float pitch;
    public float yaw;
}

[System.Serializable]
public struct CameraAngle
{
    public float max;
    public float min;
}