using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class UIHandler : MonoBehaviour {

    static public UIHandler S;
    private Button _addContentButton;
    private Button _backToScanButton;
    //private Image capturedImage;
    private Button captureButton;
    private CanvasGroup _contentButtonsPanel;
    private CanvasGroup _userMessagePanel;
    private Image captureImageFrame;
    private CanvasGroup _youtubePanel;
    private CanvasGroup _textPanel;
    private Text _messageHeader;
    private Text _messageContent;
    private Button _messageDismissButton;
    [SerializeField] private UserMessage[] _userMessages;
    private Dictionary<string, string> _userMessageMap = new Dictionary<string, string>();


    private void Awake()
    {
        if (S != null)
            Destroy(this);
        S = this;
        
    }

    private void OnEnable()
    {
        AppManager.OnModeChanged += HandleModeChange;
        CaptureHandler.OnCapture += FadeFrame;
    }

    private void OnDisable()
    {
        AppManager.OnModeChanged -= HandleModeChange;
        CaptureHandler.OnCapture -= FadeFrame;
    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        AssignButton(ref _addContentButton, 4, AppManager.S.InitCaptureMode);
        AssignButton(ref _backToScanButton, 5, AppManager.S.InitScanMode);
        AssignButton(ref captureButton, 0, CaptureHandler.S.TakeSnapShot);
        _contentButtonsPanel = transform.GetChild(3).GetComponent<CanvasGroup>();
        _userMessagePanel = transform.GetChild(6).GetComponent<CanvasGroup>();
        _messageHeader = _userMessagePanel.transform.GetChild(0).GetComponent<Text>();
        _messageContent = _userMessagePanel.transform.GetChild(1).GetComponent<Text>();
        _messageDismissButton = _userMessagePanel.transform.GetChild(2).GetComponent<Button>();
        _messageDismissButton.onClick.RemoveAllListeners();
        _messageDismissButton.onClick.AddListener(() => DismissMessage());
        captureImageFrame = transform.GetChild(1).GetComponent<Image>();
        //capturedImage = transform.GetChild(2).GetComponent<Image>();
        _youtubePanel = transform.GetChild(7).GetComponent<CanvasGroup>();
        _textPanel = transform.GetChild(8).GetComponent<CanvasGroup>();

        ToggleElement(_addContentButton, true, true);
        ToggleElement(_backToScanButton, false, true);
        ToggleElement(captureButton, false, true);
        ToggleElement(_contentButtonsPanel, false, true);
        ToggleElement(captureImageFrame, false, true);
        ToggleElement(_userMessagePanel, false, true);
        ToggleElement(_youtubePanel, false, true);
        ToggleElement(_textPanel, false, true);
        ToggleElement(_userMessagePanel, false, true);

        foreach (UserMessage um in _userMessages)
        {
            if (!_userMessageMap.ContainsKey(um.header))
                _userMessageMap.Add(um.header, um.content);
        }
    }

    private void DismissMessage()
    {
        ToggleElement(_userMessagePanel, false);
    }

    public void DisplayMessage(string header)
    {
        _messageHeader.text = header;
        _messageContent.text = _userMessageMap[header];
        ToggleElement(_userMessagePanel, true);
    }

    public void FadeFrame()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(captureImageFrame.DOFade(0, 0f));
        seq.Append(captureImageFrame.DOFade(0, 0.5f));
        seq.Append(captureImageFrame.DOFade(1f, 1f));
        seq.Play();
    }

    public void HandleModeChange(Mode prev, Mode current)
    {
        switch (current)
        {
            case Mode.Scanning:
                InitScanMode();
                break;
            case Mode.Capturing:
                InitCaptureMode();
                break;
            default:
                break;
        }
    }

    public void InitScanMode()
    {
        ToggleElement(captureImageFrame, false);
        ToggleElement(captureButton, false);
        ToggleElement(_contentButtonsPanel, false);
        ToggleElement(_backToScanButton, false);
        ToggleElement(_addContentButton, true);
    }

    public void InitCaptureMode()
    {
        ToggleElement(captureImageFrame, true);
        ToggleElement(captureButton, true);
        ToggleElement(_addContentButton, false);
        ToggleElement(_backToScanButton, true);
    }

    public void ToggleContentButtonsPanel(bool on)
    {
        ToggleElement(_contentButtonsPanel, on);
    }

    public void InitContentUploadPanel(CanvasGroup panel)
    {
        ToggleElement(panel, true);
        panel.GetComponent<ContentUploaderBase>().Initialize();
    }

    public void ToggleElement(Component s, bool on, bool immediate = false)
    {
        ToggleUIElement(s, on, immediate);
    }

    private void ToggleUIElement(Component s, bool on, bool immediate = false)
    {
        CanvasGroup cv;
        if (s is CanvasGroup)
            cv = s as CanvasGroup;
        else
            cv = s.GetComponent<CanvasGroup>();

        if (!cv)
            cv = s.gameObject.AddComponent<CanvasGroup>();
        if (immediate)
        {
            if (on)
            {
                cv.alpha = 1f;
                cv.gameObject.SetActive(true);
            }
            else
            {
                cv.alpha = 0f;
                cv.gameObject.SetActive(false);
            }
        }
        else
        {
            if (on)
            {
                cv.DOFade(1f, 0.25f).OnStart(() =>
                {
                    cv.gameObject.SetActive(true);
                }).OnComplete(() =>
                {
                    cv.interactable = true;
                });
            }
            else
            {
                cv.DOFade(0f, 0.25f).OnComplete(() =>
                {
                    cv.gameObject.SetActive(false);
                    cv.interactable = false;
                });
            }
        }
    }

    private void AssignButton(ref Button b, int transformIndex, Action action)
    {
        b = transform.GetChild(transformIndex).GetComponent<Button>();
        b.onClick.RemoveAllListeners();
        b.onClick.AddListener(() => { action(); });
    }
}
