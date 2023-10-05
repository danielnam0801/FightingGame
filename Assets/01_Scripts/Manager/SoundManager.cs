using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public enum SoundType
{
    Hit, // �¾��� ��
    Attack, // ���� Ű ������ ��
    Block, // ��� Ű ������ ���� ��
    Win, //���� �̰��� ��
    Start, // ���� �����Ҷ�
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
            Debug.Log("����� �Ŵ����� ����");
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

        _audioSources[(int)SoundType.BGM].loop = true; // bgm ������ ���� �ݺ� ���
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
