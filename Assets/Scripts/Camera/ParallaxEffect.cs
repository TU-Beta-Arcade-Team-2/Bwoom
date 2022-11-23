using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private GameObject m_backgroundGameObject;
    private List<Transform> m_backgroundPositions = new List<Transform>();

    [SerializeField] private float m_smoothingValue;
    private float[] m_parallaxValues;

    [SerializeField] private Transform m_cameraTransform;
    private Vector3 m_previousCameraPosition;


    // Start is called before the first frame update
    private void Start()
    {
        foreach(Transform go in m_backgroundGameObject.transform)
        {
            m_backgroundPositions.Add(go);
        }


        m_previousCameraPosition = m_cameraTransform.position;

        m_parallaxValues = new float[m_backgroundPositions.Count];

        for (int i = 0; i < m_backgroundPositions.Count; i++)
        {
            m_parallaxValues[i] = m_backgroundPositions[i].position.z * -1;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        for (int i = 0; i < m_backgroundPositions.Count; i++)
        {
            float parallax = (m_previousCameraPosition.x - m_cameraTransform.position.x) * m_parallaxValues[i];

            float backgroundTargetPositionX = m_backgroundPositions[i].position.x + parallax;

            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX,
                m_backgroundPositions[i].position.y, m_backgroundPositions[i].position.z);

            m_backgroundPositions[i].position = Vector3.Lerp(m_backgroundPositions[i].position,
                backgroundTargetPosition, m_smoothingValue * Time.deltaTime);
        }


        m_previousCameraPosition = m_cameraTransform.position;
    }
}
