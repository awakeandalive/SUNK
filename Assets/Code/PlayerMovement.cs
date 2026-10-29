using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller;
    public float speed = 5f;
    public float gravity = -19.62f;
    public float jumpHeight = 1.5f;

    [Header("Water & Swimming Settings")]
    public float waterLevel = 0f;       // The Y height where your ocean plane is
    public float floatSpeed = 4f;       // How fast you bob back to the surface
    public float waterJumpForce = 7f;   // The burst of speed to vault out of water onto a boat/cube

    [Header("Look Settings")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    private float xRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Ground Check
        isGrounded = controller.isGrounded;

        // 2. Mouse Look
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // 3. Walk Around (WASD)
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        // 4. Check if Player is in Water (Only if NOT standing on solid ground/cube)
        bool isInWater = transform.position.y < waterLevel && !isGrounded;

        if (isInWater)
        {
            // Slow down movement slightly while swimming
            controller.Move(move * (speed * 0.7f) * Time.deltaTime);

            float targetYVelocity = floatSpeed; // Default: float towards surface

            // Hold Left Ctrl to dive / sink down
            if (Input.GetKey(KeyCode.LeftControl))
            {
                targetYVelocity = -4f;
            }
            // PRESS Spacebar to Vault / Jump out of the water onto the boat/cube!
            else if (Input.GetButtonDown("Jump"))
            {
                velocity.y = waterJumpForce; // Instant upward leap out of water
            }
            // Hold Space to swim upward steadily
            else if (Input.GetKey(KeyCode.Space))
            {
                targetYVelocity = 5f;
                velocity.y = Mathf.Lerp(velocity.y, targetYVelocity, Time.deltaTime * 5f);
            }
            else
            {
                // Smoothly bob towards the surface when doing nothing
                velocity.y = Mathf.Lerp(velocity.y, targetYVelocity, Time.deltaTime * 5f);
            }
        }
        else
        {
            // Normal Land / Platform Movement
            controller.Move(move * speed * Time.deltaTime);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            // Jump on land or when standing on a cube/boat deck!
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
        }

        // Apply final calculated movement vector
        controller.Move(velocity * Time.deltaTime);
    }
}