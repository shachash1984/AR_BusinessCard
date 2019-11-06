using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System;

public class CloudHandler : MonoBehaviour, ICloudRecoEventHandler
{
    private CloudRecoBehaviour mCloudRecoBehaviour;
    public ImageTargetBehaviour ImageTargetTemplate;
    //private bool mIsScanning = false;
    //private string mTargetMetaData = "";
    public static event Action<TargetFinder.TargetSearchResult> OnCloudTargetFound;
    

    private void Start()
    {
        //registering this event handler at the cloud reco behaviour
        mCloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        if (mCloudRecoBehaviour)
            mCloudRecoBehaviour.RegisterEventHandler(this);
    }

    /*private void OnGUI()
    {
        //Display current scanning status
        GUI.Box(new Rect(20, 100, 200, 50), mIsScanning ? "Scanning" : "Not scanning");

        //Display metadata of latest detected cloud-target
        GUI.Box(new Rect(20, 200, 200, 50), "Metadata: " + mTargetMetaData);

        //If not scanning , show button so that user can restart cloud scanning
        if (!mIsScanning)
        {
            if (GUI.Button(new Rect(100, 300, 200, 50), "Restart Scanning"))
            {
                //restart TargetFinder
                mCloudRecoBehaviour.CloudRecoEnabled = true;
            }
        }
    }*/

    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco init error " + initError.ToString());
    }

    public void OnInitialized()
    {
        Debug.Log("Cloud Reco initialized");
    }

    //handling a cloud target recogintion event
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        //Build augmentation based on target
        if (ImageTargetTemplate)
        {
            //enable the new result with the same ImageTargetBehaviour
            ObjectTracker tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            ImageTargetBehaviour imageTargetBehaviour = (ImageTargetBehaviour)tracker.GetTargetFinder<TargetFinder>().EnableTracking(targetSearchResult, ImageTargetTemplate.gameObject);
            Debug.Log(imageTargetBehaviour);
        }
        //do something with the target metadata
        mCloudRecoBehaviour.CloudRecoEnabled = false;
        Debug.Log("New search result " + targetSearchResult.UniqueTargetId);
        if (OnCloudTargetFound != null)
            OnCloudTargetFound(targetSearchResult);
    }

    public void OnStateChanged(bool scanning)
    {
        //Debug.Log("Cloud Reco State Changed scanning: " + scanning);
        //mIsScanning = scanning;
        if (scanning)
        {
            //clear all known trackables
            var tracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
            tracker.GetTargetFinder<TargetFinder>().ClearTrackables(false);
        }
    }

    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error " + updateError.ToString());
    }

    public void OnInitialized(TargetFinder targetFinder)
    {
        //Debug.Log("Cloud Reco initialized ");
    }
}

