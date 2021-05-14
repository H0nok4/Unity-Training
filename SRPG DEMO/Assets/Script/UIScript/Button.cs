using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image image;
    public Color normalColor;
    public Color enterColor;
    public Color clickColor;
    private void Start()
    {
        image = GetComponent<Image>();
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        image.color = enterColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;
    }
}