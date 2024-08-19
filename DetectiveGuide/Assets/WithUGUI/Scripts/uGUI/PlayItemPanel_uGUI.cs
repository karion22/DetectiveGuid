using TMPro;
using UnityEngine;

public class PlayItem_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_TitleLabel;
    [SerializeField] private PlaySubItem_uGUI[] m_SubItems;

    public void Initialize()
    {
        //UpdateItems();
    }

    public void UpdateItems(PlayItem inItem)
    {
        int nCount = inItem.m_Items.Count;
        for (int i = 0, end = m_SubItems.Length; i < end; i++)
        {
            if(i < nCount)
            {
                m_SubItems[i].SetTitle(inItem.m_Items[i].GetStandardValue());
                m_SubItems[i].SetValue(inItem.m_Items[i].m_Value);
            }
            else
            {
                m_SubItems[i].SetTitle("");
                m_SubItems[i].SetValue("");
            }
        }
    }
}
