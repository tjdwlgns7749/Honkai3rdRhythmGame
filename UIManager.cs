using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager instance = null;

    public int ReadyCount = 0;
    public Transform posX;

    public CountDown m_Countdown;
    public Combo m_combo;
    public Score m_score;
    public HP m_hp;
    public Button m_Pausebutton;
    public ResultInfo m_resultinfo;
    public ImgChange m_imageChange;
    public titleText m_titleText;
    public MusicSelectInfo m_musicselectinfo;
    public VideoCheck m_videocheck;
    public Image background;
    public Loading m_loading;
    public VolumOption m_volumoption;

    Queue<Checkview> PerfectPool = new Queue<Checkview>();
    Queue<Checkview> GoodPool = new Queue<Checkview>();
    Queue<Checkview> MissPool = new Queue<Checkview>();
    List<Checkview> ActiveListView = new List<Checkview>();

    [SerializeField] private Checkview PerfectPrefab;
    [SerializeField] private Checkview GoodPrefab;
    [SerializeField] private Checkview MissPrefab;


    public static UIManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = FindObjectOfType<UIManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        GameSetting(ReadyCount);
        GetMusicInfo();
    }


    public void ObjectActive(string str, bool Active)
    {
        var gameobject = this.transform.Find(str);
        gameobject.gameObject.SetActive(Active);
    }

    public void CanvasActive(string str, bool Active)
    {
        var gameobject = this.transform.Find(str);
        gameobject.GetComponent<Canvas>().enabled = Active;
    }

    public void PauseActive(bool Active)
    {
        m_Pausebutton.gameObject.SetActive(Active);

    }

    public void ResultImg(bool Active)
    {
        m_videocheck.ImgActive(Active);
    }

    public void ActiveVideo(bool Active)
    {
        m_videocheck.VideoActive(Active);
    }

    public void MusicInfo(Sprite sprite)
    {
        m_imageChange.GetIMG();
        m_titleText.GetTitle();
        m_musicselectinfo.GetMusicSelectInfo();
        background.sprite = sprite;
    }

    public void GetMusicInfo()
    {
        m_musicselectinfo.GetMusicSelectInfo();
    }

    public void CountDown()
    {
        m_Countdown.gameObject.SetActive(true);
    }

    

    public void PlayerInfoUi(int hp, int combo, int score, int scoreup)
    {
        m_hp.GetHP(hp);
        m_combo.GetCombo(combo);
        m_score.GetScore(score, scoreup);
    }

    public void PlayerInfoUIRestart()
    {
        m_hp.Setting();
        m_combo.Setting();
        m_score.Setting();
    }

    public void GetView(Check check)
    {
        switch (check)
        {
            case Check.PERFECT:
                var newPer = PerfectPool.Dequeue();
                ActiveListView.Add(newPer);
                SetView(newPer);
                break;
            case Check.GOOD:
                var newGood = GoodPool.Dequeue();
                ActiveListView.Add(newGood);
                SetView(newGood);
                break;
            case Check.MISS:
                var newMiss = MissPool.Dequeue();
                ActiveListView.Add(newMiss);
                SetView(newMiss);
                break;
        }

    }

    public void SetView(Checkview newview)
    {
        newview.gameObject.SetActive(true);

    }

    public void ReturnObject(Checkview checkview)
    {
        checkview.gameObject.SetActive(false);
        checkview.SetPosX(posX);

        switch (checkview.eCheck)
        {
            case Check.PERFECT:
                PerfectPool.Enqueue(checkview);
                break;
            case Check.GOOD:
                GoodPool.Enqueue(checkview);
                break;
            case Check.MISS:
                MissPool.Enqueue(checkview);
                break;
        }

    }

    public void ViewRestart()
    {
        for (int i = 0; i < ActiveListView.Count; i++)
        {
            ActiveListView[i].gameObject.SetActive(false);
            ActiveListView[i].SetPosX(posX);
        }
    }

    void GameSetting(int count)
    {
        for (int i = 0; i < count; i++)
        {
            PerfectPool.Enqueue(CreateView(PerfectPrefab));
            GoodPool.Enqueue(CreateView(GoodPrefab));
            MissPool.Enqueue(CreateView(MissPrefab));
        }
    }

    Checkview CreateView(Checkview checkview)
    {
        var newView = Instantiate(checkview);
        newView.gameObject.SetActive(false);
        newView.transform.SetParent(transform.Find("GameUI"));
        newView.SetPosX(posX);

        newView.eCheck = checkview.eCheck;

        return newView;
    }

}

