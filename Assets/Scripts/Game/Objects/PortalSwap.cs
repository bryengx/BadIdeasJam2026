using System.Collections.Generic;
using UnityEngine;

public class PortalSwap : MonoBehaviour, IInteractable
{
    [System.Serializable]
    public class PortalPair
    {
        public Portal source;
        public Portal destination;
    }
    public List<PortalPair> portals;
    int idx;
    private void Start()
    {
        idx = 0;
        PortalPair pair = portals[idx];
        pair.source.tartgetPortal = pair.destination;
    }
    public void Interact(PlayerController2D player)
    {
        idx++;
        idx %= portals.Count;

        PortalPair pair = portals[idx];
        pair.source.tartgetPortal = pair.destination;
    }
}
