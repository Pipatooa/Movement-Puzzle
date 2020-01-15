using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraRotSpeed = 5f;
    public float cameraMoveSpeed = 5f;

    public float zoomSpeed = 10000f;
    public float minZoom = 15f;
    public float maxZoom = 30f;

    public float cameraRotation = 60f;

    private Vector3 cameraOffset;

    // Update is called once per frame
    void LateUpdate()
    {
        // Alter offset depending on camera zoom
        cameraOffset.y += Input.GetAxisRaw("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime;
        cameraOffset.y = Mathf.Clamp(cameraOffset.y, minZoom, maxZoom);
        cameraOffset.z = -cameraOffset.y / Mathf.Tan(cameraRotation * Mathf.Deg2Rad);

        Vector3 previousCameraOffset = Quaternion.Euler(0, gameObject.transform.eulerAngles.y, 0) * cameraOffset;
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(cameraRotation, LevelInfo.playerManager.currentPlayer.transform.eulerAngles.y, 0), cameraRotSpeed * Time.deltaTime);

        Vector3 newCameraOffset = Quaternion.Euler(0, gameObject.transform.eulerAngles.y, 0) * cameraOffset;
        Vector3 desiredPosition = LevelInfo.playerManager.currentPlayer.transform.position;
        desiredPosition.y = 0.5f;

        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position - previousCameraOffset, desiredPosition, cameraMoveSpeed * Time.deltaTime) + newCameraOffset;
    }
}
