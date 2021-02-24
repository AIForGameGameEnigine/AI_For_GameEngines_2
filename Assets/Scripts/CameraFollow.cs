using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 cameraOffeset;

    [Range(0.01f, 1.0f)]
    public float smoothness = 0.5f;

    private float camFov;
    public float zoomSpeed;

    private float mouseScrollInput;

    public float camSpeed = 20;
    public float screenSizeThickness = 10;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //snap to player
        cameraOffeset = new Vector3(8.1f, 13, 0);
        Vector3 newpos = player.position + cameraOffeset;
        transform.position = Vector3.Slerp(transform.position, newpos, smoothness);
        camFov = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        
        // Change FOV
        mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");

        camFov -= mouseScrollInput * zoomSpeed;
        camFov = Mathf.Clamp(camFov, 40, 70);

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, camFov, zoomSpeed);

        //Roam

        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= Screen.height - screenSizeThickness) //up
            pos.x -= camSpeed * Time.deltaTime;
        if (Input.mousePosition.y <=  screenSizeThickness) // down
            pos.x += camSpeed * Time.deltaTime;
        if (Input.mousePosition.x >= Screen.width - screenSizeThickness)//right
            pos.z += camSpeed * Time.deltaTime;
        if (Input.mousePosition.x <=  screenSizeThickness)//left
            pos.z -= camSpeed * Time.deltaTime;

        transform.position = pos;
    }
}
