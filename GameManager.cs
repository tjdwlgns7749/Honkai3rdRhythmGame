using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum Check
{
    PERFECT = 0,
    GOOD = 1,
    MISS = 2
}
public class GameManager : MonoBehaviour
{
    static GameManager instance = null;

    [SerializeField] UIManager uiMgr;
    [SerializeField] NoteManager noteMgr;
    [SerializeField] AudioManager audioMgr;
    [SerializeField] CharacterManager characterMgr;
    public UIManager UIManager => uiMgr;
    public NoteManager NoteManager => noteMgr;
    public AudioManager AudioManager => audioMgr;
    public CharacterManager CharacterManager => characterMgr;



    //PlayerInfo
    public int Hp { get; private set; }
    public int Combo { get; private set; }
    public int Score { get; private set; }
    public int MaxCombo { get; private set; }
    public int MaxScore { get; private set; }
    public int PerCount { get; private set; }
    public int GoodCount { get; private set; }
    public int MissCount { get; private set; }
    public int Data_MaxCombo { get; private set; }

    int ScoreUp = 0;

    public bool m_bPlayCheck { get; set; }
    bool m_bPause;

    float widthScaleFactor;
    float heightScaleFactor;

    Character resultcharacter;

    public static GameManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = FindObjectOfType<GameManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
    void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        GameSetting();
    }

    void GameSetting()//게임세팅
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float referenceWidth = 1600.0f;
        float referenceHeight = 900.0f;

        widthScaleFactor = screenWidth / referenceWidth;
        heightScaleFactor = screenHeight / referenceHeight;

        StartSetting();//초기값설정
        uiMgr.ObjectActive("MainMenuUI", true);
        audioMgr.Stopvideo();
        audioMgr.PlayBGM();
    }

    public void Pause()
    {
        if (m_bPlayCheck)
        {
            if (!m_bPause)
            {
                m_bPause = true;
                uiMgr.ObjectActive("PauseUI", true);
                uiMgr.PauseActive(false);
                audioMgr.Pause();
                Time.timeScale = 0;
            }
            else
            {
                uiMgr.ObjectActive("PauseUI", false);
                uiMgr.CountDown();
                StartCoroutine(UnPause());
            }
        }
    }

    IEnumerator UnPause()
    {
        yield return new WaitForSecondsRealtime(3.0f);
        m_bPause = false;
        uiMgr.PauseActive(true);
        audioMgr.Pause_PlaySong();
        audioMgr.PlayVideo();
        Time.timeScale = 1;

        yield return null;
    }

    public void Restart()
    {
        GameDataReset();
        StartSetting();
        uiMgr.ObjectActive("PauseUI", false);
        uiMgr.PauseActive(true);
        Time.timeScale = 1;
        StartCoroutine(GameStart3second());
    }

    public void Pause_EXIT()
    {
        GameDataReset();
        StartSetting();
        uiMgr.ObjectActive("PauseUI", false);
        uiMgr.CanvasActive("Notecanvas", false); ;
        uiMgr.CanvasActive("GameUI", false);
        uiMgr.ObjectActive("MusicSelectUI", true);
        audioMgr.PlaySong();
        uiMgr.GetMusicInfo();
        uiMgr.PauseActive(true);
        Time.timeScale = 1;
    }

    public void GameOver_Restart()
    {
        GameDataReset();
        StartSetting();
        uiMgr.ObjectActive("GameOverUI", false);
        Time.timeScale = 1;
        StartCoroutine(GameStart3second());
    }

    public void GameOver_EXIT()
    {
        GameDataReset();
        StartSetting();
        uiMgr.CanvasActive("Notecanvas", false); ;
        uiMgr.CanvasActive("GameUI", false);
        uiMgr.ObjectActive("GameOverUI", false);
        uiMgr.ObjectActive("MusicSelectUI", true);
        uiMgr.GetMusicInfo();
        audioMgr.PlaySong();
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        uiMgr.ObjectActive("GameOverUI", true);
        audioMgr.Stop();
        audioMgr.Stopvideo();
        Time.timeScale = 0;
    }

    public IEnumerator Result()
    {
        if (Score > MaxScore)
            MaxScore = Score;
        if (MaxCombo > Data_MaxCombo)
            Data_MaxCombo = MaxCombo;
        
        GameDataReset();
        audioMgr.SetData();
        audioMgr.SaveData();
        yield return new WaitForSeconds(3.0f);

        uiMgr.CanvasActive("Notecanvas", false); ;
        uiMgr.CanvasActive("GameUI", false);
        uiMgr.ObjectActive("ResultUI", true);
        uiMgr.ResultImg(true);
        resultcharacter = characterMgr.ResultGetCharacter();

        yield return null;
    }

    public void ResultExit()
    {
        resultcharacter.ResultDestroy();
        StartSetting();
        uiMgr.ObjectActive("ResultUI", false);
        uiMgr.ResultImg(false);
        uiMgr.ObjectActive("MusicSelectUI", true);
        audioMgr.PlaySong();
        uiMgr.GetMusicInfo();
    }

    void PlayGame()
    {
        uiMgr.CanvasActive("Notecanvas", true);//노트UI
        uiMgr.CanvasActive("GameUI", true);//노트UI
        StartCoroutine(GameStart3second());
    }

    IEnumerator GameStart3second()
    {
        uiMgr.CountDown();
        uiMgr.ViewRestart();
        UIManager.PlayerInfoUIRestart();
        yield return new WaitForSeconds(3.0f);
        noteMgr.GetNoteData(widthScaleFactor);//게임시작할때마다 BPM주기
        audioMgr.PlaySong();//음악재생
        audioMgr.PlayVideo();
        uiMgr.ActiveVideo(true);
        m_bPlayCheck = true;
        yield return null;
    }

    void GameDataReset()
    {
        noteMgr.Restart();
        characterMgr.Restart();
        audioMgr.Stop();
        audioMgr.Stopvideo();
        uiMgr.ActiveVideo(false);
    }


    public void GetInfo(int hp, int combo, int score)
    {
        Hp -= hp;
        Combo = combo == 0 ? 0 : Combo + combo;
        Score += score;
    }

    //Player함수
    public void GetCheck(Check eCheck)
    {
        switch (eCheck)
        {
            case Check.PERFECT:
                GetInfo(0, 1, 100);
                ScoreUp = 100;
                PerCount++;
                Hp += 10;
                if (Hp >= 100)
                    Hp = 100;
                break;
            case Check.GOOD:
                GetInfo(0, 1, 50);
                ScoreUp = 50;
                GoodCount++;
                Hp += 5;
                if (Hp >= 100)
                    Hp = 100;
                break;
            case Check.MISS:
                GetInfo(-1, 0, 0);
                ScoreUp = 0;
                MissCount++;
                Hp -= 21;
                break;
        }
        if (Hp <= 0)
            GameOver();
        if (Combo > MaxCombo)
            MaxCombo = Combo;
        uiMgr.GetView(eCheck);
        uiMgr.PlayerInfoUi(Hp, Combo, Score, ScoreUp);
    }

    public void StartSetting()//초기값설정
    {
        m_bPause = false;
        m_bPlayCheck = false;
        Hp = 100;
        Combo = 0;
        Score = 0;
        MaxCombo = 0;
        PerCount = 0;
        GoodCount = 0;
        MissCount = 0;
    }

    IEnumerator Loading(System.Action function)
    {
        audioMgr.Stop();
        uiMgr.ObjectActive("LoadingUI", true);
        yield return new WaitForSeconds(4.0f);
        uiMgr.ObjectActive("LoadingUI", false);
        function();

        yield return null;
    }

    void MainMenu_Play2()
    {
        uiMgr.ObjectActive("MainMenuUI", false);
        uiMgr.ObjectActive("MusicSelectUI", true);
        audioMgr.PlaySong(audioMgr.MusicDataList[audioMgr.MusicIndexNum].SelectTime);
        uiMgr.MusicInfo(audioMgr.MusicDataList[audioMgr.MusicIndexNum].spriteBackGround);
    }

    void MusicSelectMenu_Play2()
    {
        uiMgr.ObjectActive("MusicSelectUI", false);
        PlayGame();
    }

    //버튼함수

    public void MainMenu_Play()//메인메뉴_시작버튼
    {
        StartCoroutine(Loading(MainMenu_Play2));
    }

    public void Option()
    {
        if (m_bPause)
            uiMgr.ObjectActive("PauseUI", false);
        uiMgr.ObjectActive("OptionUI", true);
    }

    public void Option_Exit()
    {
        if (m_bPause)
            uiMgr.ObjectActive("PauseUI", true);
        uiMgr.ObjectActive("OptionUI", false);
    }

    public void MainMenu_Exit()//메인메뉴_게임종료버튼
    {
        PlayerPrefs.DeleteAll();
        Application.Quit();
    }

    public void MusicSelectMenu_Play()
    {
        audioMgr.Stop();
        StartCoroutine(Loading(MusicSelectMenu_Play2));
    }

    public void MusicSelectMenu_Exit()//노래선택화면_나가기버튼
    {
        uiMgr.ObjectActive("MusicSelectUI", false);
        uiMgr.ObjectActive("MainMenuUI", true);
        audioMgr.PlayBGM();
        audioMgr.IndexZero();
 
    }

    public void MusicSelectMenu_Before()
    {
        audioMgr.Stop();
        audioMgr.IndexNumberDown();
        audioMgr.PlaySong(audioMgr.MusicDataList[audioMgr.MusicIndexNum].SelectTime);
        uiMgr.MusicInfo(audioMgr.MusicDataList[audioMgr.MusicIndexNum].spriteBackGround);
    }

    public void MusicSelectMenu_Next()
    {
        audioMgr.Stop();
        audioMgr.IndexNumberUp();
        audioMgr.PlaySong(audioMgr.MusicDataList[audioMgr.MusicIndexNum].SelectTime);
        uiMgr.MusicInfo(audioMgr.MusicDataList[audioMgr.MusicIndexNum].spriteBackGround);
    }
}

