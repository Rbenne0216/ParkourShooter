using UnityEngine;

public class playerControl : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMult;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool isGrounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 direction;
    Rigidbody playerCharacter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCharacter = GetComponent<Rigidbody>();
        playerCharacter.freezeRotation = true;

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        UserInput();
        SpeedControl();

        if(isGrounded)
        {
            playerCharacter.linearDamping = groundDrag;
        }
        else
        {
            playerCharacter.linearDamping = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void UserInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        direction = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        if(isGrounded)
        {
            playerCharacter.AddForce(direction.normalized * moveSpeed * 10f, ForceMode.Force);
        } 
        else if (!isGrounded)
        {
            playerCharacter.AddForce(direction.normalized * moveSpeed * 10f * airMult, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVelo = new Vector3(playerCharacter.linearVelocity.x, 0f,  playerCharacter.linearVelocity.z);

        if (flatVelo.magnitude > moveSpeed)
        {
            Vector3 limitVelo = flatVelo.normalized * moveSpeed;
            playerCharacter.linearVelocity = new Vector3(limitVelo.x, playerCharacter.linearVelocity.y, limitVelo.z);
        }
    }

    private void Jump()
    {
        playerCharacter.linearVelocity = new Vector3(playerCharacter.linearVelocity.x, 0f, playerCharacter.linearVelocity.z);
        playerCharacter.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
