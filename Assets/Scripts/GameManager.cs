using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct Recipe
    {
        public Sprite ingredientA;
        public Sprite ingredientB;
        public Sprite output;
    }

    public List<Recipe> recipes;
    public List<Sprite> knownItems;
    
    void Update()
    {
        
    }
}
