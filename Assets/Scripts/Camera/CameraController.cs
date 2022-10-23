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

    // We are potentially going to be doing more than one thing with the dynamic camera
    // at once, so I am going to use Booleans to track its state for now...
    private bool m_isZooming;
    private float m_zoomTimer;

    // This information is grabbed from a CameraTrigger object, and set through SetCameraZoom()
    private float m_targetZoom;
    private float m_targetZoomDuration;


    void Update()
    {
        if (m_isZooming)
        {
            m_zoomTimer += Time.deltaTime;

            ZoomOverTime();
        }
    }

    public void SetCameraZoom(float targetZoom, float zoomDuration)
    {
        m_targetZoom = targetZoom;
        m_targetZoomDuration = zoomDuration;
        m_zoomTimer = 0f;

        m_isZooming = true;
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
