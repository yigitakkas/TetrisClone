using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{
    public Sprite IconTrue;
    public Sprite IconFalse;
    public bool DefaultIconState = true;
    private Image _image;
    void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = DefaultIconState ? IconTrue : IconFalse;
    }

    public void ToggleIcon (bool state)
    {
        if(!_image || !IconTrue || !IconFalse)
        {
            Debug.LogWarning("IconTrue or IconFalse missing");
            return;
        }
        _image.sprite = (state) ? IconTrue : IconFalse;
    }
}
