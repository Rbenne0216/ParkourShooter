using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform cameraPosistion;

    private void Update()
    {
        //makes the camera always move with the player
        transform. position = cameraPosistion.position;
    }
}
