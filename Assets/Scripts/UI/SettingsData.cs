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

        private readonly string m_filePath = Application.dataPath + "/DwarfSettings.txt";

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
            if (File.Exists(m_filePath))
            {
                Debug.LogError("DwarfSettings found");
                StreamReader reader = new StreamReader(m_filePath);
                string jsonData = reader.ReadToEnd();
                Data = JsonUtility.FromJson<SettingsStruct>(jsonData);
                reader.Close();
            }
            else
            {
                Debug.LogError("DwarfSettings not found");
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
            Data = data;
            StreamWriter writer = new StreamWriter (m_filePath);
            writer.Write(JsonUtility.ToJson(data, true));   
            writer.Close();
        }
        
    }
}