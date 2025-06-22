using UnityEngine;

public class ClickPointMarkerController
{
    private const float MinDistanceToDeactivate = 0.5f;

    private readonly ClickPointMarkerView _marker;

    public ClickPointMarkerController(ClickPointMarkerView markerPrefab)
    {
        _marker = Object.Instantiate(markerPrefab);
        _marker.gameObject.SetActive(false);
    }

    public void CheckMarkerForDeactivate(Vector3 playerPosition)
    {
        if (Vector3.Distance(playerPosition, _marker.transform.position) <= MinDistanceToDeactivate)
        {
            _marker.gameObject.SetActive(false);
        }
    }

    public void SetMarkerToPosition(Vector3 at)
    {
        _marker.gameObject.SetActive(true);
        _marker.transform.position = at;
    }

    public void Deactivate()
    {
        _marker.gameObject.SetActive(false);
    }
}