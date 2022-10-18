using UnityEngine;

/// <summary>
/// My properly templated Singleton Class I made a few months ago... To Instantiate a singleton, inherit from this class with the Type Param of the class
/// you want to become a singleton e.g. class MusicManager : Singleton MusicManager - Tom :)
/// </summary>
/// <typeparam name="T">The templated type to instantiate</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T instance = null;

    public static T Instance
    {
        get
        {
            instance ??= FindObjectOfType(typeof(T)) as T;

            instance ??= new GameObject(typeof(T).ToString(), typeof(T)).GetComponent<T>();

            return instance;
        }
    }

    void OnApplicationQuit()
    {
        instance = null;
    }
}