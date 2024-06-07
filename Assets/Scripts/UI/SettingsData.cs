using System;
using System.IO;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// This class is just for storing and loading settings data from disk, and staying in do not destroy
    /// </summary>
    public class SettingsData : MonoBehaviour
    {
        [Serializable]
        public struct SettingsStruct
        {
            //Gameplay
            public bool InvertXLook;
            public bool InvertYLook;
            public float MouseXSens;
            public float MouseYSens;
            
            //Graphics
            public float Brightness;
            public int Antialiasing;
            
            //Audio
            public bool MuteAllAudio;
            public float MasterVolume;
            public float MusicVolume;
            public float SoundEffectVolume;
            public float DialogueVolume;
        }

        public static SettingsData Instance;
        public SettingsStruct Data;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else if (Instance != null)
            {
                Debug.LogError("How did the settings get a second awake call FIX IT!");
            }
            
            LoadData();
            DontDestroyOnLoad(this.gameObject);
        }

        private void LoadData()
        {
            if (File.Exists(Application.dataPath + "DwarfSettings.txt"))
            {
                StringReader reader = new StringReader(Application.dataPath + "DwarfSettings.txt");
                string jsonData = reader.ReadToEnd();
                reader.Close();
                Data = JsonUtility.FromJson<SettingsStruct>(jsonData);
            }
            else
            {
                LoadDefaults();
            }
        }

        private void LoadDefaults()
        {
            Data = new SettingsStruct
            {
                InvertXLook = false,
                InvertYLook = false,
                MouseXSens = 10,
                MouseYSens = 10,
                Brightness = 100,
                Antialiasing = 2,
                MuteAllAudio = false,
                MasterVolume = 100,
                MusicVolume = 100,
                SoundEffectVolume = 100,
                DialogueVolume = 100
            };
        }

        public void SaveData(SettingsStruct data)
        {
            StringWriter writer = new StringWriter();
            writer.Write(JsonUtility.ToJson(Data, true));   
            writer.Close();
        }
        
    }
}