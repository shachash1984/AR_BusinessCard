using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class YoutubeContentHandler : ContentHandlerBase
{
    [SerializeField] private SimplePlayback _simplePlayback;

    public override void Initialize(ContentMetaData cmd)
    {
        Debug.Log("<color=yellow>YoutubeContentHandler Initialize</color>");
        if (!_simplePlayback)
            _simplePlayback = GetComponent<SimplePlayback>();
        _simplePlayback.unityVideoPlayer.SetTargetAudioSource(0, Camera.main.GetComponent<AudioSource>());
        _simplePlayback.videoId = cmd.contentAsString;
        _simplePlayback.unityVideoPlayer.url = _simplePlayback.videoId;
        transform.localPosition = cmd.contentPosition;
        transform.localScale = Vector3.zero;
        transform.DOScale(cmd.contentScale, 1.5f).OnStart(() => PlayContent());
        //play pop sound
    }

    public override void PlayContent()
    {
        _simplePlayback.PlayYoutubeVideo(_simplePlayback.videoId);
    }

    public override void StopContent()
    {
        _simplePlayback.unityVideoPlayer.Stop();
        transform.DOScale(Vector3.zero, 1.5f).OnComplete(() => Destroy(gameObject));
        //play pop sound
    }
}
