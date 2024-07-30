using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameScene : MonoBehaviour
{
    private ListView m_ListView = null;

    private VisualTreeAsset m_CardItemPrefab = null;
    private List<CardItem> m_CardItems = new List<CardItem>();

    private void Awake()
    {
        var document = GetComponent<UIDocument>();
        VisualElement root = document.rootVisualElement;

        // 프리팹 로드
        m_CardItemPrefab = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/UXML/UXML_CardItemDocument.uxml");

        // 데이터 초기화
        m_CardItems.Clear();
        for(int i = 0, end = GameMgr.Instance.UserCount; i < end; i++)
        {
            CardItem item = new CardItem();
            //item.UserName = 
        }

        // 리스트에 프리팹 설정
        m_ListView = root.Q<ListView>("ItemList");
        if(m_ListView != null)
        {
            m_ListView.makeItem = () => m_CardItemPrefab.CloneTree();
            m_ListView.bindItem = (element, index) => {
                int maxCount = m_CardItems.Count;
                int useItem = GameMgr.Instance.ItemCount;

                if (index < 0 || index >= maxCount || m_CardItems[index] == null)
                {
                    element.Q<Label>("NameText").text = "Unknown";
                    element.Q<Label>("CharacterLabel").text = "Unknown";
                    
                    for(int i = 0; i < 5; i++)
                        element.Q<Label>($"CertainItemLabel" + (i + 1)).text = "";
                }
                else
                {
                    element.Q<Label>("NameText").text = m_CardItems[index].UserName;
                    element.Q<Label>("CharacterLabel").text = m_CardItems[index].CharacterName;

                    for (int i = 0; i < 5; i++)
                    {
                        if(i < useItem)
                            element.Q<Label>($"CertainItemLabel" + (i + 1)).text = m_CardItems[index].Items[i];
                        else
                            element.Q<Label>($"CertainItemLabel" + (i + 1)).text = "";
                    }
                }
            };
            m_ListView.itemsSource = m_CardItems;

            m_ListView.selectionType = SelectionType.Single;
            m_ListView.itemIndexChanged += OnItemSelectChanged;
            m_ListView.RefreshItems();
        }

        // 버튼 이벤트 처리
        root.Q<Button>("ReturnBtn").clicked += () => {
            SceneManager.LoadScene("MenuScene");
        };

        root.Q<Button>("LogBtn").clicked += () => {
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/UXML/UXML_CardPopupItemDocument.uxml");
            if (asset != null)
            {
                VisualElement popup = asset.Instantiate();
                UIUtility.SetPopup(popup);
                root.Add(popup);
            }
        };

        root.Q<Button>("TableBtn").clicked += () => { };

    }

    private void OnItemSelectChanged(int inPrevValue, int inNewValue)
    {

    }
}
