using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DebugClasses;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using Newtonsoft.Json;
using UnityEngine;

namespace DataModel
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance { get; private set; }
        public UserData UserData { get; private set; }
        private const string UserDataKey = "UserData";

        private FirebaseFirestore db;
        private bool isFirebaseInitialized = false;

        #region Constant Data

        [Header("Constant Data")] public List<Game> games;

        #endregion

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

        public async Task InitDatabase()
        {
            
        }

        public async Task LoadUserDataFromFirestore()
        {
            
        }

        public async Task SaveUserDataToFirestore()
        {
            
        }
    
    }

}
