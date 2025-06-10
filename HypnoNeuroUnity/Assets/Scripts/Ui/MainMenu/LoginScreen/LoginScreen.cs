using System.Text.RegularExpressions;
using DebugClasses;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;
using Object = System.Object;

namespace Ui.MainMenu.LoginScreen
{
    public class LoginScreen : FullScreenView
    {
        [Header("Ui Elements")] 
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button googleLoginButton;
        [SerializeField] private Button appleLoginButton;
        [SerializeField] private Button signUpButton;

        protected override void OnShow(Object obj)
        {
            
            loginButton.interactable = true;
            loginButton.onClick.AddListener(OnClickLogin);
            signUpButton.onClick.AddListener(OnClickSignUp);
        }

        protected override void OnHide()
        {
            emailInputField.text = "";
            passwordInputField.text = "";
            
            loginButton.onClick.RemoveListener(OnClickLogin);
            signUpButton.onClick.RemoveListener(OnClickSignUp);
        }

        private void OnClickLogin()
        {
            if (emailInputField.text.Length <= 0)
            {
                UiManager.Instance.ShowToast("Email Cannot Be Empty");
                return;
            }

            if (!IsValidEmail(emailInputField.text))
            {
                UiManager.Instance.ShowToast("Invalid Email");
                return;
            }

            if (passwordInputField.text.Length <= 0)
            {
                UiManager.Instance.ShowToast("Password Cannot Be Empty");
                return;
            }

            // TODO :: LOGIN USER
            loginButton.interactable = false;
            FirebaseManager.Instance.Login(emailInputField.text, passwordInputField.text, OnSuccessful, OnFailed);
        }

        private void OnSuccessful(string message)
        {
            UiManager.Instance.ShowPanel(UiScreenName.MainScreen, null);
        }

        private void OnFailed(string message)
        {
            loginButton.interactable = true;
            UiManager.Instance.ShowToast(message);
        }

        private void OnClickSignUp()
        {
            UiManager.Instance.ShowPanel(UiScreenName.SignUpScreen, null);
        }

        private bool IsValidEmail(string email)
        {
            const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}