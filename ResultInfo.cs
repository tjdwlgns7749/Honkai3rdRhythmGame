using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultInfo : MonoBehaviour
{
    public TextMeshProUGUI MaxcomboText;
    public TextMeshProUGUI ResultScoreText;
    public TextMeshProUGUI PerCount;
    public TextMeshProUGUI GoodCount;
    public TextMeshProUGUI MissCount;

    private void OnEnable()
    {
        MaxcomboText.text = GameManager.Instance.MaxCombo.ToString();
        ResultScoreText.text = GameManager.Instance.Score.ToString();
        PerCount.text = GameManager.Instance.PerCount.ToString();
        GoodCount.text = GameManager.Instance.GoodCount.ToString();
        MissCount.text = GameManager.Instance.MissCount.ToString();
    }

    private void OnDisable()
    {
        MaxcomboText.text = "0";
        ResultScoreText.text = "0";
        PerCount.text = "0";
        GoodCount.text = "0";
        MissCount.text = "0";
    }

}
