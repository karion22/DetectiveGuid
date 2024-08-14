using TMPro;
using UnityEngine;

public class PlaySubItem_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_TitleLabel = null;
    [SerializeField] private TMP_Text m_ValueLabel = null;

    public void SetTitle(string inTitle) { m_TitleLabel.text = inTitle; }
    public void SetValue(string inValue) { m_ValueLabel.text = inValue; }

    public void Set(string inTitle, string inValue)
    {
        SetTitle(inTitle);
        SetValue(inValue);
    }
}
