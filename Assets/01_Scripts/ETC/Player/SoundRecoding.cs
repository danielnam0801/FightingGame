using UnityEngine;

public class SoundRecoding
{
    public SoundType soundType;
    private AudioClip recordedClip;
    
    public SoundRecoding(SoundType soundType)
    {
        this.soundType = soundType;
    }

    public void StartRecording()
    {
        Debug.Log("≥Ï¿Ω¡ﬂ");
        recordedClip = Microphone.Start(null, false, 10, 44100);
    }

    public void StopRecording()
    {
        Debug.Log($"{recordedClip}, {soundType}");
        Microphone.End(null);
        SaveRecording();
    }

    private void SaveRecording()
    {
        SoundManager.Instance.Save(soundType, recordedClip);
        SoundManager.Instance.Load(soundType);
    }
}
