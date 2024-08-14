using TMPro;
using UnityEngine;

public class CardSubItem_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Title = null;
    [SerializeField] private TMP_Text m_Description = null;

    public void SetTitle(string inText) { m_Title.text = inText; }
    public void SetDescription(string inText) {m_Description.text = inText; }
}
