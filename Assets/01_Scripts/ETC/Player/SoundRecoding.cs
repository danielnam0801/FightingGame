using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRecoding : MonoBehaviour
{
    [SerializeField]
    AudioSource _audioSource;
    public AudioClip recordedClip;
    public SoundType soundType;

    public void StartRecording()
    {
        Debug.Log("≥Ï¿Ω¡ﬂ");
        recordedClip = Microphone.Start(null, false, 10, 44100);
    }

    public void StopRecording()
    {
        Debug.Log("≥Ï¿Ω ≥°");
        Microphone.End(null);
        _audioSource.clip = recordedClip;
        SaveRecording();
    }

    private void SaveRecording()
    {
        string tempPath = $"Sound-{Enum.GetName(typeof(SoundType), soundType)}";
        
        SoundManager.Instance.saveLoadWav.Save(tempPath, recordedClip, false);
        SoundManager.Instance.LoadAudio(tempPath);
    }

    public void StartSound()
    {
        _audioSource.Play();
    }
}
