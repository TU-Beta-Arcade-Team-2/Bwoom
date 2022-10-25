using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

// Camera movement is handled by Cinemachine, so this script will handle the dynamic camera effects we 
// want to see in-game, such as zooming in and out, locking the camera movement to just one axis
// and also to trigger the camera animations such as the camera shake when the player gets damaged
public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera m_camera;
    private GameObject m_followGameObject;

    // We are potentially going to be doing more than one thing with the dynamic camera
    // at once, so I am going to use Booleans to track its state for now...
    private bool m_isZooming;

    private bool m_isLockedX;
    private bool m_isLockedY;

    // This information is grabbed from a CameraTrigger object, and set through SetCameraZoom()
    private float m_targetZoom;
    private float m_targetZoomDuration;
    private float m_zoomTimer;

    private float m_lockedCoordX;
    private float m_lockedCoordY;

    private void Start()
    {
        m_followGameObject = m_camera.m_Follow.gameObject;
    }

    void Update()
    {
        if (m_isZooming)
        {
            m_zoomTimer += Time.deltaTime;

            ZoomOverTime();
        }

        if (m_isLockedX)
        {
            transform.position = new Vector3(
                m_lockedCoordX, 
                m_followGameObject.transform.position.y, 
                -10
            );
        }

        if (m_isLockedY)
        {
            transform.position = new Vector3(
                m_followGameObject.transform.position.x,
                m_lockedCoordY,
               -10
            );
        }
    }

    public void SetCameraZoom(float targetZoom, float zoomDuration)
    {
        m_targetZoom = targetZoom;
        m_targetZoomDuration = zoomDuration;
        m_zoomTimer = 0f;

        m_isZooming = true;
    }

    public void LockCoordX(float xCoord)
    {
        m_lockedCoordX = xCoord;
        m_isLockedX = true;
        m_camera.m_Follow = null;
    }

    public void UnlockCoordX()
    {
        m_isLockedX = false;

        // If the Y axis isn't locked as well, reset the camera follow
        if (!m_isLockedY)
        {
            m_camera.m_Follow = m_followGameObject.transform;
        }
    }

    public void LockCoordY(float yCoord)
    {
        m_lockedCoordY = yCoord;
        m_isLockedY = true;
        m_camera.m_Follow = null;
    }

    public void UnlockCoordY()
    {
        m_isLockedY = false;

        // If the X axis isn't locked as well, reset the camera follow
        if (!m_isLockedX)
        {
            m_camera.m_Follow = m_followGameObject.transform;
        }
    }

    private void ZoomOverTime()
    {
        float lerpVal = Mathf.Clamp01(m_zoomTimer / m_targetZoomDuration);

        float newOrthoSize = Mathf.Lerp(m_camera.m_Lens.OrthographicSize, m_targetZoom, lerpVal);

        if (Math.Abs(newOrthoSize - m_targetZoom) < Mathf.Epsilon)
        {
            m_isZooming = false;
        }

        m_camera.m_Lens.OrthographicSize = newOrthoSize;
    }
}
