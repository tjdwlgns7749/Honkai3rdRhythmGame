using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultScore : MonoBehaviour
{
    public TextMeshProUGUI ResultScoreText;

    private void OnEnable()
    {
        ResultScoreText.text = GameManager.Instance.Score.ToString();
    }

    private void OnDisable()
    {
        ResultScoreText.text = "0";
    }
}
