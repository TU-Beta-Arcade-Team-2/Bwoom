using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private LevelManager lvlManager;
    private Animator cameraAnim;

    [Header("Health Variables")]
    [Range(0, 8)]
    [SerializeField] private int playerHealth;
    [Range(1, 8)]
    [SerializeField] private int maxPlayerHealth;

    [SerializeField] private Image[] mask;
    [SerializeField] private Sprite fullMask;
    [SerializeField] private Sprite brokenMask;
    [Space(10)]

    [Header("Mask Ability Variables")]
    [Range(1, 4)]
    [SerializeField] private int maskSelected;
    [SerializeField] private SpriteRenderer playerMask;
    [SerializeField] private Sprite maskSprite1;
    [SerializeField] private Sprite maskSprite2;
    [SerializeField] private Sprite maskSprite3;
    [SerializeField] private Sprite maskSprite4;

    #region Main Functions

    private void Start()
    {
        playerHealth = Mathf.Clamp(playerHealth, 0, maxPlayerHealth);
        DisplayUIMasks();

        lvlManager = FindObjectOfType<LevelManager>();
        cameraAnim = Camera.main.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        //This update is just for testing in inspector will be removed onced finalised
        MaskChange(maskSelected);
    }

    #endregion

    #region Health Functions

    public void TakeDMG(int incomingDMG)
    {
        playerHealth -= incomingDMG;

        DisplayUIMasks();

        cameraAnim.SetTrigger("LightShake");

        if (playerHealth <= 0)
        {
            lvlManager.Death();
            Destroy(gameObject);
            return;
        }

        //Play hurt animation
    }

    public void TakeHEAL(int incomingHEAL)
    {
        playerHealth = Mathf.Clamp(playerHealth + incomingHEAL, 0, maxPlayerHealth);

        DisplayUIMasks();

        //Play heal animation
    }

    private void DisplayUIMasks()
    {
        for (int i = 0; i < mask.Length; i++)
        {
            if (i < playerHealth)
            {
                mask[i].sprite = fullMask;
            }

            else
            {
                mask[i].sprite = brokenMask;
            }

            mask[i].enabled = (i < maxPlayerHealth);
        }
    }

    #endregion

    #region Mask Ability Functions

    public void MaskChange(int maskNo)
    {
        maskSelected = maskNo;

        switch (maskNo)
        {
            case 1:
                playerMask.sprite = maskSprite1;
                break;
            case 2:
                playerMask.sprite = maskSprite2;
                break;
            case 3:
                playerMask.sprite = maskSprite3;
                break;
            case 4:
                playerMask.sprite = maskSprite4;
                break;
        }
    }

    #endregion
}
