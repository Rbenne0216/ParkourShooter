using UnityEngine;

public class cameraMouse : MonoBehaviour
{
    public Vector3 turn;
    public float sensitivity = 2.0f; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        turn.x += Input.GetAxisRaw("Mouse X") * sensitivity;
        turn.y += Input.GetAxisRaw("Mouse Y") * sensitivity;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
