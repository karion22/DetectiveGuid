using KRN.Utility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Windows;

// 게임 모드
public enum eGameMode
{
    Clue,
    HarryPotter,
    Custom,
}

public enum eDataType
{
    Name,
    Weapon,
    Place,
}

// 플레이 유저 정보
public class UserData
{
    public string UserName;
    public List<PlayDetailItem> Items = new List<PlayDetailItem>();

    public void Clear()
    {
        UserName = "";
        Items.Clear();
    }
}

// 진행 정보
public class PlayDetailItem
{
    public eDataType m_Type;
    public string m_Value;

    public string GetStandardValue()
    {
        return Utility.BuildString("<color=#dedede>{0}</color> : {1}", GetString(), m_Value);
    }

    private string GetString()
    {
        return (m_Type switch
        {
            eDataType.Name => "이름",
            eDataType.Weapon => "무기",
            eDataType.Place => "장소",
            _ => ""
        });
    }

    public void CopyTo(PlayDetailItem inItem)
    {
        inItem.m_Type = m_Type;
        inItem.m_Value = new string(m_Value);
    }
}

public class PlayItem
{
    public int m_Turn;
    public string m_UserName;
    public bool m_Complete;
    public List<PlayDetailItem> m_Items = new List<PlayDetailItem>();

    public void Add(PlayDetailItem inItem)
    {
        m_Items.Add(inItem);
    }

    public void Remove(eDataType inType, string inValue)
    {
        var item = m_Items.Find((value) => {
            return (inType == value.m_Type && inValue == value.m_Value);
        });

        if(item != null)
            m_Items?.Remove(item);
    }

    public void CopyTo(PlayItem inItem)
    {
        inItem.m_Turn = m_Turn;
        inItem.m_UserName = new string(m_UserName);
        inItem.m_Items.Clear();
        foreach (var item in m_Items)
        {
            PlayDetailItem detailItem = new PlayDetailItem();
            item.CopyTo(detailItem);
            inItem.m_Items.Add(detailItem);
        }
    }
}

public class ResultItem
{
    public string Name;
    public ResultSubItem[] Items;
}

public class ResultSubItem
{
    public string Name;
    public string Value;
}

// 게임 진행 정보
public class PlayHistory
{
    public List<PlayItem> m_Items = new List<PlayItem>();

    public void Add(PlayItem item)
    {
        m_Items.Add(item);
    }

    public void Remove(int inTurn, string inUserName)
    {
        var target = m_Items.Find((value) => {
        if (value.m_Turn == inTurn && value.m_UserName.Equals(inUserName))
            return true;
        else
            return false;
        });

        if (target != null)
            m_Items.Remove(target);
    }

    public void Clear()
    {
        m_Items.Clear();
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
    private GameDataSetScriptable m_DataSet;
    public GameDataSetScriptable DataSet
    {
        get { return m_DataSet; }
    }

    private List<GameDataSetScriptable> m_DataSetList = new List<GameDataSetScriptable>();

    // 유저 값
    private List<UserData> m_UserList = new List<UserData>();
    public List<UserData> UserList
    {
        get { return m_UserList; }
    }

    // 플레이 정보
    public List<PlayHistory> m_PlayStack = new List<PlayHistory>();

    // 
    public bool m_IsReady = false;

    public override void Awake()
    {
        base.Awake();

        m_IsReady = false;

        m_DataSetList.Clear();

        var handle = Addressables.LoadAssetsAsync<GameDataSetScriptable>("ruledata", (value) => { m_DataSetList.Add(value); });
        handle.Completed += (result) => {
            m_IsReady = true;
        };
    }

    public void SetGameMode(eGameMode inGameMode)
    {
        GameMode = inGameMode;

        switch (GameMode)
        {
            case eGameMode.Clue:
                m_DataSet = m_DataSetList[0];
                break;

            case eGameMode.HarryPotter:
                // TODO
                break;

            case eGameMode.Custom:
                break;

            default:
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
        ItemCount = Mathf.Max(ItemCount - inValue, 1);
    }

    public void SetItemCount(int inValue)
    {
        ItemCount = Mathf.Max(Mathf.Min(inValue, MAX_ITEM_COUNT), 1);
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
        UserCount = Mathf.Max(UserCount - inValue, 1);
    }

    public void SetUserCount(int inValue)
    {
        UserCount = Mathf.Max(Mathf.Min(inValue, MAX_USER_COUNT), 1);
    }
    #endregion

    #region User Info Control
    public void BuildUserList()
    {
        m_UserList.Clear();
        for (int i = 0; i < UserCount; i++)
            m_UserList.Add(new UserData());
    }

    public void AddUserData(UserData inUserData)
    {
        if (m_UserList.Contains(inUserData) == false)
            m_UserList.Add(inUserData);
    }

    public void RemoveUserData(int inIndex)
    {
        m_UserList.RemoveAt(inIndex);
    }

    public void RemoveUserData(UserData inUserData)
    {
        m_UserList.Remove(inUserData);
    }

    public void SetUserData(int inIndex, string inValue)
    {
        if (inIndex < 0 || inIndex >= m_UserList.Count)
            DebugLog.PrintError(Utility.BuildString("Index value is wrong. {0}", inIndex));
        else
            m_UserList[inIndex].UserName = inValue;
    }

    public void SetUserDataList(List<UserData> inUserList)
    {
        m_UserList.Clear();
        m_UserList.AddRange(inUserList);
    }

    public UserData GetUserData(int inIndex)
    {
        if (inIndex < 0 || inIndex >= m_UserList.Count)
        {
            DebugLog.PrintError(Utility.BuildString("Index value is wrong. {0}", inIndex));
            return null;
        }
        else
            return m_UserList[inIndex];
    }
    #endregion

    public void Restart()
    {
        m_PlayStack.Clear();
    }
}