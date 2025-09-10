using System.Collections.Generic;
using UnityEngine;

public class LeverManager : MonoBehaviour
{
    public static LeverManager Instance;

    private List<InteractLever> allLevers = new List<InteractLever>();
    private InteractLever activeLever = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterLever(InteractLever lever)
    {
        if (!allLevers.Contains(lever))
            allLevers.Add(lever);
    }

    public void SetActiveLever(InteractLever lever)
    {
        foreach (var l in allLevers)
        {
            if (l != lever)
                l.Deactivate();
        }

        lever.Activate();
        activeLever = lever;
    }

    public void ClearActiveLever(InteractLever lever)
    {
        if (activeLever == lever)
        {
            lever.Deactivate();
            activeLever = null;
        }
    }
}
