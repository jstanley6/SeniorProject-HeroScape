using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationSpeed = 250f;
    public GameObject gridObject;
    public float zoomSpeed = 10f;
    public float distance = 10f;
    public float maxZoom = 25f;
    public float minZoom = 5f;

    private Vector3 offset;

    void Start()
    {
        if(gridObject != null)
        {
            offset = transform.position - gridObject.transform.position;
            distance = offset.magnitude;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(1))
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            float verticalInput = -1 * Input.GetAxis("Mouse Y");

            //transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
            //transform.Rotate(Vector3.left, verticalInput * rotationSpeed * Time.deltaTime);

            transform.RotateAround(gridObject.transform.position, Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
            transform.RotateAround(gridObject.transform.position, transform.right, verticalInput * rotationSpeed * Time.deltaTime);

            offset = transform.position - gridObject.transform.position;
        }

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        distance -= scrollInput * zoomSpeed;
        distance = Mathf.Clamp(distance, minZoom, maxZoom);

        transform.position = gridObject.transform.position + offset.normalized * distance;

        transform.LookAt(gridObject.transform.position);

    }
}
