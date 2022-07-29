using UnityEngine;
using WeddingIntro.Data;
using WeddingIntro.Utility;

namespace WeddingIntro.Avatar
{
    public class AvatarAnimationSpriteSheetChanger : MonoBehaviour
    {
        private SpriteSheetTableUnit[] tableEntries;

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
            if (spriteRenderer == null || tableEntries == null)
            {
                return;
            }

            foreach (SpriteSheetTableUnit unit in tableEntries)
            {
                if (unit.Prefix == null || unit.Prefix.Length <= 0)
                {
                    continue;
                }

                if (unit.SpriteSequence == null || unit.SpriteSequence.Length <= 0)
                {
                    continue;
                }

                string currentSpriteName = spriteRenderer.sprite.name;
                if (currentSpriteName.Length > unit.Prefix.Length && currentSpriteName.StartsWith(unit.Prefix))
                {
                    string indexString = currentSpriteName.Substring(unit.Prefix.Length);
                    bool indexParseResult = int.TryParse(indexString, out int spriteIndex);
                    if (indexParseResult && spriteIndex < unit.SpriteSequence.Length)
                    {
                        spriteRenderer.sprite = unit.SpriteSequence[spriteIndex];
                    }
                }
            }
        }

        private void OnPlayerInfoSetResponse(PlayerInfo info)
        {
            foreach (SpriteSheetTablePair pair in spriteSheetsTable.Table)
            {
                if (pair.Key == info.Gender)
                {
                    SetSpriteSheetChangerData(pair.TableUnits);
                    break;
                }
            }
        }

        private void SetSpriteSheetChangerData(SpriteSheetTableUnit[] tableUnits)
        {
            Debug.Assert(tableUnits != null);
            tableEntries = tableUnits;
        }
    }
}
