using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CustomUI
{
    public class CustomElement<T>
    {
        public T _element { get; private set; }
        public Action CallBackEvt;
        public CustomElement(T element)
        {
            _element = element;
        }
    }
}
