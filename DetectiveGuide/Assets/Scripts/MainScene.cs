using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainScene : MonoBehaviour
{
    private UIDocument _document = null;

    private EnumField _modeField = null;

    private Label _playerCountLabel = null;
    private Label _itemCountLabel = null;    

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        VisualElement root = _document.rootVisualElement;

        _modeField = root.Q<EnumField>("GameModePopup");
        _modeField.RegisterValueChangedCallback((value) => {
            GameMgr.Instance.SetGameMode((eGameMode)value.newValue);
            UpdatePlayerCountLabel();
            UpdateItemCountLabel();
        });

        _playerCountLabel = root.Q<Label>("PlayerCountLabel");
        _itemCountLabel = root.Q<Label>("ItemCountLabel");

        root.Q<Button>("PlayerCountAddBtn").clicked += () => {
            GameMgr.Instance.IncreaseUserCount();
            UpdatePlayerCountLabel();
        };
        root.Q<Button>("PlayerCountMinusBtn").clicked += () => {
            GameMgr.Instance.DecreaseUserCount();
            UpdatePlayerCountLabel();
        };

        root.Q<Button>("ItemCountAddBtn").clicked += () => {
            GameMgr.Instance.IncreaseItemCount();
            UpdateItemCountLabel();
        };
        root.Q<Button>("ItemCountMinusBtn").clicked += () => {
            GameMgr.Instance.DecreaseItemCount();
            UpdateItemCountLabel();
        };

        root.Q<Button>("StartBtn").clicked += () => {
            SceneManager.LoadScene("PlayerSetScene");
        };
    }

    private void Start()
    {
        UpdatePlayerCountLabel();
        UpdateItemCountLabel();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Return) || Input.GetKeyUp(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void UpdatePlayerCountLabel()
    {
        _playerCountLabel.text = GameMgr.Instance.UserCount.ToString();
    }

    private void UpdateItemCountLabel()
    {
        _itemCountLabel.text = GameMgr.Instance.ItemCount.ToString();
    }
}
