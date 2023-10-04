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
}

public class SoundManager : Singleton<SoundManager>
{
    AudioSource[] _audioSources = new AudioSource[(int)Sound.MaxCount];
    public Dictionary<SoundType, AudioClip> audios = new Dictionary<SoundType, AudioClip>();
    public SaveLoadWav saveLoadWav = new SaveLoadWav();

    public void Save(string filename, AudioClip clip, bool makeClipShort = true)
    {
        saveLoadWav.Save(filename, clip, makeClipShort);
    }

    public void Load(string path, AudioSource audioSource)
    {
        string path = saveLoad.GetPath(filename);

        if (File.Exists(path))
        {
            WWW www = new WWW("file:" + path);
            yield return www;

            audios.Add(www.GetAudioClip(false, false, AudioType.WAV);
            audioSource.clip.name = filename;
        }
    }

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Sound)); // "Bgm", "Effect"
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Sound.Bgm].loop = true; // bgm ������ ���� �ݺ� ���
        }
    }

    public void PlaySound(AudioSource audioSource, SoundType soundType)
    {
        audioSource.clip = audios[soundType];
        audioSource.Play();
    }

    public void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }

    public void Play(AudioClip audioClip, Sound type = Sound.Effect, float pitch = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Sound.Bgm) // BGM ������� ���
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // Effect ȿ���� ���
        {
            AudioSource audioSource = _audioSources[(int)Sound.Effect];
            audioSource.pitch = pitch;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void LoadAudio(string path)
    {
        StartCoroutine(loadAudio(path));
    }
    //public void Play(string path, Sound type = Sound.Effect, float pitch = 1.0f)
    //{
    //    AudioClip audioClip = GetOrAddAudioClip(path, type);
    //    Play(audioClip, type, pitch);
    //}

    //AudioClip GetOrAddAudioClip(string path, Sound type = Sound.Effect)
    //{
    //    if (path.Contains("Sounds/") == false)
    //        path = $"Sounds/{path}"; // ??Sound ���� �ȿ� ����� �� �ֵ���

    //    AudioClip audioClip = null;

    //    if (type == Sound.Bgm) // BGM ������� Ŭ�� ���̱�
    //    {
    //        audioClip = Resources.Load<AudioClip>(path);
    //    }
    //    else // Effect ȿ���� Ŭ�� ���̱�
    //    {
    //        if (_audioClips.TryGetValue(path, out audioClip) == false)
    //        {
    //            audioClip = Resources.Load<AudioClip>(path);
    //            _audioClips.Add(path, audioClip);
    //        }
    //    }

    //    if (audioClip == null)
    //        Debug.Log($"AudioClip Missing ! {path}");

    //    return audioClip;
    //}

    //public void Clear()
    //{
    //    // ����� ���� ��� ��ž, ���� ����
    //    foreach (AudioSource audioSource in _audioSources)
    //    {
    //        audioSource.clip = null;
    //        audioSource.Stop();
    //    }
    //    // ȿ���� Dictionary ����
    //    _audioClips.Clear();
    //}

    public void AddSoundClip(SoundType soundType, AudioClip clip)
    {
        if (audios.ContainsKey(soundType))
            audios[soundType] = clip;
        else
            audios.Add(soundType, clip);
    }

    IEnumerator loadAudio(string path)
    {
        //Load Audio
        AudioClip audioClip = null;
        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.WAV))
        {
            yield return uwr.SendWebRequest();
            if(uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                Debug.Log("Success");
                audioClip = DownloadHandlerAudioClip.GetContent(uwr);
            }
        }
        if (audioClip != null)
        {
            string[] mystring = path.Split();
            // -�������� �ڿ� ����
            SoundType soundType = (SoundType)System.Enum.Parse(typeof(SoundType), mystring[1]);
            AddSoundClip(soundType, audioClip);
        }
    }
}
