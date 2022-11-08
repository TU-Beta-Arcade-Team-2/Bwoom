using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject GameHud;
    [SerializeField] private GameObject DeathHud;

    [SerializeField] private GameObject RadialHudCanvas;
    [SerializeField] private Image RadialHealthBar;
    [SerializeField] private Image RadialMaskIcon;
    [SerializeField] private GameObject HorizontalHudCanvas;
    [SerializeField] private Image HorizontalHealthBar;
    [SerializeField] private Image HorizontalMaskIcon;
    private bool altHud;

    [SerializeField] private InputAction gameHudAltButton;
 
    private void OnEnable()
    {
        gameHudAltButton.Enable();
    }

    private void OnDisable()
    {
        gameHudAltButton.Disable();
    }

    private void Start()
    {
        gameHudAltButton.performed += _ => AltSwitch();
        GameHud.SetActive(true);
        DeathHud.SetActive(false);
    }

    private void AltSwitch()
    {
        if (altHud)
        {
            FindObjectOfType<PlayerStats>().Switch(RadialHealthBar, RadialMaskIcon);
            RadialHudCanvas.SetActive(true);
            HorizontalHudCanvas.SetActive(false);
            altHud = false;
            return;
        }

        RadialHudCanvas.SetActive(false);
        HorizontalHudCanvas.SetActive(true);
        FindObjectOfType<PlayerStats>().Switch(HorizontalHealthBar, HorizontalMaskIcon);
        altHud = true;
    }

    public void Death()
    {
        GameHud.SetActive(false);
        DeathHud.SetActive(true);
    }

    public void Continue_Button()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Quit_Button()
    {
        SceneManager.LoadScene("TitleScreen");
    }
}
