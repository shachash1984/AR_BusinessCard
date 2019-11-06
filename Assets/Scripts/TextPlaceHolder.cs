using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPlaceHolder : ContentPlaceHolder {

    public TextMeshProUGUI placeHolderText;

    private void OnEnable()
    {
        Init();
    }

    private void Update()
    {
        HandleTouch();
    }

    protected override void Init()
    {
        base.Init();
        placeHolderText = GetComponent<TextMeshProUGUI>();
    }

    protected override void HandleTouch()
    {
        base.HandleTouch();
    }

    public void SetTextColor(Color c)
    {
        placeHolderText.color = c;
    }
}
