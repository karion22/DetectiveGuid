using System.Collections;
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


}
