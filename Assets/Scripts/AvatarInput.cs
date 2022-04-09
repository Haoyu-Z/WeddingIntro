using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DirectionKeyResponsePriority
{
    Movement = 0,
    ChangeDialogSelection = 1,
    MessageBoardFocus = 2,
    PunchMachine = 3,

    // sentinel, always highest
    UIFocus,
}

public enum InteractionKeyPriority
{
    InteractiveWatcher = 0,
    DialogConfirm = 1,
    MessageBoardFocus = 2,
    PunchMachine = 3,

    // sentinel, always highest
    UIFocus,
}

public enum GameKeyType
{
    Direction,
    Interaction,
}

public enum GameKeyCode
{
    DirectionUp,
    DirectionDown,
    DirectionLeft,
    DirectionRight,
    KeyA,
    KeyB,
}

public enum KeyPressType
{
    DefaultPressing,
    KeyDown,
}

[System.Serializable]
public struct GameKeyDefinition
{
    public KeyCode key;
    public UIKeyCode uiKey;
    public GameKeyCode keyDefinition;
    public GameKeyType keyType;
}

public class AvatarInput : MonoBehaviour
{
    public delegate void KeyResponse(GameKeyCode key);

    private struct KeyResponseEntry
    {
        public readonly KeyResponse Response;
        public readonly KeyPressType PressType;

        public KeyResponseEntry(KeyResponse response, KeyPressType pressType)
        {
            Response = response;
            PressType = pressType;
        }
    }

    [SerializeField]
    private PieceOfInputSetting inputSetting;

    private readonly Dictionary<System.Type, SortedList> responseListMap = new Dictionary<System.Type, SortedList>();

    private readonly Dictionary<GameKeyType, System.Type> gameKeyTypeToPriorityType = new Dictionary<GameKeyType, System.Type>();

    public void AddKeyResponse<T>(T priority, KeyResponse response, KeyPressType pressType)
    {
        System.Type priorityType = priority.GetType();
        if (!responseListMap.ContainsKey(priorityType))
        {
            responseListMap[priorityType] = new SortedList();
        }

        Debug.Assert(!responseListMap[priorityType].ContainsKey(priority));
        responseListMap[priorityType].Add(priority, new KeyResponseEntry(response, pressType));
    }

    public void RemoveKeyResponse<T>(T priority)
    {
        System.Type priorityType = priority.GetType();
        if (responseListMap.ContainsKey(priorityType) && responseListMap[priorityType].ContainsKey(priority))
        {
            responseListMap[priorityType].Remove(priority);
        }
    }

    private void RegisterGameKeyTypePriorityType()
    {
        gameKeyTypeToPriorityType.Add(GameKeyType.Direction, typeof(DirectionKeyResponsePriority));
        gameKeyTypeToPriorityType.Add(GameKeyType.Interaction, typeof(InteractionKeyPriority));
    }

    private void Start()
    {
        RegisterGameKeyTypePriorityType();
    }

    private void Update()
    {
        Debug.Assert(GameStatics.Instance != null || UIKeyManager.Instance != null);

        foreach (GameKeyDefinition definition in inputSetting.InputSettings)
        {
            bool keyDown = Input.GetKeyDown(definition.key) || UIKeyManager.Instance.GetUIKeyDown(definition.uiKey);
            bool keyPressing = Input.GetKey(definition.key) || UIKeyManager.Instance.GetUIKey(definition.uiKey);
            if (keyDown || keyPressing)
            {
                Debug.Assert(gameKeyTypeToPriorityType.ContainsKey(definition.keyType));
                Debug.Assert(responseListMap.ContainsKey(gameKeyTypeToPriorityType[definition.keyType]));

                SortedList responseList = responseListMap[gameKeyTypeToPriorityType[definition.keyType]];
                if (responseList.Count > 0)
                {
                    KeyResponseEntry responseEntry = (KeyResponseEntry)responseList.GetByIndex(responseList.Count - 1);
                    if (
                        (responseEntry.PressType == KeyPressType.DefaultPressing && keyPressing)
                        ||
                        (responseEntry.PressType == KeyPressType.KeyDown && keyDown)
                        )
                    {
                        responseEntry.Response.Invoke(definition.keyDefinition);
                    }
                }
            }
        }
    }
}
