using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorCameraMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float zoomSpeed = 750f;

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

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, 0, LevelInfo.levelData.sizeX - 1);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, 0, LevelInfo.levelData.sizeY - 1);

        // Camera zoom - don't move camera is cursor is over GUI element
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            zoom += Input.GetAxisRaw("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        }

        // Update camera
        gameObject.transform.position = desiredPosition;
        cam.orthographicSize = zoom;
    }
}
