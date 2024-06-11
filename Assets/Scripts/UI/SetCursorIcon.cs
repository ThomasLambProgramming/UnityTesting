using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursorIcon : MonoBehaviour
{
    [SerializeField] private Texture2D m_mouseIcon;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(m_mouseIcon, Vector2.zero, CursorMode.Auto);
    }
}
