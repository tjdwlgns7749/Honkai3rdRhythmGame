using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteOutCheck : MonoBehaviour
{
    Check eCheck;

    private void Start()
    {
        eCheck = Check.MISS;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Note>().NoteDestroy();
        GameManager.Instance.GetCheck(eCheck);
    }

}
