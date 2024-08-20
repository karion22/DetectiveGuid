using TMPro;
using UnityEngine;

public class UserNameItemPanel_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField m_InputField = null;
    private int m_Index = -1;

    public void Initialize(int inIndex, UserData inUserData)
    {
        m_Index = inIndex;

        if(m_InputField != null)
        {
            m_InputField.onValueChanged.RemoveAllListeners();
            m_InputField.onValueChanged.AddListener((value) => {
                inUserData.UserName = value;
            });
        }
    }

    public void SetItem(string inValue)
    {
        m_InputField.text = inValue;
    }
}
