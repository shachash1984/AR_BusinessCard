using System.Collections;
using UnityEngine;
using Vuforia;
using System;
using DG.Tweening;
using System.Text;

public enum Mode { Scanning, Capturing}

[System.Serializable]
public struct UserMessage
{
    public string header;
    public string content;
}

public class AppManager : MonoBehaviour {

    static public AppManager S;
    private ContentHandlerBase _contentHandler;
    [SerializeField] private ContentHandlerBase _youtubeHandlerPrefab;
    [SerializeField] private ContentHandlerBase _webHandlerPrefab;
    [SerializeField] private ContentHandlerBase _pictureHandlerPrefab;
    [SerializeField] private ContentHandlerBase _textHandlerPrefab;
    [SerializeField] private ContentHandlerBase _sketchHandlerPrefab;
    [SerializeField] private CloudRecoBehaviour _cloudReco;
    private string _uploadURL = "http://popar.live/upload";
    public string url { get { return _uploadURL; } }
    public CloudHandler cloudHandler { get; private set; }
    public UIHandler ui { get; private set; }
    public ServerHandler server { get; private set; }
    public CaptureHandler capture { get; private set; }
    public EaseHandler ease { get; private set; }
    public StringBuilder targetAsString { get; set; } 
    public Mode mode = Mode.Scanning;
    public static event Action<Mode, Mode> OnModeChanged;

    void Awake()
    {
        if (S != null)
            Destroy(gameObject);
        S = this;
    }

    private void Start()
    {
        Init();
    }

    private void OnEnable()
    {
        ServerHandler.OnUploadUpdate += HandleUploadStatus;
        CloudHandler.OnCloudTargetFound += HandleNewFoundTarget;
        TrackableEventHandler.OnTargetLost += HandleTargetLost;
        ContentUploaderBase.OnUploadClicked += HandleUploadClicked;
    }

    private void OnDisable()
    {
        ServerHandler.OnUploadUpdate -= HandleUploadStatus;
        CloudHandler.OnCloudTargetFound -= HandleNewFoundTarget;
        TrackableEventHandler.OnTargetLost -= HandleTargetLost;
        ContentUploaderBase.OnUploadClicked -= HandleUploadClicked;
    }

#if UNITY_ANDROID
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
#endif

    private void Init()
    {
        ui = UIHandler.S;
        server = ServerHandler.S;
        capture = CaptureHandler.S;
        ease = EaseHandler.S;
        cloudHandler = FindObjectOfType<CloudHandler>();
    }

    public void HandleUploadStatus(UploadStatus us)
    {
        switch (us)
        {
            case UploadStatus.Success:
                ClearTargetString();
                break;
            case UploadStatus.Fail:
                //handle error
                break;
            case UploadStatus.Pending:
                //visualize to user
                break;
            case UploadStatus.None:
                break;
            default:
                break;
        }
    }

    public void HandleUploadClicked(ContentMetaData cm)
    {
        server.Upload(cm);
        capture.DismissCapturedImage();
        ui.DisplayMessage("Success");
        InitScanMode();
        Debug.Log("<color=green>Content and target sent</color>");
    }

    public void Stop()
    {
        _contentHandler.StopContent();
    }

    private void ClearTargetString()
    {
        targetAsString.Remove(0, targetAsString.Length);
        targetAsString = null;
    }

    public void HandleNewFoundTarget(TargetFinder.TargetSearchResult result)
    {
        ContentMetaData cmd = JsonUtility.FromJson<ContentMetaData>(result.MetaData);
        switch (cmd.contentType)
        {
            case ContentType.Youtube:
                _contentHandler = Instantiate(_youtubeHandlerPrefab, cloudHandler.ImageTargetTemplate.transform).GetComponent<YoutubeContentHandler>();
                break;
            case ContentType.Web:
                break;
            case ContentType.Picture:
                break;
            case ContentType.Text:
                break;
            case ContentType.Sketch:
                break;
            default:
                break;
        }
        _contentHandler.Initialize(cmd);
       
        ease.StartContentTrackingEase(cloudHandler, _contentHandler.transform, cmd.contentPosition);
    }

    public void HandleTargetLost()
    {
        if (VuforiaManager.Instance.Initialized)
        {
            _cloudReco.CloudRecoEnabled = true;
            Stop();
            ease.EndContentTrackingEase();
        }
    }

    public void SetMode(Mode m)
    {
        Mode prevMode = mode;
        mode = m;
        if (OnModeChanged != null)
            OnModeChanged(prevMode, mode);
    }

    public void InitScanMode()
    {
        Debug.Log("<color=black>InitScanMode</color>");
        _cloudReco.CloudRecoEnabled = true;
        SetMode(Mode.Scanning);
    }

    public void InitCaptureMode()
    {
        Debug.Log("<color=black>InitCaptureMode</color>");
        _cloudReco.CloudRecoEnabled = false;
        SetMode(Mode.Capturing);
    }

}

