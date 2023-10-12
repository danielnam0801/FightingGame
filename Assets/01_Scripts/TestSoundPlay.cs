using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoundPlay : MonoBehaviour
{
    public SoundType soundType;

    public void Play()
    {
        SoundManager.Instance.PlaySound(soundType);
    }
}
