using System.Collections.Generic;
using UnityEngine;

// ���� ���
public enum eGameMode
{
    Clue,
    HarryPotter,
    Custom
}

// �÷��� ���� ����
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

// ��� ����
public class ResultData
{
    public string Name;         // �̸�
    public string Weapon;       // ����
    public string Place;        // ���
}

// ���� ���� ����
public class ClueData
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
    // ���� ���
    public eGameMode GameMode = eGameMode.Clue;

    // ���Ǵ� ������ ����
    public int ItemCount = 3;

    // �ִ� ������ ����
    public const int MAX_ITEM_COUNT = 4;

    // �÷��� ���� ����
    public int UserCount = 4;

    // �ִ� �ο� ��
    public const int MAX_USER_COUNT = 8;

    // ���� ���� ��
    private ClueData _dataSet = new ClueData();

    // ���� ��
    private List<UserData> _userList = new List<UserData>();

    // �÷��� ����
    private List<ResultData> _playStack = new List<ResultData>();

    public void SetGameMode(eGameMode inGameMode)
    {
        GameMode = inGameMode;
        _dataSet.Clear();

        switch (GameMode)
        {
            case eGameMode.Clue:
                {
                    UserCount = 4;
                    ItemCount = 3;

                    string[] Names = { "����", "�÷�", "��Į��", "�ӽ�Ÿ��", "�׸�", "ȭ��Ʈ" };
                    string[] Weapon = { "������", "����", "�ܰ�", "��ġ", "����", "�д�" };
                    string[] Place = { "ħ��", "���", "����", "�Ĵ�", "����", "���ӷ�", "����", "�Ž�", "�ξ�" };

                    _dataSet.Names.AddRange(Names);
                    _dataSet.Weapons.AddRange(Weapon);
                    _dataSet.Places.AddRange(Place);
                }
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
    // ������ ������ ������Ų��.
    public void IncreaseItemCount(int inValue = 1)
    {
        ItemCount = Mathf.Min(ItemCount + inValue, MAX_ITEM_COUNT);
    }

    // ������ ������ ���ҽ�Ų��.
    public void DecreaseItemCount(int inValue = 1)
    {
        ItemCount = Mathf.Max(ItemCount - inValue, 0);
    }
    #endregion

    #region User Count Control
    // ���� ���� ������Ų��.
    public void IncreaseUserCount(int inValue = 1)
    {
        UserCount = Mathf.Min(UserCount + inValue, MAX_USER_COUNT);
    }

    // ���� ���� ���ҽ�Ų��.
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