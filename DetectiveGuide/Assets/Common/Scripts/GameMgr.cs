using KRN.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

// 게임 모드
public enum eGameMode
{
    Clue,
    HarryPotter,
    Custom
}

// 플레이 유저 정보
public class UserData
{
    public string UserName;
    public List<string> Items = new List<string>();

    public void Clear()
    {
        UserName = "";
        Items.Clear();
    }
}

// 진행 정보
public class PlayItem
{
    public string Name;         // 이름
    public string Weapon;       // 무기
    public string Place;        // 장소
}

// 게임 진행 정보
public class PlayHistory
{
    public List<string> Names = new List<string>();
    public List<string> Weapons = new List<string>();
    public List<string> Places = new List<string>();

    public void Clear()
    {
        Names.Clear();
        Weapons.Clear();
        Places.Clear();
    }
}

public class GameMgr : Singleton<GameMgr>
{
    // 게임 모드
    public eGameMode GameMode = eGameMode.Clue;

    // 사용되는 아이템 개수
    public int ItemCount = 3;

    // 최대 아이템 개수
    public const int MAX_ITEM_COUNT = 4;

    // 플레이 유저 숫자
    public int UserCount = 4;

    // 최대 인원 수
    public const int MAX_USER_COUNT = 8;

    // 게임 셋팅 값
    private GameDataSetScriptable _dataSet;
    private List<GameDataSetScriptable> _dataSetList;

    // 유저 값
    private List<UserData> _userList = new List<UserData>();

    // 플레이 정보
    private List<PlayHistory> _playStack = new List<PlayHistory>();

    public override void Awake()
    {
        base.Awake();

        _dataSetList.Clear();
        var async = Addressables.LoadAssetsAsync<GameDataSetScriptable>("GameData/GameRules", (value) => { _dataSetList.Add(value); });
    }

    public void SetGameMode(eGameMode inGameMode)
    {
        GameMode = inGameMode;
        _playStack.Clear();

         //(int)(GameMode);
        switch (GameMode)
        {
            case eGameMode.Clue:
                break;

            case eGameMode.HarryPotter:
                // TODO
                break;

            case eGameMode.Custom:
                break;
        }

        Restart();
    }

    #region Item Count Control
    // 아이템 개수를 증가시킨다.
    public void IncreaseItemCount(int inValue = 1)
    {
        ItemCount = Mathf.Min(ItemCount + inValue, MAX_ITEM_COUNT);
    }

    // 아이템 개수를 감소시킨다.
    public void DecreaseItemCount(int inValue = 1)
    {
        ItemCount = Mathf.Max(ItemCount - inValue, 0);
    }
    #endregion

    #region User Count Control
    // 유저 수를 증가시킨다.
    public void IncreaseUserCount(int inValue = 1)
    {
        UserCount = Mathf.Min(UserCount + inValue, MAX_USER_COUNT);
    }

    // 유저 수를 감소시킨다.
    public void DecreaseUserCount(int inValue = 1)
    {
        UserCount = Mathf.Max(UserCount - inValue, 0);
    }
    #endregion

    #region User Info Control
    public void ClearUserList()
    {
        _userList.Clear();
    }

    public void AddUserData(UserData inUserData)
    {
        if (_userList.Contains(inUserData) == false)
            _userList.Add(inUserData);
    }

    public void RemoveUserData(UserData inUserData)
    {
        _userList.Remove(inUserData);
    }
    #endregion

    public void Restart()
    {
        _playStack.Clear();
    }
}