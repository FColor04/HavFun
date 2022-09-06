using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Serializable]
    public struct Recipe
    {
        public Sprite ingredientA;
        public Sprite ingredientB;
        public Sprite output;
    }

    public List<Recipe> recipes;
    public List<Sprite> knownItems = new ();
    private List<Sprite> _lastKnownItems = new ();

    public bool held;
    private bool _animating;
    public Image itemAImage;
    public Image itemBImage;
    public Sprite itemA;
    public Sprite itemB;
    
    public Canvas canvas;
    public Transform itemParent;

    private void Awake()
    {
        Instance = this;
        foreach (Transform item in itemParent)
        {
            item.GetComponent<ItemDragHandler>().fresh = true;
        }
    }

    private void OnValidate()
    {
        if (Application.isPlaying) return;
        CreateItems();
    }

    private void CreateItems()
    {
        foreach (Transform child in itemParent)
        {
            if (!Application.isPlaying)
                StartCoroutine(DelayedDestroy(child.gameObject));
            else
                Destroy(child.gameObject);
        }
        
        foreach (var item in knownItems)
        {
            var itemImage = new GameObject(item.name, typeof(Image), typeof(ItemDragHandler));
            itemImage.GetComponent<Image>().sprite = item;
            itemImage.transform.SetParent(itemParent);
            itemImage.transform.localScale = Vector3.one;
            itemImage.transform.localPosition = Vector3.zero;
            itemImage.GetComponent<ItemDragHandler>().OnEnable();
            if (!_lastKnownItems.Contains(item))
                itemImage.GetComponent<ItemDragHandler>().fresh = true;
        }

        _lastKnownItems = knownItems;
    }

    IEnumerator DelayedDestroy(GameObject go)
    {
        yield return null;
        DestroyImmediate(go);
    }

    void Update()
    {
        itemAImage.enabled = itemA != null;
        itemBImage.enabled = itemB != null;
        
        if (held)
        {
            if (itemAImage.enabled)
                itemAImage.sprite = itemA;
            if (itemBImage.enabled)
                itemBImage.sprite = itemB;

            if (_animating) return;
            
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform,
                Input.mousePosition, canvas.worldCamera, out var pos);
            
            if (itemBImage.enabled)
            {
                itemBImage.transform.position = canvas.transform.TransformPoint(pos);
            }
            else
            {
                itemAImage.transform.position = canvas.transform.TransformPoint(pos);
            }
        }

        foreach (Transform child in itemParent)
        {
            if (child.GetComponent<ItemDragHandler>().fresh)
            {
                child.localScale = Vector3.one + Vector3.one * (0.2f * Mathf.Sin(Time.realtimeSinceStartup));
            }
        }
    }

    public IEnumerator VerifyRecipe()
    {
        _animating = true;
        
        if (itemB == null)
        {
            while (Vector2.Distance(itemAImage.rectTransform.anchoredPosition, Vector2.zero) >= 100f)
            {
                itemAImage.rectTransform.anchoredPosition = Vector2.Lerp(itemAImage.rectTransform.anchoredPosition, Vector2.zero, Time.deltaTime * 10f);
                yield return null;
            }
        }
        else
        {
            while (Vector2.Distance(itemBImage.rectTransform.anchoredPosition, Vector2.zero) >= 100f)
            {
                itemBImage.rectTransform.anchoredPosition = Vector2.Lerp(itemBImage.rectTransform.anchoredPosition, Vector2.zero, Time.deltaTime * 10f);
                yield return null;
            }
        }

        _animating = false;
        
        held = false;
        if (itemA != null && itemB != null)
        {
            var matchingRecipe = recipes.FirstOrDefault(recipe => recipe.ingredientA == itemA && recipe.ingredientB == itemB);
            if (matchingRecipe.output == null)
                matchingRecipe = recipes.FirstOrDefault(recipe => recipe.ingredientB == itemA && recipe.ingredientA == itemB);
            if (matchingRecipe.output != null)
            {
                if (!knownItems.Contains(matchingRecipe.output))
                    knownItems.Add(matchingRecipe.output);
            }

            itemA = null;
            itemB = null;
            CreateItems();
        }
        
        
    }
}
