using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerInfo : MonoBehaviour
{
    [SerializeField]
    private Button buttonConfirm;

    [SerializeField]
    private Button buttonSelectGenderMale;

    [SerializeField]
    private Button buttonSelectGenderFemale;

    [SerializeField]
    private InputField inputFieldName;

    [SerializeField]
    private Image blackScreen;

    [SerializeField]
    private Transform inputPanel;

    [SerializeField]
    private float panelScaleOutTime = 0.3f;

    [SerializeField]
    private float blackScreenFadeOutTime = 0.3f;

    private PlayerInfo playerInfo = PlayerInfo.Tarnished;

    private float confirmTime = -1.0f;

    private void Start()
    {
        buttonConfirm.onClick.AddListener(OnConfirmButtonClicked);
        buttonSelectGenderMale.onClick.AddListener(OnSelectGenderMaleButtonClicked);
        buttonSelectGenderFemale.onClick.AddListener(OnSelectGenderFemaleButtonClicked);
        inputFieldName.onValueChanged.AddListener(OnNameTextChanged);

        // initialization
        OnNameTextChanged(inputFieldName.text);
        OnSelectGenderMaleButtonClicked();

        Debug.Assert(GameStatics.Instance != null);
        AvatarInput avatarInput = GameStatics.Instance.PlayerAvatarInput;
        Debug.Assert(avatarInput != null);
        avatarInput.AddKeyResponse(DirectionKeyResponsePriority.UIFocus, (_) => { }, KeyPressType.DefaultPressing);
        avatarInput.AddKeyResponse(InteractionKeyPriority.UIFocus, (_) => { }, KeyPressType.DefaultPressing);
    }

    private void Update()
    {
        if (confirmTime > 0.0f)
        {
            float timeElapsed = Time.time - confirmTime;
            if (timeElapsed < panelScaleOutTime + blackScreenFadeOutTime)
            {
                inputPanel.localScale = Vector3.one * Mathf.Clamp01(Mathf.Lerp(1.0f, 0.0f, timeElapsed / panelScaleOutTime));
                blackScreen.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Clamp01(Mathf.Lerp(1.0f, 0.0f, (timeElapsed - panelScaleOutTime) / blackScreenFadeOutTime)));
            }
            else
            {
                Debug.Assert(GameStatics.Instance != null);
                AvatarInput avatarInput = GameStatics.Instance.PlayerAvatarInput;
                Debug.Assert(avatarInput != null);
                avatarInput.RemoveKeyResponse(DirectionKeyResponsePriority.UIFocus);
                avatarInput.RemoveKeyResponse(InteractionKeyPriority.UIFocus);
                Destroy(gameObject);
            }
        }
    }

    private void OnConfirmButtonClicked()
    {
        if (playerInfo.Name.Trim() == "")
        {
            inputFieldName.text = GameStatics.Instance.PlayerNameAutoFill;
            return;
        }

        GameStatics.Instance.UIDebugText.AddDebugText($"PlayerName={playerInfo.Name}, PlayerGender={playerInfo.Gender}");

        Debug.Assert(GameStatics.Instance != null);
        GameStatics.Instance.PlayerInformation = playerInfo;
        HideUIPanel();

        if(GameStatics.Instance.SendMailOnLogin)
        {
            Mailer.SendMail(new Mailer.MailInfo(Mailer.MailType.Login, playerInfo));
        }
    }

    private void OnSelectGenderMaleButtonClicked()
    {
        playerInfo.Gender = "Male";
        buttonSelectGenderMale.GetComponent<UIButtonWithEnabledState>()?.ChangeState(true);
        buttonSelectGenderFemale.GetComponent<UIButtonWithEnabledState>()?.ChangeState(false);
    }

    private void OnSelectGenderFemaleButtonClicked()
    {
        playerInfo.Gender = "Female";
        buttonSelectGenderMale.GetComponent<UIButtonWithEnabledState>()?.ChangeState(false);
        buttonSelectGenderFemale.GetComponent<UIButtonWithEnabledState>()?.ChangeState(true);
    }

    private void OnNameTextChanged(string nameValue)
    {
        Debug.Assert(nameValue != null);
        GameStatics.Instance.UIDebugText.AddDebugText($"Name text field is changed to {nameValue}");
        playerInfo.Name = nameValue;
    }

    private void HideUIPanel()
    {
        confirmTime = Time.time;
    }
}
