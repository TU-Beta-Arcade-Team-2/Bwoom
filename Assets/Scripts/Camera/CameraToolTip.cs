using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraToolTip : MonoBehaviour
{
    public GameObject Controller;

    public string GameObjectName = "CAMERA_TRIGGER";

    public List<CameraTrigger.eTriggerProperty> TriggerProperties;

    [Header("Camera Zoom Properties")] 
    public float TargetZoom;
    public float ZoomDuration;


    [CustomEditor(typeof(CameraToolTip))]
    public class CameraToolTipEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CameraToolTip myTarget = target as CameraToolTip;
            
            if (GUILayout.Button("Place Camera Trigger"))
            {
                GameObject newCameraTrigger = new GameObject("CAMERA_TRIGGER")
                {
                    transform =
                    {
                        position = myTarget.transform.position,
                        rotation = myTarget.transform.localRotation,
                        localScale = myTarget.transform.localScale
                    },

                    name = myTarget.GameObjectName
                };

                var iconContent = EditorGUIUtility.IconContent("sv_icon_name7");
                EditorGUIUtility.SetIconForObject(newCameraTrigger, (Texture2D)iconContent.image);

                CameraTrigger ct = newCameraTrigger.AddComponent<CameraTrigger>();

                ct.SetCameraController(myTarget.Controller);
                ct.Properties = myTarget.TriggerProperties;
                ct.ZoomAmount = myTarget.TargetZoom;
                ct.ZoomDuration = myTarget.ZoomDuration;

                BoxCollider2D collider = newCameraTrigger.AddComponent<BoxCollider2D>();
                collider.isTrigger = true;
                collider.size = new Vector2(5, 5);
            }

            DrawDefaultInspector();
        }
    }
}
