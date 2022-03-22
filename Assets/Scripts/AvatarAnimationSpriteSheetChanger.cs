using UnityEngine;

public class AvatarAnimationSpriteSheetChanger : MonoBehaviour
{
    [SerializeField]
    private string asPrefix;

    [SerializeField]
    private PieceOfAvatarSprite newSprites;

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

        if (newSprites == null || newSprites.SpriteSequence.Length <= 0)
        {
            return;
        }

        string currentSpriteName = spriteRenderer.sprite.name;
        if (currentSpriteName.Length > asPrefix.Length && currentSpriteName.StartsWith(asPrefix))
        {
            string indexString = currentSpriteName.Substring(asPrefix.Length);
            bool indexParseResult = int.TryParse(indexString, out int spriteIndex);
            if (indexParseResult && spriteIndex < newSprites.SpriteSequence.Length)
            {
                spriteRenderer.sprite = newSprites.SpriteSequence[spriteIndex];
            }
        }
    }
}
