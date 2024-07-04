using UnityEngine;

namespace Player
{
    /// <summary>
    /// This class is for storing values and giving debugging options to view without cluttering the player/other scripts
    /// </summary>
    public class PlayerDebug : MonoBehaviour
    {
        public static PlayerDebug Instance;
        private void Awake() { Instance = this; }
    }
}