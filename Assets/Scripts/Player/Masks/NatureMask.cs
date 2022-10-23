using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureMask : MaskClass
{
    [SerializeField] private GameObject m_projectile;
    [SerializeField] private Controller m_playerController;
    [SerializeField] private float m_increasedMovement;

    private void OnEnable()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;

        //m_playerController.movementSpeed += m_increasedMovement;
    }

    private void OnDisable()
    {
        //m_playerController.movementSpeed -= m_increasedMovement;
    }

    public override void SpecialAttack()
    {
        Instantiate(m_projectile,transform.position,transform.rotation);
    }
}
