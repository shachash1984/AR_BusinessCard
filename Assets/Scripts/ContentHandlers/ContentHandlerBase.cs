using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ContentHandlerBase : MonoBehaviour {

    protected ContentMetaData _contentMetaData;
    public abstract void Initialize(ContentMetaData cmd);
    public abstract void PlayContent();
    public abstract void StopContent();
}
