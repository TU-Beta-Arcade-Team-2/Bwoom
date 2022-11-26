using UnityEngine;
using UnityEngine.UI;

public class UIGamePause : MonoBehaviour
{
    [SerializeField] private Button m_continueButton;
    [SerializeField] private Button m_restartButton;
    [SerializeField] private Button m_optionsButton;
    [SerializeField] private Button m_quitButton;

    public void OnContinuePressed()
    {
        GameManager.Instance.PauseGame();
        UIManager.Instance.PlayUiClick();
    }

    public void OnRestartPressed()
    {
        GameManager.Instance.RestartGame();
        UIManager.Instance.PlayUiClick();
    }

    public void OnOptionsPressed()
    {
        UIManager.Instance.ShowOptionsMenu();
        UIManager.Instance.PlayUiClick();
    }

    public void OnQuitPressed()
    {
        UIManager.Instance.QuitGame();
        UIManager.Instance.PlayUiClick();
    }
}
