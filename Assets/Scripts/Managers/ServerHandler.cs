using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;

public enum UploadStatus { Success, Fail, Pending, None}

public class ServerHandler : MonoBehaviour {

    static public ServerHandler S;
    public static event Action<UploadStatus> OnUploadUpdate;
    public UploadStatus uploadStatus { get; private set; }

    private void Awake()
    {
        if (S != null)
            Destroy(this);
        S = this;
        uploadStatus = UploadStatus.None;
    }

    public void Upload(ContentMetaData metaData)
    {
        StartCoroutine(UploadImageToServer(metaData));
    }

    private IEnumerator UploadImageToServer(ContentMetaData metaData)
    {
        //Convert metaData to JSON
        string md = JsonUtility.ToJson(metaData);

        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("target", AppManager.S.targetAsString.ToString());
        form.AddField("metaData", md);
        //form.AddField("contentID", metaData._id);

        WWW w = new WWW(AppManager.S.url, form);
        uploadStatus = UploadStatus.Pending;
        yield return w;
        
        if (string.IsNullOrEmpty(w.error))
        {

            Debug.Log("Image uploaded successfully!");
            uploadStatus = UploadStatus.Success;
            if (OnUploadUpdate != null)
                OnUploadUpdate(UploadStatus.Success);
        }
        else
        {
            Debug.LogError("Error when uploading target to server" + w.error);
            uploadStatus = UploadStatus.Fail;
            if (OnUploadUpdate != null)
                OnUploadUpdate(UploadStatus.Fail);
        }
        uploadStatus = UploadStatus.None;
    }
}
