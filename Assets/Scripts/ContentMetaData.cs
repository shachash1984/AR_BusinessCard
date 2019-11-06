using UnityEngine;
using System;

[System.Serializable]
public enum ContentType { Youtube = 0, Web, Picture, Text, Sketch}

[System.Serializable]
public class ContentMetaData
{
    public string _id;
    public string contentAsString;
    public string contentHeader;
    public ContentType contentType;
    public Vector3 contentPosition;
    public Vector3 contentScale;

    public ContentMetaData(string content, string header, ContentType conType, Vector3 pos, Vector3 scale)
    {
        this.contentAsString = content;
        this.contentHeader = header;
        this.contentType = conType;
        this.contentPosition = pos;
        this.contentScale = scale;
        this._id = GenerateID();
    }

    public string GetID()
    {
        return _id;
    }

    public string GenerateID()
    {
        return Guid.NewGuid().ToString("N");
    }

    public string GetContentMetaDataAsJSON()
    {
        return JsonUtility.ToJson(this);
    }


}
