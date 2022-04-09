using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveWatcherPunchMachine : InteractiveWatcherBase
{
    private float startTime = -1.0f;

    private int levelNumber = 1;

    [SerializeField]
    private float defaultPeriod = 1.0f;

    private float currentLuckyValue = 0.0f;

    private Vector3 indicatorOriginalScale;

    [SerializeField]
    private SpriteRenderer indicator;

    public override void InvokeInteract()
    {
        AvatarInput avatarInput = GameStatics.Instance.PlayerAvatarInput;
        avatarInput.AddKeyResponse(InteractionKeyPriority.PunchMachine, new AvatarInput.KeyResponse(HitMachine), KeyPressType.KeyDown);
        avatarInput.AddKeyResponse(DirectionKeyResponsePriority.PunchMachine, new AvatarInput.KeyResponse((GameKeyCode _) => { }), KeyPressType.KeyDown);

        levelNumber = 1;

    }

    private void StartLevel()
    {
        startTime = Time.time;
    }

    private void HitMachine(GameKeyCode gameKeyCode)
    {

    }

    private void Start()
    {
        indicatorOriginalScale = indicator.gameObject.transform.localScale;
        indicator.gameObject.transform.localScale = new Vector3(indicatorOriginalScale.x, 0.0f, indicatorOriginalScale.z);
    }

    private void Update()
    {
        //if (startTime < 0.0f)
        //{
        //    return;
        //}

        float period = defaultPeriod / (float)levelNumber;
        float timeResidue = Mathf.Repeat(Time.time - startTime, period);
        if (timeResidue > 0.5f * period)
        {
            timeResidue = period - timeResidue;
        }
        timeResidue = timeResidue * 2 * (1 / period);

        currentLuckyValue = 1 - Mathf.Sin(timeResidue * Mathf.PI * 0.5f);
        indicator.transform.localScale = new Vector3(indicatorOriginalScale.x, indicatorOriginalScale.y * currentLuckyValue, indicatorOriginalScale.z);
    }
}
