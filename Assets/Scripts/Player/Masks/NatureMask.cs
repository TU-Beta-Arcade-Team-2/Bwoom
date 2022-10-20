using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NatureMask : MaskClass
{
    [SerializeField] private GameObject m_projectile;

    private void OnEnable()
    {
        m_maskRenderer = GameObject.Find("Mask").GetComponent<SpriteRenderer>();

        m_maskRenderer.sprite = m_maskSprite;
    }

    public override void SpecialAttack()
    {
        Instantiate(m_projectile,transform.position,transform.rotation);

        Debug.Log(transform.localEulerAngles);
    }
}
