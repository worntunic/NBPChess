using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeselectZone : MonoBehaviour, IPointerClickHandler
{
    public event Action onDeselectClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        onDeselectClicked?.Invoke();
    }
}
