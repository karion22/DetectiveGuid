using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSubItem_uGUI : MonoBehaviour
{
    [SerializeField] private Text m_Title = null;
    [SerializeField] private Text m_Description = null;

    public void SetTitle(string inText) { m_Title.text = inText; }
    public void SetDescription(string inText) {m_Description.text = inText; }
}
