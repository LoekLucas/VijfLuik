using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 7f;

    private Vector3 moveDirection;
    private Vector3 moveDirectionZ;
    private Vector3 moveDirectionX;
    private Vector3 velocity;

    private float gravityValue = -9.81f;
    private float tpDistance;
    public bool playerUp = false;
    public float playerLayer = 1;
    private bool tpAllowed = true;
    private int tps = 0;

    private CharacterController characterController;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();

        tpDistance = 1000;

        if (Input.GetMouseButtonDown(1) && tps < 6)
        {
            TeleportPlayer(new Vector3(transform.position.x, transform.position.y + tpDistance, transform.position.z));
            tps++;
        }
    }

    private void Move()
    {
        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirectionZ = new Vector3(0, 0, moveZ);
        moveDirectionX = new Vector3(moveX, 0, 0);
        moveDirection = transform.TransformDirection(moveDirectionZ + moveDirectionX);

        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            Walk();
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            Run();
        }

        velocity.y += gravityValue * Time.deltaTime;
        moveDirection += velocity;
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void Walk()
    {
        moveDirection *= walkSpeed;
    }

    private void Run()
    {
        moveDirection *= runSpeed;
    }

    private void TeleportPlayer(Vector3 location)
    {
        characterController.enabled = false;
        Quaternion savedRotation = transform.rotation;
        transform.SetPositionAndRotation(location, savedRotation);

        characterController.enabled = true;
    }
}
