using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [SerializeField] Animator noteHitAnimator = null;
    string hit = "Hit";

    public void NoteHitEffect()
    {
        noteHitAnimator.SetTrigger(hit);
    }
}
