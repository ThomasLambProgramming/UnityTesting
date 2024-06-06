using UnityEngine;

public class MainMenuCameraSpin : MonoBehaviour
{
    [SerializeField] private float m_SpinSpeed = 72f;
    void Update()
    {
        transform.Rotate(0, m_SpinSpeed * Time.deltaTime,0);   
    }
}
