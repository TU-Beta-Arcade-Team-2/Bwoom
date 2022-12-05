using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(StringConstants.PLAYER_TAG))
        {
            SaveLoad.LoadLevel("WinScene");
        }
    }
}
