using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSet", menuName = "Scriptable Object Asset/GameDataSet")]
public class GameDataSetScriptable : ScriptableObject
{
    public eGameMode GameMode;

    public int DefaultUserCount;
    public int DefaultItemCount;

    public string[] Names;
    public string[] Places;
    public string[] Weapons;
}   
