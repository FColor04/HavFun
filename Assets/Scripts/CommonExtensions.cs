using UnityEngine;

public static class CommonExtensions
{
    public static Texture2D ToTexture2D(this Sprite sprite)
    {
        var croppedTexture = new Texture2D( (int)sprite.rect.width, (int)sprite.rect.height );
        croppedTexture.filterMode = FilterMode.Point;
        var pixels = sprite.texture.GetPixels(  (int)sprite.textureRect.x, 
            (int)sprite.textureRect.y, 
            (int)sprite.textureRect.width, 
            (int)sprite.textureRect.height );
        croppedTexture.SetPixels( pixels );
        croppedTexture.Apply();
        return croppedTexture;
    }        
}