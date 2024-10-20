using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerDownHandler
{

    public string SFXName;
    public void OnPointerDown(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(SFXName);
    }
}
    