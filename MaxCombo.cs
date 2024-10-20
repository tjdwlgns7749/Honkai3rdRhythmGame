using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaxCombo : MonoBehaviour
{
    public TextMeshProUGUI MaxcomboText;

    private void OnEnable()
    {
        MaxcomboText.text = GameManager.Instance.MaxCombo.ToString();
    }

    private void OnDisable()
    {
        MaxcomboText.text = "0";
    }

}
