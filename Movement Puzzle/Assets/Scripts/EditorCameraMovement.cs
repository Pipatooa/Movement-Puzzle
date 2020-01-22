using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zoomSpeed = 1000f;

    public float minX = 0f;
    public float maxX = 50f;

    public float minZ = 0f;
    public float maxZ = 50f;

    public float minZoom = 5f;
    public float maxZoom = 25f;

    Camera cam;

    Vector3 desiredPosition;
    float zoom;

    void Awake()
    {
        cam = gameObject.GetComponent<Camera>();

        zoom = minZoom;
        desiredPosition = gameObject.transform.position;
    }

    void LateUpdate()
    {
        // Camera panning
        desiredPosition.x += Input.GetAxisRaw("Horizontal") * moveSpeed * zoom * Time.deltaTime;
        desiredPosition.z += Input.GetAxisRaw("Vertical") * moveSpeed * zoom * Time.deltaTime;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);

        // Camera zoom
        zoom += Input.GetAxisRaw("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        // Update camera
        gameObject.transform.position = desiredPosition;
        cam.orthographicSize = zoom;
    }
}
