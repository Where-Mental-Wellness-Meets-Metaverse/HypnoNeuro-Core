using System;
using System.Threading.Tasks;
using DataModel;
using DebugClasses;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

namespace Managers
{
    public class FirebaseManager : MonoBehaviour
    {
        private FirebaseAuth auth;

        public FirebaseUser CurrentUser { get; private set; }

        public static FirebaseManager Instance { get; private set; }
        
        private bool isFirebaseInitialized = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (auth is null) return;
            
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }


        public async Task InitializeFirebase()
        {
            if (isFirebaseInitialized)
            {
                Debugger.ShowLogMessage("Firebase is already initialized.");
                return;
            }

            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                var app = FirebaseApp.DefaultInstance;
                Debugger.ShowLogMessage("Firebase App Name: " + app.Name);
                auth = FirebaseAuth.DefaultInstance;
                auth.StateChanged += AuthStateChanged;
                AuthStateChanged(this, null);
                Debugger.ShowLogMessage("Firebase initialized successfully.");
        
                isFirebaseInitialized = true; // Mark Firebase as initialized
            }
            else
            {
                Debugger.ShowErrorMessage($"Could not resolve Firebase dependencies: {dependencyStatus}");
            }
        }


        private void AuthStateChanged(object sender, EventArgs eventArgs)
        {
            if (auth.CurrentUser == CurrentUser) return;

            var signedIn = CurrentUser != auth.CurrentUser && auth.CurrentUser != null && auth.CurrentUser.IsValid();

            if (!signedIn && CurrentUser != null)
            {
                Debugger.ShowLogMessage("Signed out " + CurrentUser.UserId);
            }

            CurrentUser = auth.CurrentUser;

            if (signedIn)
            {
                Debugger.ShowLogMessage("Signed in " + CurrentUser.UserId);
                // DataManager.Instance.ListenForUserData();
            }
        }

        public void SignUp(string email, string password, Action<string> onSuccess, Action<string> onFailure)
        {
            auth?.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debugger.ShowErrorMessage("CreateUserWithEmailAndPasswordAsync was canceled.");
                    onFailure?.Invoke("Sign up was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debugger.ShowErrorMessage("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                    if (task.Exception != null) onFailure?.Invoke(task.Exception.Message);
                    return;
                }

                CurrentUser = task.Result.User;
                Debugger.ShowLogMessage($"Firebase user created successfully: {CurrentUser.DisplayName} ({CurrentUser.UserId})");

                onSuccess?.Invoke("Sign up successful.");
            });
        }

        public void Login(string email, string password, Action<string> onSuccess, Action<string> onFailure)
        {
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debugger.ShowErrorMessage("SignInWithEmailAndPasswordAsync was canceled.");
                    onFailure?.Invoke("Login was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debugger.ShowErrorMessage($"SignInWithEmailAndPasswordAsync encountered an error: {task.Exception}");

                    if (task.Exception?.GetBaseException() is not FirebaseException firebaseEx) return;

                    var errorCode = (AuthError)firebaseEx.ErrorCode;

                    var errorMessage = errorCode switch
                    {
                        AuthError.MissingEmail => "Email field is required.",
                        AuthError.MissingPassword => "Password field is required.",
                        AuthError.WrongPassword => "Incorrect password. Please try again.",
                        AuthError.InvalidEmail => "Invalid email format.",
                        AuthError.UserNotFound => "No account found with this email.",
                        _ => "Login failed. Please check your credentials."
                    };

                    onFailure?.Invoke(errorMessage);
                    return;
                }

                // Successfully signed in
                CurrentUser = task.Result.User;
                Debugger.ShowLogMessage($"User signed in successfully: {CurrentUser.DisplayName} ({CurrentUser.UserId})");

                // Start listening to real-time updates
                // DataManager.Instance.ListenForUserData();

                onSuccess?.Invoke("Login successful.");
            });
        }

        public void SignOut()
        {
            if (auth == null) return;

            auth.SignOut();
            CurrentUser = null;
            Debugger.ShowLogMessage("User signed out successfully.");
        }

    }

}
