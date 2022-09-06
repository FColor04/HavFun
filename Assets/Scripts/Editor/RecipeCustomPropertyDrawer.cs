using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(GameManager.Recipe))]
public class RecipeCustomPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        var r1 = new Rect(position.x, position.y, position.height, position.height);
        var r2 = new Rect(position.x + 4 + position.height, position.y, position.height, position.height);
        var r3 = new Rect(position.x + 32 + position.height * 2f, position.y, position.height, position.height);
        
        var r4 = new Rect(256 + position.x, position.y, position.height, position.height);
        var r5 = new Rect(256 + position.x + 4 + position.height, position.y, position.height, position.height);
        var r6 = new Rect(256 + position.x + 32 + position.height * 2f, position.y, position.height, position.height);
        
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PropertyField(r1, property.FindPropertyRelative("ingredientA"), GUIContent.none);
        EditorGUI.PropertyField(r2, property.FindPropertyRelative("ingredientB"), GUIContent.none);
        EditorGUI.PropertyField(r3, property.FindPropertyRelative("output"), GUIContent.none);
        EditorGUI.DrawTextureTransparent(r4,
            (property.FindPropertyRelative("ingredientA").objectReferenceValue as Sprite).ToTexture2D()
            );
        EditorGUI.DrawTextureTransparent(r5,
            (property.FindPropertyRelative("ingredientB").objectReferenceValue as Sprite).ToTexture2D()
        );
        EditorGUI.DrawTextureTransparent(r6,
            (property.FindPropertyRelative("output").objectReferenceValue as Sprite).ToTexture2D()
        );
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 40;
    }
}
