using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimationSpriteSheetChanger : MonoBehaviour
{
    public string AsPrefix;

    public Sprite[] NewSprites;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();          
    }

    private void LateUpdate()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (AsPrefix.Length <= 0)
        {
            return;
        }

        if (NewSprites == null || NewSprites.Length <= 0)
        {
            return;
        }

        string currentSpriteName = spriteRenderer.sprite.name;
        if (currentSpriteName.Length > AsPrefix.Length && currentSpriteName.StartsWith(AsPrefix))
        {
            string indexString = currentSpriteName.Substring(AsPrefix.Length);
            bool indexParseResult = int.TryParse(indexString, out int spriteIndex);
            if (indexParseResult && spriteIndex < NewSprites.Length)
            {
                spriteRenderer.sprite = NewSprites[spriteIndex];
            }
        }
    }
}
