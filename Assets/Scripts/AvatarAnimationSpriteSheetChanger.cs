using UnityEngine;

public class AvatarAnimationSpriteSheetChanger : MonoBehaviour
{
    private string asPrefix;

    private Sprite[] newSprites;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private AvatarSpriteSheetsTable spriteSheetsTable;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Debug.Assert(GameStatics.Instance != null);
        GameStatics.Instance.PlayerInfoSetEvent += OnPlayerInfoSetResponse;
    }

    private void LateUpdate()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        if (asPrefix == null || asPrefix.Length <= 0)
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

    private void OnPlayerInfoSetResponse(PlayerInfo info)
    {
        foreach(SpriteSheetTablePair pair in spriteSheetsTable.Table)
        {
            if(pair.Key == info.Gender)
            {
                SetSpriteSheetChangerData(pair.Prefix, pair.SpriteSequence);
                break;
            }
        }
    }

    private void SetSpriteSheetChangerData(string prefix, Sprite[] sprites)
    {
        Debug.Assert(prefix != null && sprites != null);
        asPrefix = prefix;
        newSprites = sprites;
    }
}
