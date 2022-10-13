using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaskClass
{
    [SerializeField]
    private Sprite m_maskSprite;

    public Sprite MaskSprite
    {
        get { return m_maskSprite; }
        set { m_maskSprite = value; }
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
    }
}
