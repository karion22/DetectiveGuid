using UnityEngine.UI;

public class ePanel : eElement
{
    protected Image m_Image;

    protected Image Image
    {
        get
        {
            if (m_Image == null) m_Image = GetComponent<Image>();
            return m_Image;
        }
    }
}
