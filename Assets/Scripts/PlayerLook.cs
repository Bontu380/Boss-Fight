using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform player;
    private float mouseX, mouseY, xRotation;
    public float mouseSensivity = 50f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {

        if (GameCoordinator.instance.isPaused )
        {
            return;
        }

        mouseX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;


      
        xRotation -= mouseY;

        /*  Aşağıdakini kullanmayıp xRotationda tutmamızn sebebi, clamp yapabilmek
            transform.Rotate(Vector3.right * mouseY);

       */

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        player.Rotate(Vector3.up * mouseX );
        transform.localRotation = Quaternion.Euler(xRotation,0f,0f);


    }
}
