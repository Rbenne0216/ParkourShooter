using System.Collections;


using System.Collections.Generic;

using UnityEngine;

public class MouseLook : MonoBehaviour
{
    //mouse sensitivity
    public float sensX;
    public float sensY;

    //players orientation
    public Transform orientation;

    //rotation of the camera
    float xRotation;
    float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //cursor is locked in the middle of screen
        Cursor.visible = false;
    }

    private void Update()
    {
        //get mouse input
        //get axis raw is gets the movement on the axis since the last frame
        //delta time is the time in seconds it took to complete the last frame
        //sens custom defined mouse sensitivity 
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;

        //can only rotate the camera up and down on the x axis 90 degrees
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);


    }

}
