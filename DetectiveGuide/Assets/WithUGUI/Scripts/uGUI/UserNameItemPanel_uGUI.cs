using TMPro;
using UnityEngine;

public class UserNameItemPanel_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_InputField = null;

    public void SetItem(string inValue)
    {
        m_InputField.text = inValue;
    }
}
