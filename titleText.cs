using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class titleText : MonoBehaviour
{
    public TextMeshProUGUI title;

    private void OnEnable()
    {
        title.text = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].name;
    }

    public void GetTitle()
    {
        title.text = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].name;
    }
}
