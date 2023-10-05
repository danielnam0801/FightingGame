using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public enum SoundType
{
    Hit, // 맞았을 떄
    Attack, // 공격 키 눌렀을 때
    Block, // 방어 키 누르고 있을 때
    Win, //게임 이겼을 때
    Start, // 게임 시작할때
    BGM,
    MAXCOUNT,
}

public class SoundManager : Singleton<SoundManager>
{

    AudioSource[] _audioSources = new AudioSource[(int)SoundType.MAXCOUNT];

    public Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        GameObject root = this.gameObject;
        if(root == null)
        {
            Debug.Log("오디오 매니저가 없음");
            return;
        }

        string[] soundNames = System.Enum.GetNames(typeof(SoundType)); // "Bgm", "Effect"
        for (int i = 0; i < soundNames.Length - 1; i++)
        {
            GameObject go = new GameObject { name = soundNames[i] };
            _audioSources[i] = go.AddComponent<AudioSource>();
            _audioSources[i].playOnAwake = false;
            go.transform.parent = root.transform;
        }

        _audioSources[(int)SoundType.BGM].loop = true; // bgm 재생기는 무한 반복 재생
    }

    public void Save(SoundType soundType, AudioClip clip, PlayerType player = PlayerType.player1)
    {
        string filename = $"{player}-{Enum.GetName(typeof(SoundType), soundType)}";
        SavWav.Save(filename, clip);
    }

    public void Load(SoundType soundType, PlayerType player = PlayerType.player1)
    {
        string filename = $"{player}-{Enum.GetName(typeof(SoundType), soundType)}";
        StartCoroutine(LoadAudio(filename, _audioSources[(int)soundType]));
    }

    public void PlaySound(SoundType soundType)
    {
        _audioSources[(int)soundType].Play();
    }

    public void StopSound(SoundType soundType)
    {
        _audioSources[(int)soundType].Stop();
    }

    public IEnumerator<WWW> LoadAudio(string filename, AudioSource audioSource)
    {
        if (!String.IsNullOrEmpty(filename) && audioSource != null)
        {
            string path = GetPath(filename);

            if (File.Exists(path))
            {
                WWW www = new WWW("file:" + path);
                yield return www;
                audioSource.clip = www.GetAudioClip(false, false, AudioType.WAV);
                audioSource.clip.name = filename;
            }
        }
        yield break;
    }
    private string GetPath(string filename)
    {
        return Path.Combine(Application.persistentDataPath, filename.EndsWith(".wav") ? filename : filename + ".wav");
    }
}
