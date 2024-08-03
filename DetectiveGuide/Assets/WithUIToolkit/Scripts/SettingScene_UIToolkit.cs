using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Rendering.FilterWindow;

public class SettingScene_UIToolkit : MonoBehaviour
{
    private UIDocument _document = null;

    private List<Button> _tabBtns = new List<Button>();
    private TextField _userName = null;
    private DropdownField _playerName = null;

    private List<DropdownField> _items = new List<DropdownField>();

    private int _selectedTabIdx = 0;

    private const string SELECT_TAB = ".currentSelectedTab";
    private const string DESELECT_TAB = ".tab";

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        VisualElement root = _document.rootVisualElement;

        _tabBtns.Clear();
        for(int i = 0; i < GameMgr.MAX_USER_COUNT; i++)
        {
            _tabBtns[i] = root.Q<Button>($"Button" + (i + 1));

            if(i < GameMgr.Instance.UserCount)
            {
                _tabBtns[i].style.display = DisplayStyle.Flex;
                _tabBtns[i].clicked += () => {

                    if (_selectedTabIdx != i)
                    {
                        _tabBtns[_selectedTabIdx].RemoveFromClassList(SELECT_TAB);
                        _tabBtns[_selectedTabIdx].AddToClassList(DESELECT_TAB);

                        _tabBtns[i].AddToClassList(SELECT_TAB);
                        _tabBtns[i].RemoveFromClassList(DESELECT_TAB);

                        _selectedTabIdx = i;
                    }
                };
            }
            else
            {
                _tabBtns[i].style.display = DisplayStyle.None;
            }
        }

        

    }
}
