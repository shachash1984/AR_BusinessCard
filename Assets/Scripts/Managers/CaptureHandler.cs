using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using DG.Tweening;

public class CaptureHandler : MonoBehaviour {

    static public CaptureHandler S;
    [SerializeField] private Transform _capturedImage;
    [SerializeField] private Vector3 _capturedImageSize;
    public static event Action OnCapture;



    private void Awake()
    {
        if (S != null)
            Destroy(this);
        S = this;
    }

    private void OnEnable()
    {
        AppManager.OnModeChanged += HandleModeChange;
    }

    private void OnDisable()
    {
        AppManager.OnModeChanged -= HandleModeChange;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        _capturedImage.gameObject.SetActive(false);
        _capturedImage.localScale = Vector3.zero;
    }

    private void ToggleCapturedImage(bool on, Texture2D tex)
    {
        if(on)
        {
            _capturedImage.gameObject.SetActive(true);
            _capturedImage.GetComponent<Renderer>().material.mainTexture = tex;
            _capturedImage.DOScale(_capturedImageSize, 1f).SetEase(Ease.InOutSine);
        }
        else
        {
            _capturedImage.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutSine);
            _capturedImage.gameObject.SetActive(false);
            _capturedImage.GetComponent<Renderer>().material.mainTexture = null;
            Destroy(tex);
        }
    }

    public void TakeSnapShot()
    {
        StartCoroutine(TakeSnapShotCoroutine());
    }

    private IEnumerator TakeSnapShotCoroutine()
    {
        Debug.Log("TakingSnapshot");
        if (OnCapture != null)
            OnCapture();
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();
        // Create a texture the size of the screen, RGB24 format

        var tex = new Texture2D(Screen.width * 8 / 10, Screen.height * 25 / 100, TextureFormat.RGB24, true);
        // Read screen contents into the texture
        Rect r = new Rect(Screen.width * 0.1f, Screen.height * 0.375f, Screen.width * 0.9f, Screen.height * 0.4f);
        
        tex.ReadPixels(r, 0, 0, true);

        tex.Apply();

        byte[] bytes = tex.EncodeToJPG();
        if (AppManager.S.targetAsString == null)
            AppManager.S.targetAsString = new StringBuilder();
        AppManager.S.targetAsString.Append(Convert.ToBase64String(bytes));
        ToggleCapturedImage(true, tex);
        //Destroy(tex);
        AppManager.S.ui.ToggleContentButtonsPanel(true);
    }

    public void DismissCapturedImage()
    {
        ToggleCapturedImage(false, (Texture2D)_capturedImage.GetComponent<Renderer>().material.mainTexture);
    }

    public void HandleModeChange(Mode prev, Mode current)
    {
        switch (current)
        {
            case Mode.Scanning:
                DismissCapturedImage();
                break;
            case Mode.Capturing:
                break;
            default:
                break;
        }
    }
}
