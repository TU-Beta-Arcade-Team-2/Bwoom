using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will be placed around the scene to trigger the dynamic camera effects when the player 
// passes over them
public class CameraTrigger : MonoBehaviour
{
    public enum eTriggerProperty
    {
        Zoom,
        Pan,
        Rotate,
        Shake,
        LockX,
        LockY
    }

    public List<eTriggerProperty> Properties;


    [Header("Camera Zoom Properties")]
    public float ZoomAmount;
    public float ZoomDuration;

    [Header("Camera Restrictions")]
    public float LockedCoordX;
    public float LockedCoordY;

    public GameObject CameraControllerObject;
    private CameraController m_cameraController;

    void Start()
    {
        CameraController cameraController = CameraControllerObject.GetComponent<CameraController>();
        m_cameraController = cameraController;

        BetterDebugging.Instance.Assert(cameraController != null);
    }

    [ExecuteInEditMode] public void SetCameraController(GameObject cameraObject)
    {
        CameraControllerObject = cameraObject;
        BetterDebugging.Instance.DebugLog("SETTING CAMERA CONTROLLER", BetterDebugging.eDebugLevel.Message);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(StringConstants.PLAYER_TAG)) { return; }

        BetterDebugging.Instance.DebugLog(
            "Hit the player!",
            BetterDebugging.eDebugLevel.Message
        );

        // Keep track of whether we are locking or unlocking the XY coords in this trigger
        bool lockedX = false;
        bool lockedY = false;

        foreach (eTriggerProperty property in Properties)
        {
            switch (property)
            {
                case eTriggerProperty.Zoom:
                    m_cameraController.SetCameraZoom(ZoomAmount, ZoomDuration);
                    
                    BetterDebugging.Instance.DebugLog(
                        $"Setting camera zoom to {ZoomAmount} over {ZoomDuration} seconds",
                        BetterDebugging.eDebugLevel.Message
                    );
                    break;


                case eTriggerProperty.Pan:
                    break;
                case eTriggerProperty.Rotate:
                    break;
                case eTriggerProperty.Shake:
                    break;
                case eTriggerProperty.LockX:
                    m_cameraController.LockCoordX(LockedCoordX);

                    BetterDebugging.Instance.DebugLog(
                        $"Locking camera X Coordinate to {LockedCoordX}",
                        BetterDebugging.eDebugLevel.Message
                    );

                    lockedX = true;
                    break;


                case eTriggerProperty.LockY:
                    m_cameraController.LockCoordY(LockedCoordY);

                    BetterDebugging.Instance.DebugLog(
                        $"Locking camera Y Coordinate to {LockedCoordY}",
                        BetterDebugging.eDebugLevel.Message
                    );

                    lockedY = true;
                    break;


                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        // Reset if the property isn't in the list
        if (!lockedX)
        {
            m_cameraController.UnlockCoordX();
        }

        if (!lockedY)
        {
            m_cameraController.UnlockCoordY();
        }
    }
}
