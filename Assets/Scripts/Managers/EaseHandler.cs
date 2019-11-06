using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EaseHandler : MonoBehaviour {

    static public EaseHandler S;
    private Coroutine _contentMovementCoroutine;


    private void Awake()
    {
        if (S != null)
            Destroy(this);
        S = this;
    }

    public void StartContentTrackingEase(CloudHandler _cloudHandler, Transform trackingContent, Vector3 contentPos)
    {
        _contentMovementCoroutine = StartCoroutine(EaseContentTrackingCoroutine(_cloudHandler, trackingContent, contentPos));
    }

    private IEnumerator EaseContentTrackingCoroutine(CloudHandler _cloudHandler, Transform trackingContent, Vector3 contentPos)
    {
        if (!_cloudHandler)
            _cloudHandler = FindObjectOfType<CloudHandler>();
        yield return new WaitForEndOfFrame();
        while (_cloudHandler.ImageTargetTemplate)
        {
            if (!DOTween.IsTweening(trackingContent))
            {
                trackingContent.DOLocalMove(contentPos, 0.1f);
                trackingContent.DORotate(_cloudHandler.ImageTargetTemplate.transform.rotation.eulerAngles, 0.1f);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void EndContentTrackingEase()
    {
        if (_contentMovementCoroutine != null)
            StopCoroutine(_contentMovementCoroutine);
    }
}
