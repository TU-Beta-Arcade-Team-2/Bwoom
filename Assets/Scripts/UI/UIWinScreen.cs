using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWinScreen : MonoBehaviour
{
    public void OnMainMenuButtonPressed()
    {
        SaveLoad.LoadLevel(StringConstants.TITLE_SCREEN_LEVEL);
    }
}
