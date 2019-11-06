using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using System;

public class TrackableEventHandler : DefaultTrackableEventHandler {

    public static event Action OnTargetFound;
    public static event Action OnTargetLost;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        if (OnTargetFound != null)
            OnTargetFound();
    }

    protected override void OnTrackingLost()
    {
        base.OnTrackingLost();
        if (OnTargetLost != null)
            OnTargetLost();
    }
}
