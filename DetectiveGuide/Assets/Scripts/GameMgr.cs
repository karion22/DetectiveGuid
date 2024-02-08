using System.Collections.Generic;
using UnityEngine;

public enum eGameMode
{
    Clue,
    HarryPotter,
    Custom
}

public class UserData
{
    public string UserName;
    public List<string> Items = new List<string>();
}

public class ResultData
{
    public string Name;         // �̸�
    public string Weapon;       // ����
    public string Place;        // ���
}

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
    public int UseItemCount = 3;

    // �ִ� �ο� ��
    public int UsePlayerCount = 5;

    // �÷��� ���� ����
    public int UserCount = 4;

    // ���� ���� ��
    private ClueData _dataSet = new ClueData();

    // ���� ��
    private List<UserData> _dataList = new List<UserData>();

    // ����
    private List<ResultData> _stack = new List<ResultData>();

    public void SetGameMode(eGameMode inGameMode)
    {
        if(GameMode != inGameMode)
        {
            GameMode = inGameMode;
            _dataSet.Clear();

            switch (GameMode)
            {
                case eGameMode.Clue:
                    {
                        UsePlayerCount = 4;
                        UseItemCount = 3;

                        string[] Names = { "����", "�÷�", "��Į��", "�ӽ�Ÿ��", "�׸�", "ȭ��Ʈ" };
                        string[] Weapon = { "������", "����", "�ܰ�", "��ġ", "����", "�д�" };
                        string[] Place = { "ħ��", "���", "����", "�Ĵ�", "����", "���ӷ�", "����", "�Ž�", "�ξ�" };

                        _dataSet.Names.AddRange(Names);
                        _dataSet.Weapons.AddRange(Weapon);
                        _dataSet.Places.AddRange(Place);
                    }
                    break;

                case eGameMode.HarryPotter:
                    break;
            }

            Restart();
        }
    }

    private void Restart()
    {
        _stack.Clear();
    }
}
