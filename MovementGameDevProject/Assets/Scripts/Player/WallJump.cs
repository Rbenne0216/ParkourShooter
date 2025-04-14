using UnityEngine;

public class WallJump : MonoBehaviour
{
    public LayerMask whatIsWall;
    public float wallJumpForce = 10f;
    public float wallDetectionDistance = 0.6f;
    public KeyCode jumpKey = KeyCode.Space;

    private Rigidbody rb;
    private PlayerMovement playerMovement;
    private bool wallAvailable;
    private bool canWallJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        CheckWall();

        if (Input.GetKeyDown(jumpKey) && !playerMovement.grounded && wallAvailable && canWallJump)
        {
            DoWallJump();
        }
    }

    private void CheckWall()
    {
        // Raycast in the direction player is facing (left and right)
        Vector3 right = transform.right;
        Vector3 left = -transform.right;

        wallAvailable = Physics.Raycast(transform.position, right, wallDetectionDistance, whatIsWall)
                     || Physics.Raycast(transform.position, left, wallDetectionDistance, whatIsWall);

        if (playerMovement.grounded)
            canWallJump = true; // Reset when on ground
    }

    private void DoWallJump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // zero out y
        rb.AddForce(Vector3.up * wallJumpForce, ForceMode.Impulse);
        canWallJump = false;
    }
}