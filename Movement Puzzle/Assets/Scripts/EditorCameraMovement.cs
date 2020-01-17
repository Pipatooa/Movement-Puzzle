using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float zoomSpeed = 10000f;

    public float minX = -10f;
    public float maxX = 60f;

    public float minZ = -10f;
    public float maxZ = 60f;

    public float minZoom = 10f;
    public float maxZoom = 30f;

    private Vector3 desiredPosition;

    private void Awake()
    {
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
        desiredPosition.y += Input.GetAxisRaw("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime;
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minZoom, maxZoom);

        // Update position
        gameObject.transform.position = desiredPosition;
    }
}
