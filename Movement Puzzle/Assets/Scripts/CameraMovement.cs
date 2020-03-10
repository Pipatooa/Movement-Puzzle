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

    Vector3 cameraOffset;

    bool trackingEnabled;

    // Moves camera to current player and enables tracking
    public void StartTracking()
    {
        trackingEnabled = true;

        cameraOffset.y = minZoom;
        cameraOffset.z = -minZoom / Mathf.Tan(cameraRotation * Mathf.Deg2Rad);
        cameraOffset = Quaternion.Euler(0, LevelInfo.playerManager.transform.rotation.y, 0) * cameraOffset;

        gameObject.transform.position = LevelInfo.playerManager.currentPlayer.gameObject.transform.position + cameraOffset;
    }

    // Move camera after physics updates
    void LateUpdate()
    {
        // Do not update camera if tracking is not enabled
        if (!trackingEnabled)
        {
            return;
        }
        
        // Alter offset depending on camera zoom
        cameraOffset.y += Input.GetAxisRaw("Mouse ScrollWheel") * -zoomSpeed * Time.deltaTime;
        cameraOffset.y = Mathf.Clamp(cameraOffset.y, minZoom, maxZoom);
        cameraOffset.z = -cameraOffset.y / Mathf.Tan(cameraRotation * Mathf.Deg2Rad);

        // Store the camera offset prior to rotation and apply new rotation
        Vector3 previousCameraOffset = Quaternion.Euler(0, gameObject.transform.eulerAngles.y, 0) * cameraOffset;
        gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.Euler(cameraRotation, LevelInfo.playerManager.currentPlayer.gameObject.transform.eulerAngles.y, 0), cameraRotSpeed * Time.deltaTime);

        // Calculate new camera offset and position
        Vector3 newCameraOffset = Quaternion.Euler(0, gameObject.transform.eulerAngles.y, 0) * cameraOffset;
        Vector3 desiredPosition = LevelInfo.playerManager.currentPlayer.gameObject.transform.position;
        desiredPosition.y = 0.5f;

        // Smoothly move camera to desired position, taking camera offset before rotation into account
        gameObject.transform.position = Vector3.Lerp(gameObject.transform.position - previousCameraOffset, desiredPosition, cameraMoveSpeed * Time.deltaTime) + newCameraOffset;
    }
}
