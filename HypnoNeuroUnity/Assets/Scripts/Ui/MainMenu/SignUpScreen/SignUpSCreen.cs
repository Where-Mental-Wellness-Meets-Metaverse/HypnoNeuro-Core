using System.Text.RegularExpressions;
using DebugClasses;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

namespace Ui.MainMenu.SignUpScreen
{
    public class SignUpSCreen : FullScreenView
    {
        [Header("Ui Elements")] 
        [SerializeField] private TMP_InputField emailInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private TMP_InputField confirmPasswordInputField;
        [SerializeField] private Button signUpButton;
        [SerializeField] private Button googleLoginButton;
        [SerializeField] private Button appleLoginButton;
        [SerializeField] private Button backButton;
        
        
        protected override void OnShow(Object obj)
        {
            signUpButton.onClick.AddListener(OnClickSignUp);
            backButton.onClick.AddListener(OnClickBack);

            signUpButton.interactable = true;
        }


        protected override void OnHide()
        {
            emailInputField.text = "";
            passwordInputField.text = "";
            confirmPasswordInputField.text = "";
            
            signUpButton.onClick.RemoveListener(OnClickSignUp);
            backButton.onClick.RemoveListener(OnClickBack);
        }

        private void OnClickSignUp()
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
            
            if (passwordInputField.text != confirmPasswordInputField.text)
            {
                UiManager.Instance.ShowToast("Password Does Not Match");
                return;
            }

            if (passwordInputField.text.Length <= 0)
            {
                UiManager.Instance.ShowToast("Password Cannot Be Empty");
                return;
            }
            
            // TODO :: SIGN UP USER
            FirebaseManager.Instance.SignUp(emailInputField.text, passwordInputField.text, OnSuccessful, OnFailed);
            signUpButton.interactable = true;
            
        }
        private void OnSuccessful(string message)
        {
            UiManager.Instance.ShowPanel(UiScreenName.MainScreen, null);
        }

        private void OnFailed(string message)
        {
            signUpButton.interactable = true;
            UiManager.Instance.ShowToast(message);
        }

        private void OnClickBack()
        {
            UiManager.Instance.HidePanel();
        }

        private bool IsValidEmail(string email)
        {
            const string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
        
    }
}
