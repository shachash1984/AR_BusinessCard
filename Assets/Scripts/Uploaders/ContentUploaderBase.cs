using UnityEngine;
using UnityEngine.UI;
using System;



public abstract class ContentUploaderBase : MonoBehaviour {

    public abstract void Initialize();
    protected abstract bool SaveContent();
    public abstract ContentMetaData GetContentMetaData();
    public abstract void HandleModeChange(Mode prev, Mode current);
    protected Button _addContentButton;
    protected ContentMetaData _contentMetaData;
    [SerializeField] protected ContentPlaceHolder _contentPlaceHolder;
    
    public static event Action<ContentMetaData> OnUploadClicked;
    public static void TriggerOnUploadClicked(ContentMetaData cm)
    {
        if (OnUploadClicked != null)
            OnUploadClicked(cm);
    }
    protected Vector3 GetAdjustedContentPosition(Vector3 currentPos)
    {
        return new Vector3(currentPos.x, 0f, currentPos.y*0.75f);
    }
    protected Vector3 GetAdjustedContentScale(Vector3 currentScale)
    {
        return new Vector3(currentScale.x * -0.1f, currentScale.z, currentScale.y * -0.05f);
    }

}
