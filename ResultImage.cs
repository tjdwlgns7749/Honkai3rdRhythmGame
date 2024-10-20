using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultImage : MonoBehaviour
{
    public Image backgroundimg;


    private void OnEnable()
    {
        backgroundimg.sprite = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].spriteBackGround;
    }
}
