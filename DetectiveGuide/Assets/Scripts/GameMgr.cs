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
    public string Name;         // 이름
    public string Weapon;       // 무기
    public string Place;        // 장소
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
    // 게임 모드
    public eGameMode GameMode = eGameMode.Clue;

    // 사용되는 아이템 개수
    public int UseItemCount = 3;

    // 최대 인원 수
    public int UsePlayerCount = 5;

    // 플레이 유저 숫자
    public int UserCount = 4;

    // 게임 셋팅 값
    private ClueData _dataSet = new ClueData();

    // 유저 값
    private List<UserData> _dataList = new List<UserData>();

    // 스택
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

                        string[] Names = { "피콕", "플럼", "스칼렛", "머스타드", "그린", "화이트" };
                        string[] Weapon = { "파이프", "밧줄", "단검", "렌치", "권총", "촛대" };
                        string[] Place = { "침실", "욕실", "마당", "식당", "차고", "게임룸", "서재", "거실", "부엌" };

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
