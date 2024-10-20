using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MusicSelectInfo : MonoBehaviour
{
    public TextMeshProUGUI MaxComboText;
    public TextMeshProUGUI ScoreText;

    private void OnEnable()
    {
        MaxComboText.text = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].musicMaxCombo.ToString();
        ScoreText.text = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].musicScore.ToString();
    }

    public void GetMusicSelectInfo()
    {
        MaxComboText.text = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].musicMaxCombo.ToString();
        ScoreText.text = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].musicScore.ToString();
    }
}
