using UnityEngine;

public class SetCursorIcon : MonoBehaviour
{
    [SerializeField] private Texture2D m_mouseIcon;
    // Start is called before the first frame update
    void Start()
    {
        //For now the icon just isnt in a good position that is clear where the clicks are meant to be.
        //Cursor.SetCursor(m_mouseIcon, Vector2.zero, CursorMode.Auto);
    }
}
