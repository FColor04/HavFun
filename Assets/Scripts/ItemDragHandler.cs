using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler
{
    public Sprite mySprite;
    public bool fresh;

    public void OnEnable()
    {
        mySprite = GetComponent<Image>().sprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance.held) return;
        
        GameManager.Instance.held = true;
        if (GameManager.Instance.itemA == null)
            GameManager.Instance.itemA = mySprite;
        else if (GameManager.Instance.itemB == null)
            GameManager.Instance.itemB = mySprite;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (GameManager.Instance.itemB != null)
                GameManager.Instance.itemB = null;
            else
                GameManager.Instance.itemA = null;
            return;
        }

        StartCoroutine(GameManager.Instance.VerifyRecipe());
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        fresh = false;
        transform.localScale = Vector3.one;
    }
}
