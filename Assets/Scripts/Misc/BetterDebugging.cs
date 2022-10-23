#define INSTANTIATE_INGAME_TEXT
// #undef INSTANTIATE_INGAME_TEXT
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;


public class BetterDebugging : Singleton<BetterDebugging>
{
    public enum eDebugLevel
    {
        Error = -1,
        Warning = 0,
        Log = 1,
        Message = 2
    }

    public void DebugLog(
        string debugText, 
        eDebugLevel level = eDebugLevel.Log, 
        [CallerFilePath] string originFile = null,
        [CallerMemberName] string functionName = null,
        [CallerLineNumber] int originLineNumber = 0)
    {
        string debugString;
        switch (level)
        {
            case eDebugLevel.Error:
                debugString = $"<color=red><b>ERROR:</b> {debugText}</color>\tFile: {GetFileName(originFile)}::{functionName}():{originLineNumber}";
                break;
            case eDebugLevel.Warning:
                debugString = $"<color=yellow><b>WARNING:</b> <i>{debugText}</i></color>\tFile: {GetFileName(originFile)}::{functionName}():{originLineNumber}";
                break;
            case eDebugLevel.Message:
                debugString = $"<color=blue><b>WARNING:</b> <i>{debugText}</i></color>\tFile: {GetFileName(originFile)}::{functionName}():{originLineNumber}";
                break;
            case eDebugLevel.Log:
            default:
                debugString = debugText;
                break;
        }

        Debug.Log(debugString);
    }

    // TODO: textOrigin doesn't need to be provided if parent is there, so I need to create
    // some better overloads for this function... 
    public void SpawnDebugText(string debugText,
        Vector3 textOrigin,
        float lifeTime,
        Transform parent = null,
        eDebugLevel debugLevel = eDebugLevel.Log)
    {
#if INSTANTIATE_INGAME_TEXT
        GameObject go;
        if (parent)
        {
            go = Instantiate(new GameObject("DebugText"), textOrigin, Quaternion.identity, parent);
        }
        else
        {
            go = Instantiate(new GameObject("DebugText"), textOrigin, Quaternion.identity);
        }

        go.transform.position = textOrigin;

        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        TextMeshPro text = go.AddComponent(typeof(TextMeshPro)) as TextMeshPro;

        if (text != null)
        {
            text.text = debugText;

            text.color = Color.white;

            switch (debugLevel)
            {
                case eDebugLevel.Error:
                    text.color = Color.red;
                    break;
                case eDebugLevel.Warning:
                    text.color = Color.yellow;
                    break;
                case eDebugLevel.Message:
                    text.color = Color.blue;
                    break;
            }
        }
        else
        {
            DebugLog("Problem creating spawned in text!", eDebugLevel.Error);
        }

        Destroy(go, lifeTime);
#endif
    }

    public void Assert(bool condition, 
        string debugString = "",
        [CallerFilePath] string originFile = null,
        [CallerMemberName] string functionName = null,
        [CallerLineNumber] int originLineNumber = 0)
    {
        if (!condition)
        {
            DebugLog($"ASSERTION FAILED: {debugString}", eDebugLevel.Error, originFile, functionName, originLineNumber);

            Debug.Break();
        }
    }

    private string GetFileName(string fullPath)
    {
        return fullPath?.Split('\\').Last();
    }
}
