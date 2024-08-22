using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserCardPanel_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_UserNameLabel = null;
    [SerializeField] private TMP_Text[] m_Items = null;
    [SerializeField] private Image m_EnableImg = null;

    [SerializeField] private Sprite m_EnableTex = null;
    [SerializeField] private Sprite m_DisableTex = null;

    [SerializeField] private GameObject m_DetailPrefab = null;

    private PlayItem m_PlayItem = new PlayItem();
    private GameObject m_DetailPopup = null;

    public void Initialize()
    {
        var button = GetComponent<Button>();

        if(button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => {
                OnClicked();
            });
        }
    }

    // 명시적으로 한 번 더 해제한다.
    private void OnDestroy()
    {
        OnDetailPopupClosed();
    }

    public void SetUserValue([NotNull] UserData inUserData)
    {
        m_UserNameLabel.text = inUserData.UserName;
        m_PlayItem.m_UserName = inUserData.UserName;
    }

    public void SetValue(PlayItem inItem)
    {
        if(inItem == null)
        {
            for (int i = 0, end = m_Items.Length; i < end; i++)
                m_Items[i].text = "";

            m_EnableImg.gameObject.SetActive(false);
        }
        else
        {
            int nSize = inItem.m_Items.Count;
            for (int i = 0, end = m_Items.Length; i < end; i++)
            {
                if (i < nSize)
                    m_Items[i].text = inItem.m_Items[i].GetStandardValue();
                else
                    m_Items[i].text = "";
            }

            m_EnableImg.gameObject.SetActive(true);
            SetEnable(m_PlayItem.m_Complete);
        }
    }

    public void SetEnable(bool isEnable)
    {
        if(m_EnableImg.gameObject.activeInHierarchy)
        {
            m_EnableImg.overrideSprite = isEnable ? m_EnableTex : m_DisableTex;
        }
    }

    private void OnClicked()
    {
        m_DetailPopup = Instantiate(m_DetailPrefab);
        if(m_DetailPopup != null)
        {
            var detail = m_DetailPopup.GetComponent<UserDetailPopup_uGUI>();
            if (detail != null)
            {
                detail.Initialize(m_PlayItem);
                detail.onClosed = OnDetailPopupClosed;
                detail.onApplyBtnClicked = OnDetailPopupApplyClicked;
            }
        }
    }

    private void OnDetailPopupClosed()
    {
        if (m_DetailPopup != null)
            DestroyImmediate(m_DetailPopup);
        m_DetailPopup = null;
    }

    private void OnDetailPopupApplyClicked(string inName, string inWeapon, string inPlace, bool isComplete)
    {
        // 이름 값 할당
        var targetIdx = m_PlayItem.m_Items.FindIndex((value) => { return value.m_Type == eDataType.Name; });
        if (targetIdx == -1)
        {
            if (string.IsNullOrEmpty(inName) == false)
                m_PlayItem.m_Items.Add(new PlayDetailItem() { m_Type = eDataType.Name, m_Value = new string(inName) });
        }
        else
            m_PlayItem.m_Items[targetIdx].m_Value = new string(inName);

        // 무기 값 할당
        targetIdx = m_PlayItem.m_Items.FindIndex((value) => { return value.m_Type == eDataType.Weapon; });
        if (targetIdx == -1)
        {
            if (string.IsNullOrEmpty(inWeapon) == false)
                m_PlayItem.m_Items.Add(new PlayDetailItem() { m_Type = eDataType.Weapon, m_Value = new string(inWeapon) });
        }
        else
            m_PlayItem.m_Items[targetIdx].m_Value = new string(inWeapon);

        // 장소 값 할당
        targetIdx = m_PlayItem.m_Items.FindIndex((value) => { return value.m_Type == eDataType.Place; });
        if (targetIdx == -1)
        {
            if (string.IsNullOrEmpty(inPlace) == false)
                m_PlayItem.m_Items.Add(new PlayDetailItem() { m_Type = eDataType.Place, m_Value = new string(inPlace) });
        }
        else
            m_PlayItem.m_Items[targetIdx].m_Value = new string(inPlace);


        // 턴 사용 여부
        m_PlayItem.m_Complete = isComplete;

        SetValue(m_PlayItem);
    }
}
