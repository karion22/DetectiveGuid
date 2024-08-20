using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class OptionPopup_uGUI : MonoBehaviour
{
    // Player
    [SerializeField] private Slider m_UserSlider = null;
    [SerializeField] private Button m_UserUpButton = null;
    [SerializeField] private Button m_UserDownButton = null;
    [SerializeField] private TMP_InputField m_UserInputField = null;

    [SerializeField] private GridLayoutGroup m_UserGridGroup = null;
    [SerializeField] private GameObject m_UserItemPrefab = null;

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
        if (m_UserDownButton != null)
        {
            m_UserDownButton.onClick.RemoveAllListeners();
            m_UserDownButton.onClick.AddListener(() => {
                var newValue = Mathf.Max(1, m_UserCount - 1);
                UpdateUserSlider(newValue);
            });
        }

        if(m_UserUpButton != null)
        {
            m_UserUpButton.onClick.RemoveAllListeners();
            m_UserUpButton.onClick.AddListener(() => {
                var newValue = Mathf.Min(GameMgr.MAX_USER_COUNT, m_UserCount + 1);
                UpdateUserSlider(newValue);
            });
        }

        if(m_UserSlider != null)
        {
            m_UserSlider.minValue = 1;
            m_UserSlider.maxValue = GameMgr.MAX_USER_COUNT;
            m_UserSlider.value = m_UserCount;

            m_UserSlider.onValueChanged.RemoveAllListeners();
            m_UserSlider.onValueChanged.AddListener((value) => {
                int newValue = (int)value;
                UpdateUserLabel(newValue);
                RebuildUserList(m_UserCount, newValue);
                m_UserCount = newValue;
            });
        }

        if(m_UserInputField != null)
        {
            UpdateUserLabel(m_UserCount);
            m_UserInputField.onValueChanged.RemoveAllListeners();
            m_UserInputField.onValueChanged.AddListener((value) => {
                int newValue = 0;
                if(int.TryParse(value, out newValue))
                {
                    if(newValue < 1 || newValue > GameMgr.MAX_USER_COUNT)
                        m_UserInputField.text = Mathf.Max(Mathf.Min(newValue, 1), GameMgr.MAX_USER_COUNT).ToString();
                    else
                        UpdateUserSlider(newValue);
                }
            });
        }

        RebuildUserList(0, m_UserCount);
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
        m_UserSlider.value = inValue;
    }

    private void UpdateUserLabel(int inValue)
    {
        m_UserInputField.text = inValue.ToString();

    }

    private void RebuildUserList(int inPrevValue, int inNewValue)
    {
        var rt = m_UserGridGroup.transform as RectTransform;
        if (rt != null)
        {
            int nHeight = Mathf.Max(1, ((inNewValue + 1) / 2));

            Vector2 sizedt = rt.sizeDelta;
            sizedt.y = (m_UserGridGroup.cellSize.y * nHeight) + (m_UserGridGroup.padding.top + m_UserGridGroup.padding.bottom);
            rt.sizeDelta = sizedt;

            // Destroy Items
            for (int i = rt.childCount - 1; i >= inNewValue; i--)
            {
                var child = rt.GetChild(i);
                if (child != null)
                    DestroyImmediate(child.gameObject);
            }

            // Add Items
            int nDiff = inNewValue - inPrevValue;
            for (int i = 0; i < nDiff; i++)
            {
                var child = Instantiate(m_UserItemPrefab, m_UserGridGroup.gameObject.transform);
                if (child != null)
                {
                    var item = child.GetComponent<UserNameItemPanel_uGUI>();
                    if (item != null)
                        item.SetItem("");
                }
            }
        }
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
