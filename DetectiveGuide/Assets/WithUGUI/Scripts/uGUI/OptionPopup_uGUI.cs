using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionPopup_uGUI : MonoBehaviour
{
    // Player
    [SerializeField] private Slider m_PlayerSlider = null;
    [SerializeField] private Button m_PlayerLeftButton = null;
    [SerializeField] private Button m_PlayerRightButton = null;
    [SerializeField] private TMP_InputField m_PlayerInputField = null;

    // Item
    [SerializeField] private Slider m_ItemSlider = null;
    [SerializeField] private Button m_ItemLeftButton = null;
    [SerializeField] private Button m_ItemRightButton = null;
    [SerializeField] private TMP_InputField m_ItemInputField = null;

    // 
    [SerializeField] private Button m_ApplyBtn = null;
    [SerializeField] private Button m_CloseBtn = null;

    //
    private int m_UserCount = 1;
    private int m_ItemCount = 1;

    private void Start()
    {
        m_UserCount = GameMgr.Instance.UserCount;
        m_ItemCount = GameMgr.Instance.ItemCount;

        #region Player
        if (m_PlayerLeftButton != null)
        {
            m_PlayerLeftButton.onClick.RemoveAllListeners();
            m_PlayerLeftButton.onClick.AddListener(() => {
                m_UserCount = Mathf.Max(1, m_UserCount - 1);
                UpdateUserSlider(m_UserCount);
            });
        }

        if(m_PlayerRightButton != null)
        {
            m_PlayerRightButton.onClick.RemoveAllListeners();
            m_PlayerRightButton.onClick.AddListener(() => {
                m_UserCount = Mathf.Min(GameMgr.MAX_USER_COUNT, m_UserCount + 1);
                UpdateUserSlider(m_UserCount);
            });
        }

        if(m_PlayerSlider != null)
        {
            m_PlayerSlider.minValue = 1;
            m_PlayerSlider.maxValue = GameMgr.MAX_USER_COUNT;
            m_PlayerSlider.value = m_UserCount;

            m_PlayerSlider.onValueChanged.RemoveAllListeners();
            m_PlayerSlider.onValueChanged.AddListener((value) => {
                m_UserCount = (int)value;
                UpdateUserLabel();
            });
        }

        if(m_PlayerInputField != null)
        {
            UpdateUserLabel();
        }
        #endregion

        #region Item
        if (m_ItemLeftButton != null)
        {
            m_ItemLeftButton.onClick.RemoveAllListeners();
            m_ItemLeftButton.onClick.AddListener(() => {
                m_ItemCount = Mathf.Max(1, m_ItemCount - 1);
                UpdateItemSlider(m_ItemCount);
            });
        }

        if (m_ItemRightButton != null)
        {
            m_ItemRightButton.onClick.RemoveAllListeners();
            m_ItemRightButton.onClick.AddListener(() => {
                m_ItemCount = Mathf.Min(GameMgr.MAX_ITEM_COUNT, m_ItemCount + 1);
                UpdateItemSlider(m_ItemCount);
            });
        }

        if (m_ItemSlider != null)
        {
            m_ItemSlider.minValue = 1;
            m_ItemSlider.maxValue = GameMgr.MAX_ITEM_COUNT;
            m_ItemSlider.value = m_ItemCount;

            m_ItemSlider.onValueChanged.RemoveAllListeners();
            m_ItemSlider.onValueChanged.AddListener((value) => {
                m_ItemCount = (int)value;
                UpdateItemLabel();
            });
        }

        if (m_ItemInputField != null)
        {
            UpdateItemLabel();
        }
        #endregion

        if(m_ApplyBtn != null)
        {
            m_ApplyBtn.onClick.RemoveAllListeners();
            m_ApplyBtn.onClick.AddListener(() => {
                GameMgr.Instance.SetUserCount(m_UserCount);
                GameMgr.Instance.SetItemCount(m_ItemCount);
                Close();
            });
        }

        if(m_CloseBtn != null)
        {
            m_CloseBtn.onClick.RemoveAllListeners();
            m_CloseBtn.onClick.AddListener(() => { 
                Close(); 
            });
        }
    }

    private void UpdateUserSlider(int inValue)
    {
        m_PlayerSlider.value = inValue;
    }

    private void UpdateUserLabel()
    {
        m_PlayerInputField.text = GameMgr.Instance.UserCount.ToString();
    }

    private void UpdateItemSlider(int inValue)
    {
        m_ItemSlider.value = inValue;
    }

    private void UpdateItemLabel()
    {
        m_ItemInputField.text = GameMgr.Instance.ItemCount.ToString();
    }

    private void Close()
    {
        Destroy(gameObject);
    }
}
