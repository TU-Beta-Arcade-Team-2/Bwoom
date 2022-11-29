using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_buttonsHolder;
    [SerializeField] private GameObject m_continueButton;
    [SerializeField] private UIOptions m_optionsMenu;
    [SerializeField] private GameObject m_titleHolder;


    void Start()
    {
        m_optionsMenu.LoadOptions();

        // Show the Continue game button only if we've never saved
        m_continueButton.SetActive(SaveLoad.DoesSaveGameExist());
    }

    public void OnNewGamePressed()
    {
        SaveLoad.LoadLevel(StringConstants.NATURE_LEVEL);
    }

    public void OnContinueGamePressed()
    {
        SaveLoad.LoadGame();
        GameManager.SHOULD_LOAD_SAVE = true;
    }

    public void OnOptionsPressed()
    {
        m_buttonsHolder.SetActive(false);
        m_titleHolder.SetActive(false);

        m_optionsMenu.gameObject.SetActive(true);

        UIManager.Instance.PlayUiClick();
    }

    public void OnOptionsBackPressed()
    {
        m_buttonsHolder.SetActive(true);
        m_titleHolder.SetActive(true);

        m_optionsMenu.gameObject.SetActive(false);
        m_optionsMenu.SaveOptions();

        UIManager.Instance.PlayUiClick();
    }

    public void OnQuitPressed()
    {
        Application.Quit();
    }
}
