using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionPopup_uGUI : MonoBehaviour
{
    // Player
    [SerializeField] private Slider m_PlayerSlider = null;
    [SerializeField] private Button m_PlayerUpButton = null;
    [SerializeField] private Button m_PlayerDownButton = null;
    [SerializeField] private TMP_InputField m_PlayerInputField = null;

    // Item
    [SerializeField] private Slider m_ItemSlider = null;
    [SerializeField] private Button m_ItemUpButton = null;
    [SerializeField] private Button m_ItemDownButton = null;
    [SerializeField] private TMP_InputField m_ItemInputField = null;

    // 
    [SerializeField] private Button m_ApplyBtn = null;
    [SerializeField] private Button m_CloseBtn = null;

    //
    private int m_UserCount = 1;
    private int m_ItemCount = 1;

    public UnityAction<GameObject> onClosed = null;

    private void Start()
    {
        m_UserCount = GameMgr.Instance.UserCount;
        m_ItemCount = GameMgr.Instance.ItemCount;

        #region Player
        if (m_PlayerDownButton != null)
        {
            m_PlayerDownButton.onClick.RemoveAllListeners();
            m_PlayerDownButton.onClick.AddListener(() => {
                m_UserCount = Mathf.Max(1, m_UserCount - 1);
                UpdateUserSlider(m_UserCount);
            });
        }

        if(m_PlayerUpButton != null)
        {
            m_PlayerUpButton.onClick.RemoveAllListeners();
            m_PlayerUpButton.onClick.AddListener(() => {
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
            m_PlayerInputField.onValueChanged.RemoveAllListeners();
            m_PlayerInputField.onValueChanged.AddListener((value) => {
                if(int.TryParse(value, out m_UserCount))
                {
                    if(m_UserCount < 1 || m_UserCount > GameMgr.MAX_USER_COUNT)
                        m_PlayerInputField.text = Mathf.Max(Mathf.Min(m_UserCount, 1), GameMgr.MAX_USER_COUNT).ToString();
                    else
                        UpdateUserSlider(m_UserCount);
                }
            });
        }
        #endregion

        #region Item
        if (m_ItemDownButton != null)
        {
            m_ItemDownButton.onClick.RemoveAllListeners();
            m_ItemDownButton.onClick.AddListener(() => {
                m_ItemCount = Mathf.Max(1, m_ItemCount - 1);
                UpdateItemSlider(m_ItemCount);
            });
        }

        if (m_ItemUpButton != null)
        {
            m_ItemUpButton.onClick.RemoveAllListeners();
            m_ItemUpButton.onClick.AddListener(() => {
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
            m_ItemInputField.onValueChanged.RemoveAllListeners();
            m_ItemInputField.onValueChanged.AddListener((value) => { 
                if(int.TryParse(value, out m_ItemCount))
                {
                    if(m_ItemCount < 1 || m_ItemCount > GameMgr.MAX_ITEM_COUNT)
                        m_ItemInputField.text = Mathf.Max(Mathf.Min(m_ItemCount, 1), GameMgr.MAX_ITEM_COUNT).ToString();
                    else
                        UpdateItemSlider(m_ItemCount);
                }
            });
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
        m_PlayerInputField.text = m_UserCount.ToString();
    }

    private void UpdateItemSlider(int inValue)
    {
        m_ItemSlider.value = inValue;
    }

    private void UpdateItemLabel()
    {
        m_ItemInputField.text = m_ItemCount.ToString();
    }

    private void Close()
    {
        onClosed?.Invoke(this.gameObject);
    }
}
