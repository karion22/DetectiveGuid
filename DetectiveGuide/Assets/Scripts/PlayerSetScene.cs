using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSetScene : MonoBehaviour
{
    private UIDocument _document = null;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        VisualElement root = _document.rootVisualElement;

    }
}
