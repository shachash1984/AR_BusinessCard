using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUploader : ContentUploaderBase
{
    private InputField _textInputField;

    private void OnEnable()
    {
        AppManager.OnModeChanged += HandleModeChange;
        //_textInputField.onValueChanged +=
    }

    private void OnDisable()
    {
        AppManager.OnModeChanged -= HandleModeChange;
    }

    public override ContentMetaData GetContentMetaData()
    {
        if (_contentMetaData == null)
        {
            //pop message that no content is available
            Debug.LogError("Content Meta Data is null");
            return null;
        }
        return _contentMetaData;
    }

    public override void HandleModeChange(Mode prev, Mode current)
    {
        switch (current)
        {
            case Mode.Scanning:
                _textInputField.text = "";
                AppManager.S.ui.ToggleElement(this, false);
                break;
            case Mode.Capturing:
                break;
            default:
                break;
        }
    }

    public override void Initialize()
    {
        _contentPlaceHolder = FindObjectOfType<ContentPlaceHolder>();

        _textInputField = GetComponentInChildren<InputField>(true);
        _addContentButton = GetComponentInChildren<Button>(true);
        _addContentButton.onClick.RemoveAllListeners();
        _addContentButton.onClick.AddListener(() =>
        {
            if (SaveContent())
            {
                if (GetContentMetaData() != null)
                {
                    _textInputField.text = "";
                    TriggerOnUploadClicked(_contentMetaData);
                }
            }
            else
            {
                //pop message
                Debug.LogError("Didn't upload due to error");
            }

        });
        AppManager.S.ui.ToggleElement(this, true);
    }

    protected override bool SaveContent()
    {
        if (string.IsNullOrEmpty(_textInputField.text))
        {
            //pop message saying no link
            Debug.LogError("Text is Empty or null");
            return false;
        }
        Vector3 adjustedPosition = GetAdjustedContentPosition(_contentPlaceHolder.transform.localPosition);
        Vector3 adjustedScale = GetAdjustedContentScale(_contentPlaceHolder.transform.localScale);
        _contentMetaData = new ContentMetaData(_textInputField.text, "text", ContentType.Text, adjustedPosition, adjustedScale);
        return true;
    }
}
