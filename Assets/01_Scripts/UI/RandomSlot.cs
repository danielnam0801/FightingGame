using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class RandomSlot : Slot
{
    public RandomSlot(SelectUI selectUI, VisualElement element, StyleBackground image, int idx) : base(selectUI, element, image, idx)
    {
    }

    public void SetRandom()
    {
        DataManager<Data>.Instance.ChangeDataToJson(data);
        DataManager<Data>.Instance.LoadData("s");

    }
}
