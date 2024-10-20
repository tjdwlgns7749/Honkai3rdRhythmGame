using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountDown : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public float m_Countdown = 3.0f;

    private void OnEnable()
    {
        StartCoroutine(count());
    }
    private void OnDisable()
    {
        m_Countdown = 3.0f;
    }

    IEnumerator count()
    {
        m_Countdown = 3.0f;
        textMesh.text = Mathf.Round(m_Countdown).ToString();
        yield return new WaitForSecondsRealtime(1.0f);

        m_Countdown = 2.0f;
        textMesh.text = Mathf.Round(m_Countdown).ToString();
        yield return new WaitForSecondsRealtime(1.0f);

        m_Countdown = 1.0f;
        textMesh.text = Mathf.Round(m_Countdown).ToString();
        yield return new WaitForSecondsRealtime(1.0f);

        textMesh.text = "Start";
        yield return new WaitForSecondsRealtime(1.0f);

        yield return null;

        this.gameObject.SetActive(false);
    }

}
