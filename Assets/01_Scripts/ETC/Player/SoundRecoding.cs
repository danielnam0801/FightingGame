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
        SoundManager.Instance.Save(soundType, recordedClip);
        SoundManager.Instance.Load(soundType);
    }

    public void StartSound()
    {
        _audioSource.Play();
    }
}
