using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UserDetailPopup_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_UserNameLabel = null;

    [SerializeField] private Toggle m_TurnToggle = null;
    [SerializeField] private UserDetailPopupItemPanel_uGUI m_NameItemPanel = null;
    [SerializeField] private UserDetailPopupItemPanel_uGUI m_WeaponItemPanel = null;
    [SerializeField] private UserDetailPopupItemPanel_uGUI m_PlaceItemPanel = null;

    [SerializeField] private Button m_ApplyBtn = null;
    [SerializeField] private Button m_CancelBtn = null;

    public UnityAction<string, string, string, bool> onApplyBtnClicked = null;
    public UnityAction onClosed = null;

    private string m_Name = "";
    private bool m_NameToggle = true;

    private string m_Weapon = "";
    private bool m_WeaponToggle = true;

    private string m_Place = "";
    private bool m_PlaceToggle = true;

    private bool m_UseTurn = true;

    public void Initialize(PlayItem inItem)
    {
        m_UserNameLabel.text = inItem.m_UserName;

        m_NameItemPanel.Initialize(GameMgr.Instance.DataSet.Names, (value) => { m_Name = value; }, m_NameToggle, (value) => { m_NameToggle = value; });
        m_Name = GameMgr.Instance.DataSet.Names[0];

        m_WeaponItemPanel.Initialize(GameMgr.Instance.DataSet.Weapons, (value) => { m_Weapon = value; }, m_WeaponToggle, (value) => { m_WeaponToggle = value; });
        m_Weapon = GameMgr.Instance.DataSet.Weapons[0];

        m_PlaceItemPanel.Initialize(GameMgr.Instance.DataSet.Places, (value) => { m_Place = value; }, m_PlaceToggle, (value) => { m_PlaceToggle = value; });
        m_Place = GameMgr.Instance.DataSet.Places[0];

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
                            m_NameItemPanel.Select(item.m_Value);
                    }
                    break;

                case eDataType.Weapon:
                    {
                        m_Weapon = item.m_Value;
                        if (string.IsNullOrEmpty(m_Weapon) == false)
                            m_WeaponItemPanel.Select(item.m_Value);
                    }
                    break;

                case eDataType.Place:
                    {
                        m_Place = item.m_Value;
                        if(string.IsNullOrEmpty(m_Place) == false)
                            m_PlaceItemPanel.Select(item.m_Value);
                    }
                    break;
            }
        }
        //

        m_ApplyBtn.onClick.RemoveAllListeners();
        m_ApplyBtn.onClick.AddListener(() => {
            string newName = m_NameToggle ? m_Name : string.Empty;
            string newWeapon = m_WeaponToggle ? m_Weapon : string.Empty;
            string newPlace = m_PlaceToggle ? m_Place : string.Empty;

            onApplyBtnClicked?.Invoke(newName, newWeapon, newPlace, m_UseTurn);
            onClosed?.Invoke();
        });

        m_CancelBtn.onClick.RemoveAllListeners();
        m_CancelBtn.onClick.AddListener(() => {
            onClosed?.Invoke();
        });
    }
}
