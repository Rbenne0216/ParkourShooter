using UnityEngine;

public class WallRun : MonoBehaviour
{
    [Header("Wall Run Settings")]
    public LayerMask wallLayer;
    public float wallCheckDistance = 1f;
    public float wallRunGravity = 1f;
    public float wallRunSpeed = 7f;
    public float wallStickTime = 2f;
    public float jumpForce = 8f;

    private Rigidbody rb;
    private PlayerMovement pm;

    private bool isWallRunning;
    private Vector3 currentWallNormal;
    private float wallRunTimer;

    public KeyCode jumpKey = KeyCode.Space;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!isWallRunning)
        {
            CheckForWallRunStart();
        }
        else
        {
            if (!IsBesideWall() || IsFacingWall())
            {
                StopWallRun();
                return;
            }

            if (Input.GetKeyDown(jumpKey))
            {
                JumpOffWall();
                return;
            }

            ContinueWallRun();
        }
    }

    void CheckForWallRunStart()
    {
        if (pm.grounded) return;

        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.5f;

        if (Physics.Raycast(origin, transform.right, out hit, wallCheckDistance, wallLayer) ||
            Physics.Raycast(origin, -transform.right, out hit, wallCheckDistance, wallLayer))
        {
            StartWallRun(hit.normal);
        }
    }

    void StartWallRun(Vector3 wallNormal)
    {
        isWallRunning = true;
        currentWallNormal = wallNormal;
        wallRunTimer = wallStickTime;
        rb.useGravity = false;
    }

    void ContinueWallRun()
    {
        wallRunTimer -= Time.deltaTime;

        if (wallRunTimer <= 0f)
        {
            StopWallRun();
            return;
        }

        // Run along the wall in forward direction (projected)
        Vector3 wallForward = Vector3.Cross(currentWallNormal, Vector3.up).normalized;
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;

        if (Vector3.Dot(cameraForward, wallForward) < 0)
            wallForward *= -1;

        Vector3 runDir = wallForward;
        rb.linearVelocity = runDir * wallRunSpeed + Vector3.down * wallRunGravity;
    }

    void StopWallRun()
    {
        isWallRunning = false;
        rb.useGravity = true;

        currentWallNormal = Vector3.zero;
    }

    void JumpOffWall()
    {
        StopWallRun();

        // Cancel downward velocity for clean jump
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Add jump force away from wall
        Vector3 jumpDir = Vector3.up + currentWallNormal; // upward + push off wall
        rb.AddForce(jumpDir.normalized * jumpForce, ForceMode.Impulse);
    }

    bool IsBesideWall()
    {
        Vector3 origin = transform.position + Vector3.up * 0.5f;
        return Physics.Raycast(origin, transform.right, wallCheckDistance, wallLayer) ||
               Physics.Raycast(origin, -transform.right, wallCheckDistance, wallLayer);
    }

    bool IsFacingWall()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.5f, transform.forward, 0.6f, wallLayer);
    }
}