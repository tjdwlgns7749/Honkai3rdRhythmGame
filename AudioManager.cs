using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public struct MusicData
{
    public AudioClip AudioClip;
    public string name;
    public Sprite Sprite;
    public int bpm;
    public int musicScore;
    public int musicMaxCombo;
    public VideoClip VideoClip;
    public Sprite spriteBackGround;
    public float SelectTime;
    public float realMusic;
}

public class AudioManager : MonoBehaviour
{
    static AudioManager instance = null;
    public GameManager gameMgr;

    public VideoPlayer videoPlayer;
    public RenderTexture renderTexture;
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    public MusicData[] MusicDataList;
    public AudioClip[] sfxClips;
    public AudioClip[] bgm;

    public int MusicIndexNum = 0;
    public int BGMIndexNum = 0;


    public static AudioManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = FindObjectOfType<AudioManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Start()
    {
        for (int i = 0; i < MusicDataList.Length; i++)
        {
            MusicDataList[i].musicMaxCombo = PlayerPrefs.GetInt(MusicDataList[i].name + "_MaxCombo");
            MusicDataList[i].musicScore = PlayerPrefs.GetInt(MusicDataList[i].name + "_Score");
        }
    }

    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        gameMgr = GameManager.Instance;
        bgmSource = GameObject.FindObjectOfType<AudioSource>();
    }

    private void Update()
    {
        if (!bgmSource.isPlaying && gameMgr.m_bPlayCheck && Time.timeScale == 1)//게임끝날때
        {
            gameMgr.m_bPlayCheck = false;
            StartCoroutine(gameMgr.Result());
        }
    }

    public void PlayBGM(float bgmsongTime = 0)
    {
        bgmSource.clip = bgm[BGMIndexNum];
        bgmSource.time = bgmsongTime;
        bgmSource.Play();
    }

    public void IndexZero()
    {
        MusicIndexNum = 0;
    }

    public void PlaySong(float songTime = 0)
    {
        //clip - length 전체길이
        //source - time 현재재생시간
        //bgmSource.isPlaying 재생bool 

        bgmSource.clip = MusicDataList[MusicIndexNum].AudioClip;
        bgmSource.time = songTime;
        bgmSource.Play();
    }

    public void Pause_PlaySong()
    {
        bgmSource.Play();
    }

    public void PlayVideo()
    {
        videoPlayer.clip = MusicDataList[MusicIndexNum].VideoClip;
        videoPlayer.Play();
    }

    public void PlaySFX(string bgmName)
    {
        for(int i =0;i<sfxClips.Length;i++)
        {
            if(bgmName == sfxClips[i].name) 
            {
                sfxSource.clip = sfxClips[i];
                sfxSource.Play();
            }

        }
    }

    public void Pause()
    {
        if (gameMgr.m_bPlayCheck)
        {
            bgmSource.clip = MusicDataList[MusicIndexNum].AudioClip;
            bgmSource.Pause();
            videoPlayer.clip = MusicDataList[MusicIndexNum].VideoClip;
            videoPlayer.Pause();
        }
    }

    public void Stop()
    {
        bgmSource.Stop();
    }

    public void Stopvideo()
    {
        videoPlayer.Stop();
        renderTexture.Release();
    }


    public void IndexNumberUp()
    {
        MusicIndexNum++;

        if (MusicDataList.Length == MusicIndexNum)
            MusicIndexNum = 0;
        
    }

    public void IndexNumberDown()
    {
        MusicIndexNum--;


        if (MusicIndexNum < 0)
            MusicIndexNum = MusicDataList.Length - 1;
    }

    public void SetData()
    {
        MusicDataList[MusicIndexNum].musicMaxCombo = gameMgr.Data_MaxCombo;
        MusicDataList[MusicIndexNum].musicScore = gameMgr.MaxScore;
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(MusicDataList[MusicIndexNum].name + "_MaxCombo", MusicDataList[MusicIndexNum].musicMaxCombo);
        PlayerPrefs.SetInt(MusicDataList[MusicIndexNum].name + "_Score", MusicDataList[MusicIndexNum].musicScore);
        PlayerPrefs.Save();
    }
}
