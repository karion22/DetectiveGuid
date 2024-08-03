using System;
using System.Collections.Generic;

public class CardItem_UIToolkit
{
    public string Id = Guid.NewGuid().ToString().ToUpper();
    public string UserName;
    public string CharacterName;
    public List<string> Items = new List<string>();
}
