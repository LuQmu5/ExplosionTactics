using UnityEngine;

public class PlayerInputHandler
{
    private int _mouseButton = 1;
    private LayerMask _groundMask;

    public PlayerInputHandler(LayerMask groundMask)
    {
        _groundMask = groundMask;
    }

    public bool TryGetClickPoint(out Vector3 point)
    {
        point = default;

        if (Input.GetMouseButton(_mouseButton))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, _groundMask))
            {
                point = hit.point;
                return true;
            }
        }

        return false;
    }
}
