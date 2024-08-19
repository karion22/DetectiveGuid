using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    protected virtual void Awake() { }

    protected virtual IEnumerator Start() 
    {
        GameMgr.Instance.Restart();

        while(GameMgr.Instance.m_IsReady == false)
            yield return null;

        GameMgr.Instance.SetGameMode(eGameMode.Clue);
        OnInitialize();
        yield break; 
    }

    public virtual void OnInitialize() { }
}
