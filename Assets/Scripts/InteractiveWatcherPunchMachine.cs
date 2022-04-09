using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWatcherPunchMachine : InteractiveWatcherBase
{
    private float startTime = -1.0f;

    private int levelNumber = 1;

    private int LevelNumber
    {
        get => levelNumber;
        set
        {
            levelNumber = value;
            if (numberIndicator != null && value >= 0 && value < numberSprites.Length)
            {
                numberIndicator.sprite = numberSprites[value];
            }
        }
    }

    [SerializeField]
    private float defaultPeriod = 1.0f;

    private float currentLuckyValue = 0.0f;

    private Vector3 indicatorOriginalScale;

    [SerializeField]
    private SpriteRenderer indicator;

    [SerializeField]
    private SpriteRenderer numberIndicator;

    [SerializeField]
    private Sprite[] numberSprites;

    [SerializeField]
    private float winLuckyValue = 0.8f;

    [SerializeField]
    private float difficultyFactor = 0.9f;

    [SerializeField]
    private string winAllDialog;

    private float randomTimeShift;

    public override void InvokeInteract()
    {
        AvatarInput avatarInput = GameStatics.Instance.PlayerAvatarInput;
        avatarInput.AddKeyResponse(InteractionKeyPriority.PunchMachine, new AvatarInput.KeyResponse(HitMachine), KeyPressType.KeyDown);
        avatarInput.AddKeyResponse(DirectionKeyResponsePriority.PunchMachine, new AvatarInput.KeyResponse((GameKeyCode _) => { }), KeyPressType.KeyDown);

        AudioManager.Instance.PlayBackgroundMusic("HavingFun");
        StartGame(1);
    }

    private void StartGame(int levelNum)
    {
        LevelNumber = levelNum;
        startTime = Time.time;
        randomTimeShift = Random.value;
    }

    private void EndGame(bool success)
    {
        startTime = -1.0f;

        if (!success)
        {
            LevelNumber = 0;
        }

        AvatarInput avatarInput = GameStatics.Instance.PlayerAvatarInput;
        avatarInput.RemoveKeyResponse(InteractionKeyPriority.PunchMachine);
        avatarInput.RemoveKeyResponse(DirectionKeyResponsePriority.PunchMachine);

        AudioManager.Instance.PlayBackgroundMusic("Background");

    }

    private void ResetLuckyIndicator()
    {
        indicator.gameObject.transform.localScale = new Vector3(indicatorOriginalScale.x, 0.0f, indicatorOriginalScale.z);
    }

    private void HitMachine(GameKeyCode gameKeyCode)
    {
        if(startTime < 0.0f)
        {
            StartGame(1);
        }
        else
        {
            if (currentLuckyValue > winLuckyValue)
            {
                if (LevelNumber + 1 < numberSprites.Length)
                {
                    StartGame(LevelNumber + 1);
                }
                else
                {
                    AudioManager.Instance.PlayerSoundEffect("Winner");
                    UIDialogBoxController.Instance.StartDialog(winAllDialog, null);
                    EndGame(true);
                }    
            }
            else
            {
                AudioManager.Instance.PlayerSoundEffect("RejectJoin");
                EndGame(false);
            }
        }
    }

    private new void Start()
    {
        base.Start();

        indicatorOriginalScale = indicator.gameObject.transform.localScale;
        ResetLuckyIndicator();
        LevelNumber = 0;
    }

    private void Update()
    {
        if (startTime < 0.0f)
        {
            return;
        }

        float period = defaultPeriod * Mathf.Pow(difficultyFactor, LevelNumber);
        float timeResidue = Mathf.Repeat(Time.time + randomTimeShift * period - startTime, period);
        if (timeResidue > 0.5f * period)
        {
            timeResidue = period - timeResidue;
        }
        timeResidue = timeResidue * 2 * (1 / period);

        currentLuckyValue = 1 - Mathf.Sin(timeResidue * Mathf.PI * 0.5f);
        indicator.transform.localScale = new Vector3(indicatorOriginalScale.x, indicatorOriginalScale.y * currentLuckyValue, indicatorOriginalScale.z);
    }
}
