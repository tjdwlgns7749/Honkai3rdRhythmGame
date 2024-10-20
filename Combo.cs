using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combo : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private void Start()
    {
        Setting();
    }

    public void GetCombo(int combo)
    {
        if (combo == 0)
        {
            this.GetComponent<Canvas>().enabled = false;
            textMesh.text = "";
        }
        else
        {
            this.GetComponent<Canvas>().enabled = true;
            textMesh.text = combo.ToString();
        }
    }

    public void Setting()
    {
        this.GetComponent<Canvas>().enabled = false;
    }
}
