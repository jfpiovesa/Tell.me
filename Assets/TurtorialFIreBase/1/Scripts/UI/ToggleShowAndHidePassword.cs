using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ToggleShowAndHidePassword : MonoBehaviour
{
    bool hideLoginPass = true;
    bool hideRegistrationPass = true;
    public TMP_InputField loginpasswordInputField;
    public TMP_InputField registerPasswordField, registerVerifyPasswordField;
    public Sprite[] hideShow;
    public void ShowOrHideLoginPassword(Image image)
    {
        hideLoginPass = !hideLoginPass;
        if(hideLoginPass)
        {
            loginpasswordInputField.contentType = TMP_InputField.ContentType.Password;
            image.sprite = hideShow[1];
        }
        else
        {
            loginpasswordInputField.contentType = TMP_InputField.ContentType.Standard;
            image.sprite = hideShow[0];
        }
        EventSystem.current.SetSelectedGameObject(loginpasswordInputField.gameObject, null);
    }

    public void ShowOrHideRegistrationPassword(Image image)
    {
        hideRegistrationPass = !hideRegistrationPass;
        if(hideRegistrationPass)
        {
            registerPasswordField.contentType = TMP_InputField.ContentType.Password;
            registerVerifyPasswordField.contentType = TMP_InputField.ContentType.Password;
            image.sprite = hideShow[1];
        }
        else
        {
            registerPasswordField.contentType = TMP_InputField.ContentType.Standard;
            registerVerifyPasswordField.contentType = TMP_InputField.ContentType.Standard;
            image.sprite = hideShow[0];
        }
        EventSystem.current.SetSelectedGameObject(registerPasswordField.gameObject, null);
        EventSystem.current.SetSelectedGameObject(registerVerifyPasswordField.gameObject, null);
    }
}
