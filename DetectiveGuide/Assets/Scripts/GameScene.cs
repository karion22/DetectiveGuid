using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameScene : MonoBehaviour
{
    private UIDocument _document = null;
    private ListView _listView = null;

    private VisualTreeAsset _cardItemPrefab = null;
    private List<CardItem> _cardItems = new List<CardItem>();

    private void Awake()
    {
        _document= GetComponent<UIDocument>();

        VisualElement root = _document.rootVisualElement;

        root.Q<Button>("ReturnBtn").clicked += () => {
            SceneManager.LoadScene("MenuScene");
        };

        root.Q<Button>("LogBtn").clicked += () => { };

        root.Q<Button>("TableBtn").clicked += () => { };

        _cardItemPrefab = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/UI/UXML/UXML_CardItemDocument.uxml");

        _cardItems.Clear();
        for(int i = 0, end = GameMgr.Instance.UsePlayerCount; i < end; i++)
        {
            CardItem item = new CardItem();
            //item.UserName = 
        }
        _listView = root.Q<ListView>("ItemList");
        if(_listView != null)
        {
            _listView.makeItem = () => _cardItemPrefab.CloneTree();
            _listView.bindItem = (element, index) => {
                int maxCount = _cardItems.Count;
                int useItem = GameMgr.Instance.UseItemCount;

                if (index < 0 || index >= maxCount || _cardItems[index] == null)
                {
                    element.Q<Label>("NameText").text = "Unknown";
                    element.Q<Label>("CharacterLabel").text = "Unknown";
                    
                    for(int i = 0; i < 5; i++)
                        element.Q<Label>($"CertainItemLabel" + (i + 1)).text = "";
                }
                else
                {
                    element.Q<Label>("NameText").text = _cardItems[index].UserName;
                    element.Q<Label>("CharacterLabel").text = _cardItems[index].CharacterName;

                    for (int i = 0; i < 5; i++)
                    {
                        if(i < useItem)
                            element.Q<Label>($"CertainItemLabel" + (i + 1)).text = _cardItems[index].Items[i];
                        else
                            element.Q<Label>($"CertainItemLabel" + (i + 1)).text = "";
                    }
                }
            };

            _listView.selectionType = SelectionType.Single;
            _listView.itemIndexChanged += OnItemSelectChanged;
        }
    }

    private void OnItemSelectChanged(int inPrevValue, int inNewValue)
    {

    }
}
