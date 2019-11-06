using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class TextContentHandler : ContentHandlerBase {

    private TextMeshPro _textMeshPro;

    public override void Initialize(ContentMetaData cmd)
    {
        Debug.Log("<color=yellow>TextContentHandler Initialize</color>");
        if (!_textMeshPro)
            _textMeshPro = GetComponent<TextMeshPro>();
        transform.localPosition = cmd.contentPosition;
        transform.localScale = Vector3.zero;
        _textMeshPro.text = _contentMetaData.contentAsString;
        transform.DOScale(cmd.contentScale, 1.5f).OnComplete(() => PlayContent());
        //play pop sound
    }

    public override void PlayContent()
    {
        return;
    }

    public override void StopContent()
    {
        transform.DOScale(Vector3.zero, 1.5f).OnComplete(() => Destroy(gameObject));
    }

    
}
