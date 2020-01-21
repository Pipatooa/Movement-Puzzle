using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zoomSpeed = 1000f;

    public float minX = -10f;
    public float maxX = 60f;

    public float minZ = -10f;
    public float maxZ = 60f;

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
        desiredPosition.x += Input.GetAxisRaw("Horizontal") * moveSpeed * gameObject.transform.position.y * Time.deltaTime;
        desiredPosition.z += Input.GetAxisRaw("Vertical") * moveSpeed * gameObject.transform.position.y * Time.deltaTime;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, -10, 60);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, -10, 60);

        // Camera zoom
        zoom += Input.GetAxisRaw("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime;
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);

        // Update camera
        gameObject.transform.position = desiredPosition;
        cam.orthographicSize = zoom;
    }
}
