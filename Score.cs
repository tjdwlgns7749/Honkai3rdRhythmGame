using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public TextMeshProUGUI ScoretextMesh;
    public TextMeshProUGUI ScoreUpTextMesh;

    float StartTime = 0;
    bool bPlayGame = false;

    private void Start()
    {
        Setting();
    }

    private void Update()
    {
        if (bPlayGame)
        {
            if (Time.time - StartTime > 1.0f)
            {
                bPlayGame = false;
                ScoreUpTextMesh.text = "";
            }
        }

    }

    public void GetScore(int score, int scoreup)
    {
        ScoretextMesh.text = score.ToString();

        if (scoreup != 0)
            ScoreUpTextMesh.text = "+" + scoreup.ToString();
        else
            ScoreUpTextMesh.text = "";

        StartTime = Time.time;
        bPlayGame = true;
    }

    public void Setting()
    {
        ScoretextMesh.text = "0";
        ScoreUpTextMesh.text = "";
    }
}
