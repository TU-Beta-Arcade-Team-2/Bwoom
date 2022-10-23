using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class will be placed around the scene to trigger the dynamic camera effects when the player 
// passes over them
public class CameraTrigger : MonoBehaviour
{
    public enum eCameraTriggerType
    {
        Zoom,
        Pan,
        Rotate,
        Shake,
        LockX,
        LockY
    }

    public eCameraTriggerType Type;

    public float ZoomAmount;
    public float ZoomDuration;

    [SerializeField] private CameraController m_cameraController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag(StringConstants.PLAYER_TAG)) { return; }

        switch (Type)
        {
            case eCameraTriggerType.Zoom:
                m_cameraController.SetCameraZoom(ZoomAmount, ZoomDuration);
                BetterDebugging.Instance.DebugLog($"Setting camera zoom to {ZoomAmount} over {ZoomDuration} seconds", BetterDebugging.eDebugLevel.Message);
                break;
            case eCameraTriggerType.Pan:
                break;
            case eCameraTriggerType.Rotate:
                break;
            case eCameraTriggerType.Shake:
                break;
            case eCameraTriggerType.LockX:
                break;
            case eCameraTriggerType.LockY:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
