using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCheck : MonoBehaviour
{
    BoxCollider boxCollider;

    //Gizmo
    public Transform posx;
    public Vector3 size;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        transform.position = posx.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
            other.gameObject.GetComponent<Character>().KickorMiss("Miss", true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;//Hit
        Gizmos.DrawWireCube(posx.position, size);
    }
}
