using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserCardPanel_uGUI : MonoBehaviour
{
    [SerializeField] private TMP_Text m_UserNameLabel = null;
    [SerializeField] private TMP_Text[] m_Items = null;
    [SerializeField] private Image m_EnableImg = null;

    [SerializeField] private Texture2D m_EnableTex = null;
    [SerializeField] private Texture2D m_DisableTex = null;

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

    public void SetValue(PlayItem inItem)
    {
        if(inItem == null)
        {
            m_UserNameLabel.text = "";

            for (int i = 0, end = m_Items.Length; i < end; i++)
                m_Items[i].text = "";

            m_EnableImg.gameObject.SetActive(false);
        }
        else
        {
            m_UserNameLabel.text = inItem.m_UserName;

            int nSize = inItem.m_Items.Count;
            for (int i = 0, end = m_Items.Length; i < end; i++)
            {
                if (i < nSize)
                    m_Items[i].text = inItem.m_Items[i].GetStandardValue();
                else
                    m_Items[i].text = "";
            }

            m_EnableImg.gameObject.SetActive(true);
            SetEnable(true);
        }
    }

    public void SetEnable(bool isEnable)
    {
        if(m_EnableImg.gameObject.activeInHierarchy)
        {
            m_EnableImg.GetComponent<MeshRenderer>().material.mainTexture = isEnable ? m_EnableTex : m_DisableTex;
        }
    }

    private void OnClicked()
    {

    }
}
