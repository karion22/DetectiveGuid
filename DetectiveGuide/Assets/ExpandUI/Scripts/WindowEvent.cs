using UnityEngine;
using UnityEngine.Events;

public class WindowEvent : Singleton<WindowEvent>
{
    private float m_PrevWidth;
    private float m_PrevHeight;

    public delegate void OnScreenSizeChanged();
    public OnScreenSizeChanged onScreenSizeChanged = null;

    public override void Awake()
    {
        base.Awake();

        m_PrevWidth = Screen.width;
        m_PrevHeight = Screen.height;
    }

    private void Update()
    {
        if (onScreenSizeChanged == null)
            return;

        if(m_PrevHeight != Screen.height || m_PrevWidth != Screen.width)
        {
            m_PrevHeight = Screen.height;
            m_PrevWidth = Screen.width;
            onScreenSizeChanged.Invoke();
        }
    }
}
