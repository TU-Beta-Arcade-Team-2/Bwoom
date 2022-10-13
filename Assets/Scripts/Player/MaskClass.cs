using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaskClass : MonoBehaviour
{
    protected Sprite m_maskSprite;

    protected SpriteRenderer maskRenderer;

    public void GetMaskSprite (string path)
    {
        m_maskSprite = Resources.Load<Sprite>(path);

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
        //TODO : Add other masks

        switch(maskNo)
        {
            case 0:
                if (!gameObject.GetComponent<WarMask>())
                {
                    RemoveMasks();
                    gameObject.AddComponent(typeof(WarMask));
                }
                break;
           case 1:
                if (!gameObject.GetComponent<NatureMask>())
                {
                    RemoveMasks();
                    gameObject.AddComponent(typeof(NatureMask));
                }
                break;
        }
    }

    private void RemoveMasks()
    {
        Destroy(gameObject.GetComponent<WarMask>());
        Destroy(gameObject.GetComponent<NatureMask>());
    }
}
