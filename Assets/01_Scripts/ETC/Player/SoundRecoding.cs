using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRecoding : MonoBehaviour
{
    [SerializeField]
    AudioSource _audioSource;

    AudioClip recordedClip;
    
    public void StartRecording()
    {
        Debug.Log("������");
        recordedClip = Microphone.Start(null, false, 10, 44100);
    }

    public void StopRecording()
    {
        Debug.Log("���� ��");
        Microphone.End(null);
        _audioSource.clip = recordedClip;
    }

    public void StartSound()
    {
        _audioSource.Play();
    }
}
