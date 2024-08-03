using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIUtility_UIToolkit
{
    public static void SetPopup(VisualElement inPopup)
    {
        inPopup.style.position = Position.Absolute;
        inPopup.style.top = 0;
        inPopup.style.left = 0;

        inPopup.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
        inPopup.style.height = new StyleLength(new Length(100, LengthUnit.Percent));
        inPopup.style.opacity = 0.4f;
        inPopup.style.backgroundColor = new StyleColor(Color.black);
    }
}
