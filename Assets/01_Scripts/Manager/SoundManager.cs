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

            _audioSources[(int)Sound.Bgm].loop = true; // bgm 재생기는 무한 반복 재생
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

        if (type == Sound.Bgm) // BGM 배경음악 재생
        {
            AudioSource audioSource = _audioSources[(int)Sound.Bgm];
            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.pitch = pitch;
            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else // Effect 효과음 재생
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
    //        path = $"Sounds/{path}"; // ??Sound 폴더 안에 저장될 수 있도록

    //    AudioClip audioClip = null;

    //    if (type == Sound.Bgm) // BGM 배경음악 클립 붙이기
    //    {
    //        audioClip = Resources.Load<AudioClip>(path);
    //    }
    //    else // Effect 효과음 클립 붙이기
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
    //    // 재생기 전부 재생 스탑, 음반 빼기
    //    foreach (AudioSource audioSource in _audioSources)
    //    {
    //        audioSource.clip = null;
    //        audioSource.Stop();
    //    }
    //    // 효과음 Dictionary 비우기
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
            // -기준으로 뒤에 있음
            SoundType soundType = (SoundType)System.Enum.Parse(typeof(SoundType), mystring[1]);
            AddSoundClip(soundType, audioClip);
        }
    }
}
