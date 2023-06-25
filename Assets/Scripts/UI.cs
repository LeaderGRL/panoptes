using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UI : MonoBehaviour, IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, Camera.main, out position);
        transform.localPosition = position;
    }

    public void Close()
    {
        LeanTween.scale(gameObject, Vector3.zero, 0.2f).setEase(LeanTweenType.easeInBack);
    }
}
