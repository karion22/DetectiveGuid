using System.Collections.Generic;
using UnityEngine;

public class eImage : ePanel
{
    public List<Sprite> m_States;

    public Color Color
    {
        get
        {
            if (Image != null)
                return Image.color;
            return Color.white;
        }

        set
        {
            if (Image != null)
                Image.color = value;
        }
    }

    protected override void Awake()
    {
        // Don't Execute Parent
    }

    public void ChangeState(string inState)
    {
        if (Image != null)
        {
            Sprite sprite = null;
            for (int i = 0, end = m_States.Count; i < end; i++)
            {
                sprite = m_States[i];
                if (sprite == null)
                {
                    DebugLog.Warning(this, "Sprite Value is Null");
                    continue;
                }

                if (sprite.name == inState)
                {
                    Image.overrideSprite = sprite;
                    return;
                }
            }
        }
    }

    public void ChangeState(int inIndex)
    {
        if (Image != null)
        {
            Sprite sprite = m_States[inIndex] ?? null;
            if (sprite != null)
                Image.overrideSprite = sprite;
            else
                DebugLog.Warning(this, Utility.BuildString("{0} State is Null : {1}", inIndex, sprite.name));
        }
    }
}