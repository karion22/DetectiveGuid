using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserDetailPopup_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_UserNameLabel = null;

    [SerializeField] private Toggle m_TurnToggle = null;
    [SerializeField] private TMP_Dropdown m_NameDropdown = null;
    [SerializeField] private TMP_Dropdown m_WeaponDropdown = null;
    [SerializeField] private TMP_Dropdown m_PlaceDropdown = null;

    [SerializeField] private Button m_ApplyBtn = null;
    [SerializeField] private Button m_CancelBtn = null;

    public UnityAction<string, string, string, bool> onApplyBtnClicked = null;
    public UnityAction onClosed = null;

    private string m_Name = string.Empty;
    private string m_Weapon = string.Empty;
    private string m_Place = string.Empty;
    private bool m_UseTurn = true;

    public void Initialize(PlayItem inItem)
    {
        m_UserNameLabel.text = inItem.m_UserName;

        var names = GameMgr.Instance.DataSet.Names.ToList();
        m_NameDropdown.AddOptions(names);
        m_NameDropdown.onValueChanged.RemoveAllListeners();
        m_NameDropdown.onValueChanged.AddListener((value) => { m_Name = names[value]; });
        m_Name = m_NameDropdown.options[0].text;

        var weapons = GameMgr.Instance.DataSet.Weapons.ToList();
        m_WeaponDropdown.AddOptions(weapons);
        m_WeaponDropdown.onValueChanged.RemoveAllListeners();
        m_WeaponDropdown.onValueChanged.AddListener((value) => { m_Weapon = names[value]; });
        m_Weapon = m_WeaponDropdown.options[0].text;

        var places = GameMgr.Instance.DataSet.Places.ToList();
        m_PlaceDropdown.AddOptions(places);
        m_PlaceDropdown.onValueChanged.RemoveAllListeners();
        m_PlaceDropdown.onValueChanged.AddListener((value) => { m_Place = names[value]; });
        m_Place = m_PlaceDropdown.options[0].text;

        m_UseTurn = inItem.m_Complete;
        m_TurnToggle.SetIsOnWithoutNotify(inItem.m_Complete);
        m_TurnToggle.onValueChanged.RemoveAllListeners();
        m_TurnToggle.onValueChanged.AddListener((value) => { m_UseTurn = value; });

        // 리스트화 되어 있는 값에서 각 타입에 맞는 값들을 찾아서 초기화 시킨다.
        foreach(var item in inItem.m_Items)
        {
            if (item == null) continue;

            switch(item.m_Type)
            {
                case eDataType.Name:
                    {
                        m_Name = item.m_Value;
                        if(string.IsNullOrEmpty(m_Name) == false)
                        {
                            m_NameDropdown.value = m_NameDropdown.options.FindIndex((value) => { return (value.text == item.m_Value); });
                        }
                    }
                    break;

                case eDataType.Weapon:
                    {
                        m_Weapon = item.m_Value;
                        if (string.IsNullOrEmpty(m_Weapon) == false)
                        {
                            m_WeaponDropdown.value = m_WeaponDropdown.options.FindIndex((value) => { return value.text == item.m_Value; });
                        }
                    }
                    break;

                case eDataType.Place:
                    {
                        m_Place = item.m_Value;
                        if(string.IsNullOrEmpty(m_Place) == false)
                        {
                            m_PlaceDropdown.value = m_PlaceDropdown.options.FindIndex((value) => { return value.text == item.m_Value; });
                        }
                    }
                    break;
            }
        }
        //

        m_ApplyBtn.onClick.RemoveAllListeners();
        m_ApplyBtn.onClick.AddListener(() => {
            onApplyBtnClicked?.Invoke(m_Name, m_Weapon, m_Place, m_UseTurn);
            onClosed?.Invoke();
        });

        m_CancelBtn.onClick.RemoveAllListeners();
        m_CancelBtn.onClick.AddListener(() => {
            onClosed?.Invoke();
        });
    }
}
