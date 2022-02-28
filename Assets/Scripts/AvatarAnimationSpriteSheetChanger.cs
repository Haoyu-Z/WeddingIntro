using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarAnimationSpriteSheetChanger : MonoBehaviour
{
    public string asPrefix;

    public Sprite[] newSprites;

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

        if (asPrefix.Length <= 0)
        {
            return;
        }

        if (newSprites == null || newSprites.Length <= 0)
        {
            return;
        }

        string currentSpriteName = spriteRenderer.sprite.name;
        if (currentSpriteName.Length > asPrefix.Length && currentSpriteName.StartsWith(asPrefix))
        {
            string indexString = currentSpriteName.Substring(asPrefix.Length);
            bool indexParseResult = int.TryParse(indexString, out int spriteIndex);
            if (indexParseResult && spriteIndex < newSprites.Length)
            {
                spriteRenderer.sprite = newSprites[spriteIndex];
            }
        }
    }
}
