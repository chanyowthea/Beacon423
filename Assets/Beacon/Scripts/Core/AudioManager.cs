using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] AudioSource m_BGAudioSource;
    [SerializeField] AudioClip[] m_BGAudioClips;
    [SerializeField] int m_StartIndex;
    const string AUDIO_SOURCE = "Audio/AudioSource"; 

    private void Start()
    {
        UpdateBGAudio();
    }

    void UpdateBGAudio()
    {
        m_BGAudioSource.clip = m_BGAudioClips[PickIndex()];
        m_BGAudioSource.Play();
        //Facade.m_CurGame.DelayCall(m_BGAudioSource.clip.length - 10, UpdateBGAudio);
        StartCoroutine(Routine(m_BGAudioSource.clip.length - 10, UpdateBGAudio, true));
    }

    IEnumerator Routine(float time, Action onEveryLoopDone, bool repeat = false)
    {
        if (onEveryLoopDone == null)
        {
            yield break;
        }

        var wait = new WaitForSeconds(time);

        yield return wait;
        onEveryLoopDone();

        while (repeat)
        {
            yield return wait;
            onEveryLoopDone();
        }
    }

    int m_LastValue = -1;
    List<int> m_BGAudioList = new List<int>();
    public int PickIndex()
    {
        if (m_BGAudioList.Count == 0)
        {
            for (int i = 0, length = m_BGAudioClips.Length; i < length; i++)
            {
                if (m_LastValue == i)
                {
                    continue;
                }
                m_BGAudioList.Add(i);
            }
        }
        var index = 0;
        if (m_StartIndex >= 0)
        {
            index = m_StartIndex;
            m_StartIndex = -1; 
        }
        else
        {
            index = UnityEngine.Random.Range(0, m_BGAudioList.Count);
        }
        m_LastValue = m_BGAudioList[index];
        m_BGAudioList.RemoveAt(index);
        return m_LastValue;
    }

    public void PlayOneShot(string resPath)
    {
        var source = ResourceManager.Instance.LoadAndInstantiate<AudioSource>(AUDIO_SOURCE);
        source.clip = ResourceManager.Instance.LoadObject<AudioClip>(resPath);
        source.Play();
        //Facade.m_CurGame.DelayCall(source.clip.length, () => Destroy(source.gameObject));
        StartCoroutine(Routine(source.clip.length, () => Destroy(source.gameObject)));
    }
}
