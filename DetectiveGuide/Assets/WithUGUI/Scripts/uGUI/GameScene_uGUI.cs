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
    [SerializeField] private ScrollRect m_ScrollRect = null;
    #endregion

    #region Bottom Panel
    [SerializeField] private Button m_LogBtn = null;
    #endregion

    [SerializeField] private GameObject m_ItemPrefab = null;
    private List<UserCardPanel_uGUI> m_Items = new List<UserCardPanel_uGUI>();

    [SerializeField] private GameObject m_SettingPrefab = null;

    [ExecuteInEditMode]
    [ContextMenu("Auto Find UI")]
    public void AutoFindUI()
    {
        m_PlayerCountText = transform.FindEx<TMP_Text>("PlayerCount");
        m_PlayTurnText = transform.FindEx<TMP_Text>("PlayerTurn");
        m_SettingBtn = transform.FindEx<Button>("SettingBtn");

        m_ScrollRect = transform.FindEx<ScrollRect>("ScrollView");

        m_LogBtn = transform.FindEx<Button>("LogButton");
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        m_SettingBtn.onClick.RemoveAllListeners();
        m_SettingBtn.onClick.AddListener(() => {
            Instantiate(m_SettingPrefab);
        });

        for(int i = 0; i < GameMgr.Instance.UserCount; i++)
        {
            var item = Instantiate(m_ItemPrefab, m_ScrollRect.content);              
            if(item != null)
            {
                 var cardPanel = item.GetComponent<UserCardPanel_uGUI>();
                m_Items.Add(cardPanel);
            }
        }
    }
}
