using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource SoundEffectSource, MusicSource;
    
    public float SoundVolume = 0.5f;

    public List<SoundInfo> PlayerSound;

    public List<SoundInfo> EnemySound;

    public List<SoundInfo> SkillSound;

    public List<SoundInfo> UISound;

    public List<SoundInfo> OtherSound;

    // single ton
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake() 
    {
        _instance = this;
    }

    // Body code
    void Start()
    {
        
    }

    void Update()
    {
        SoundEffectSource.volume = SoundVolume;
        MusicSource.volume = SoundVolume;
    }

    public SoundInfo GetPlayerSound(string name)
    {
        return PlayerSound.Find(x => x.NameSound == name);
    }

    public SoundInfo GetEnemySound(string name)
    {
        return EnemySound.Find(x => x.NameSound == name);
    }

    public SoundInfo GetSkillSound(string name)
    {
        return SkillSound.Find(x => x.NameSound == name);
    }

    public SoundInfo GetUISound(string name)
    {
        return UISound.Find(x => x.NameSound == name);
    }

    public SoundInfo GetOtherSound(string name)
    {
        return OtherSound.Find(x => x.NameSound == name);
    }
}

[System.Serializable]
public class SoundInfo
{
    public string NameSound;

    public AudioClip Au;

    public void AudioPlay()
    {
        if(Au != null)
            SoundManager.Instance.SoundEffectSource.PlayOneShot(Au);
    }
}