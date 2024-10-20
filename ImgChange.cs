using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgChange : MonoBehaviour
{
    public Image image;


    private void OnEnable()
    {
        image.sprite = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].Sprite;
    }

    public void GetIMG()
    {
        image.sprite = AudioManager.Instance.MusicDataList[AudioManager.Instance.MusicIndexNum].Sprite;
    }

}

