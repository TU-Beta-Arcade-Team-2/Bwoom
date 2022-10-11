using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private LevelManager lvlManager;
    private Animator cameraAnim;

    private enum Masks
    {
        warMask = 1,
        natureMask,
        seaMask,
        energyMask
    }

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
    [SerializeField] private Masks maskSelected;
    [SerializeField] private SpriteRenderer playerMask;
    [SerializeField] private Sprite warMaskSprite;
    [SerializeField] private Sprite natureMaskSprite;
    [SerializeField] private Sprite seaMaskSprite;
    [SerializeField] private Sprite energyMaskSprite;

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
        //This update function is just for testing purposes in inspector will be removed onced mask switch feature is finalised
        MaskChange((int)maskSelected);
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
        switch (maskNo)
        {
            case 1:
                playerMask.sprite = warMaskSprite;
                maskSelected = Masks.warMask;
                break;
            case 2:
                playerMask.sprite = natureMaskSprite;
                maskSelected = Masks.natureMask;
                break;
            case 3:
                playerMask.sprite = seaMaskSprite;
                maskSelected = Masks.seaMask;
                break;
            case 4:
                playerMask.sprite = energyMaskSprite;
                maskSelected = Masks.energyMask;
                break;
        }
    }

    #endregion
}
