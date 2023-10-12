using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    public void ActiveVisual(VisualElement element, bool value)
    {
        if (value == true)
        {
            element.AddToClassList("on");
        }
        else
        {
            element.RemoveFromClassList("on");
        }

    }

    public void ActiveGameObject(GameObject obj, bool value)
    {
        obj.SetActive(value);
    }
}
