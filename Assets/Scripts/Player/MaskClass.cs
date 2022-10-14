using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MaskClass : MonoBehaviour
{
    [SerializeField] protected Sprite m_maskSprite;
    protected SpriteRenderer maskRenderer;

    [SerializeField] private WarMask warMask;
    [SerializeField] private NatureMask natureMask;

    public enum eMasks
    {
        war,
        nature,
        sea,
        energy
    }

    [SerializeField]
    private bool m_unlocked;

    public bool Unlocked
    {
        get { return m_unlocked; }
        set { m_unlocked = value; }
    }

    private void Start()
    {
        RemoveMasks();
        warMask.enabled = true;
    }

    public void MaskChange(eMasks maskNo)
    {
        //TODO : Add other masks

        switch(maskNo)
        {
            case eMasks.war:
                if (!warMask.enabled)
                {
                    RemoveMasks();
                    warMask.enabled = true;
                }
                break;
           case eMasks.nature:
                if (!natureMask.enabled)
                {
                    RemoveMasks();
                    natureMask.enabled = true;
                }
                break;
        }
    }

    private void RemoveMasks()
    {
        warMask.enabled = false;
        natureMask.enabled = false;
    }
}
