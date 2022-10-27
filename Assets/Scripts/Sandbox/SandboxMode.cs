using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandboxMode : MonoBehaviour
{
    [SerializeField] private PlayerStats m_playerStats;
    [SerializeField] private RhinoEnemy m_rhino;
    [SerializeField] private WaspEnemy m_wasp;
    [SerializeField] private AntEnemy m_ant;

    private float m_distanceToPlayer = 2.0f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Keypad1))
        {
            Instantiate(m_wasp, m_playerStats.transform.position + (Vector3.right * m_distanceToPlayer), Quaternion.identity).SetPlayer(m_playerStats); ;
        }
        else if(Input.GetKeyDown(KeyCode.Keypad2))
        {
            Instantiate(m_rhino, m_playerStats.transform.position + (Vector3.right * m_distanceToPlayer), Quaternion.identity).SetPlayer(m_playerStats); ;
        }
        else if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            Instantiate(m_ant, m_playerStats.transform.position + (Vector3.right * m_distanceToPlayer), Quaternion.identity).SetPlayer(m_playerStats); ;
        }
    }
}
