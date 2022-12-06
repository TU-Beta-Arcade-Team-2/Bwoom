using UnityEngine;

public class DestroyOnTimer : MonoBehaviour
{
    [SerializeField] private float m_time;
    private void Start()
    {
        Destroy(gameObject, m_time);
    }
}
