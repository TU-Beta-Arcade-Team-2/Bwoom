using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private AudioSource m_musicSource;
    [SerializeField] private AudioSource m_sfxSource;

    private readonly Dictionary<string, AudioClip> m_audioClips = new();

    protected override void InternalInit()
    {
        AudioClip[] allSounds = Resources.LoadAll<AudioClip>("Audio");

        foreach (AudioClip clip in allSounds)
        {
            m_audioClips.Add(clip.name, clip);
        }
    }

    public void PlayMusic(string musicName, bool shouldLoop, bool playAtCurrentTimeStamp = false)
    {
        BetterDebugging.Assert(m_audioClips.Keys.Contains(musicName), $"UNRECOGNISED TRACK NAME {musicName}");

        float timeStamp = 0f;

        if (playAtCurrentTimeStamp)
        {
            timeStamp = m_musicSource.time;
        }

        m_musicSource.clip = m_audioClips[musicName];
        m_musicSource.loop = shouldLoop;
        m_musicSource.time = timeStamp;
        m_musicSource.Play();
    }

    public void PlaySfx(string sfxName)
    {
        BetterDebugging.Assert(m_audioClips.Keys.Contains(sfxName), $"UNRECOGNISED SOUND EFFECT {sfxName}");
        m_sfxSource.PlayOneShot(m_audioClips[sfxName]);
    }

    public void OnMasterVolumeChanged(float newValue)
    {
        AudioListener.volume = newValue;
    }

    public void OnMusicVolumeChanged(float newValue)
    {
        m_musicSource.volume = newValue;
    }


    public void OnSfxVolumeChanged(float newValue)
    {
        m_sfxSource.volume = newValue;
    }
}
