using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserDetailPopupItemPanel_uGUI : MonoBehaviour
{
    [SerializeField] private Toggle m_Toggle = null;
    [SerializeField] private TMP_Dropdown m_Dropdown = null;

    public void Initialize(string[] inOptions, UnityAction<string> onDropdownChanged, bool isToggle, UnityAction<bool> onToggleChanged)
    {
        m_Toggle.onValueChanged.RemoveAllListeners();
        m_Toggle.onValueChanged.AddListener((value) => { onToggleChanged?.Invoke(value); });
        m_Toggle.SetIsOnWithoutNotify(isToggle);

        var values = inOptions.ToList();
        m_Dropdown.AddOptions(values);
        m_Dropdown.onValueChanged.RemoveAllListeners();
        m_Dropdown.onValueChanged.AddListener((value) => { onDropdownChanged?.Invoke(values[value]); });
        m_Dropdown.value = 0;
    }

    public void Select(string inValue)
    {
        m_Dropdown.value = m_Dropdown.options.FindIndex( (value) => { return (value.text == inValue); });
    }
}
