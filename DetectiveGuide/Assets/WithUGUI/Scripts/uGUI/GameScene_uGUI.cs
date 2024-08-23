using KRN.Utility;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene_uGUI : GameScene
{
    #region Top Panel
    [SerializeField] private TMP_Text m_PlayerCountText = null;
    [SerializeField] private TMP_Text m_PlayTurnText = null;
    [SerializeField] private Button m_SettingBtn = null;
    #endregion

    #region Body Panel
    [SerializeField] private PlayItem_uGUI[] m_PlayItems = null;
    [SerializeField] private ScrollRect m_ScrollRect = null;
    #endregion

    #region Bottom Panel
    [SerializeField] private Button m_LogBtn = null;
    [SerializeField] private Button m_NextBtn = null;
    #endregion

    [SerializeField] private GameObject m_ItemPrefab = null;
    private List<UserCardPanel_uGUI> m_Items = new List<UserCardPanel_uGUI>();

    [SerializeField] private GameObject m_SettingPrefab = null;
    private List<GameObject> m_PopupList = new List<GameObject>();

    [ExecuteInEditMode]
    [ContextMenu("Auto Find UI")]
    public void AutoFindUI()
    {
        m_PlayerCountText = transform.FindEx<TMP_Text>("PlayerCount");
        m_PlayTurnText = transform.FindEx<TMP_Text>("PlayerTurn");
        m_SettingBtn = transform.FindEx<Button>("SettingBtn");

        m_ScrollRect = transform.FindEx<ScrollRect>("ScrollView");

        m_LogBtn = transform.FindEx<Button>("LogButton");
        m_NextBtn = transform.FindEx<Button>("NextButton");
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_SettingBtn.onClick.RemoveAllListeners();
        m_SettingBtn.onClick.AddListener(() =>
        {
            var popup = ShowPopup<OptionPopup_uGUI>(m_SettingPrefab, true);
            if (popup != null)
                popup.onClosed = CloseSettingPopup;
        });

        m_LogBtn.onClick.RemoveAllListeners();
        m_LogBtn.onClick.AddListener(() => { 

        });

        m_NextBtn.onClick.RemoveAllListeners();
        m_NextBtn.onClick.AddListener(() => {
            NextTurn();
        });

        RebuildPlayItem();
        RebuildItems();
    }

    private void RebuildPlayItem()
    {
        for(int i = 0, end = m_PlayItems.Length; i < end; i++)
        {
            m_PlayItems[i].Initialize();
        }
    }

    private void RebuildItems()
    {
        // Remove Items
        for (int i = m_Items.Count - 1; i >= 0; i--)
            DestroyImmediate(m_Items[i].gameObject);
        m_Items.Clear();

        // Add Items
        for (int i = 0; i < GameMgr.Instance.UserCount; i++)
        {
            var item = Instantiate(m_ItemPrefab, m_ScrollRect.content);
            if (item != null)
            {
                var card = item.GetComponent<UserCardPanel_uGUI>();
                card.Initialize();
                card.SetUserValue(GameMgr.Instance.UserList[i]);
                card.SetValue(null);
                m_Items.Add(card);
            }
        }

        // UI Update
        m_PlayerCountText.text = GameMgr.Instance.UserCount.ToString();

        UpdateTurn();
    }

    private void UpdateTurn()
    {
        m_PlayTurnText.text = GameMgr.Instance.GameTurn.ToString();
    }

    private void NextTurn()
    {
        GameMgr.Instance.IncreaseTurn();

        for (int i = 0, end = m_Items.Count; i < end; i++)
            m_Items[i].SetEnable(true);

        UpdateTurn();
    }

    private T ShowPopup<T>(GameObject inPrefab, bool isAlone)
    {
        if(isAlone && m_PopupList.Count > 0)
        {
            for (int i = m_PopupList.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(m_PopupList[i]);
                m_PopupList[i] = null;
            }
            m_PopupList.Clear();
        }

        var targetGo = Instantiate(m_SettingPrefab);
        m_PopupList.Add(targetGo);

        return targetGo.GetComponent<T>();
    }

    private void ClosePopup(GameObject inPopup)
    {
        if (inPopup != null)
        {
            if (m_PopupList.Remove(inPopup))
                DestroyImmediate(inPopup);
        }
    }

    public void CloseSettingPopup(GameObject inPopup)
    {
        ClosePopup(inPopup);
        RebuildItems();
    }
}
