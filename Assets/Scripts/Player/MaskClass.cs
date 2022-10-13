using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaskClass : MonoBehaviour
{
    protected Sprite m_maskSprite;

    [SerializeField] private SpriteRenderer maskRenderer;

    public void GetMaskSprite (string path)
    {
        m_maskSprite = Resources.Load<Sprite>(path);

        Debug.Log(m_maskSprite);

        maskRenderer.sprite = m_maskSprite;
    }

    [SerializeField]
    private bool m_unlocked;

    public bool Unlocked
    {
        get { return m_unlocked; }
        set { m_unlocked = value; }
    }

    public void MaskChange(int maskNo)
    {
        //TODO : Add child mask components

        gameObject.AddComponent(typeof(WarMask));
    }
}
