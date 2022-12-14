using UnityEngine;

public class OnBeginHop : MonoBehaviour
{
    [SerializeField] private float m_addedForce;

    private float m_alphaValue = 1.2f;
    private SpriteRenderer m_sprite;
    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, m_addedForce));
        m_sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        m_sprite.color = new Color(m_sprite.color.r, m_sprite.color.b, m_sprite.color.g, m_alphaValue);
        m_alphaValue = m_alphaValue - Time.deltaTime;
    }
}
