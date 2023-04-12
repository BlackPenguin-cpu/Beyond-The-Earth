using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SoundType
{
    SFX,
    BGM
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    [SerializeField]
    private AudioSource BgmSource;
    [SerializeField]
    private AudioSource SfxSource;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);

        AudioClip[] clips = Resources.LoadAll<AudioClip>("SoundClips/");

        for (int i = 0; i < clips.Length; i++)
            audioClips.Add(clips[i].name, clips[i]);

        BgmSource.loop = true;
        UISoundAccept();
    }
    private void OnLevelWasLoaded(int level)
    {
        UISoundAccept();
    }

    private void UISoundAccept()
    {
        Button[] btn = Resources.FindObjectsOfTypeAll<Button>();
        for (int i = 0; i < btn.Length; i++)
        {
            btn[i].onClick.AddListener(() => SoundManager.instance.PlaySound("UIClick"));
        }
    }

    public void PlaySound(string clip, SoundType type = SoundType.SFX, float volume = 0.5f, float pitch = 1)
    {
        if (type == SoundType.BGM)
        {
            BgmSource.clip = audioClips[clip];
            BgmSource.pitch = pitch;
            BgmSource.volume = volume;
            BgmSource.Play();
        }
        else
        {
            SfxSource.pitch = pitch;
            SfxSource.PlayOneShot(audioClips[clip], volume);
        }
    }
}
