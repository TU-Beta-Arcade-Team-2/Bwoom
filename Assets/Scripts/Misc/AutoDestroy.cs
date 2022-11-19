using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] private float m_destructionTimer;
    private void Start()
    {
        Destroy(gameObject, m_destructionTimer);
    }
}
