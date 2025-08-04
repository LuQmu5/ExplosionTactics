using UnityEngine;

public class ClickMarkerController
{
    private ClickPointMarkerView _marker;

    public ClickMarkerController(ClickPointMarkerView markerPrefab)
    {
        _marker = GameObject.Instantiate(markerPrefab);
        _marker.gameObject.SetActive(false);
    }

    public void SetMarkerToPosition(Vector3 pos)
    {
        _marker.transform.position = pos;
        _marker.gameObject.SetActive(true);
    }

    public void CheckMarkerForDeactivate(Vector3 playerPos)
    {
        if (_marker.gameObject.activeSelf &&
            Vector3.Distance(_marker.transform.position, playerPos) < 0.2f)
        {
            _marker.gameObject.SetActive(false);
        }
    }

    public void Deactivate() => _marker.gameObject.SetActive(false);
}
