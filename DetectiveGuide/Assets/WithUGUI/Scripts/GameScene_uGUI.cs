using KRN.Utility;
using TMPro;
using UnityEditor.Build.Content;
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

        for(int i = 0; i < GameMgr.Instance.UserCount; i++)
        {
            Instantiate(m_ItemPrefab, m_ScrollRect.content);              
        }
    }
}
