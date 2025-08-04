using UnityEngine.AI;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class PlayerJumpHandler
{
    private Transform _transform;
    private NavMeshAgent _agent;
    private PlayerView _view;
    private float _jumpUpOffset;
    private float _jumpDownPeak;
    private Coroutine _jumpCoroutine;

    public PlayerJumpHandler(Transform transform, NavMeshAgent agent, PlayerView view, float up, float down)
    {
        _transform = transform;
        _agent = agent;
        _view = view;
        _jumpUpOffset = up;
        _jumpDownPeak = down;
    }

    public void CheckAndTryJump(Vector3 fallbackTarget)
    {
        if (_jumpCoroutine == null && _agent.enabled && _agent.isOnOffMeshLink)
        {
            _jumpCoroutine = _view.StartCoroutine(Jump(fallbackTarget));
        }
    }

    private IEnumerator Jump(Vector3 moveAfter)
    {
        _view.SetJumpingState(true);

        OffMeshLinkData link = _agent.currentOffMeshLinkData;
        Vector3 startPos = _transform.position;
        Vector3 endPos = link.endPos;

        _agent.enabled = false;

        float jumpDuration = _view.GetAnimationClipLength("Jump") * 0.5f;
        float deltaY = endPos.y - startPos.y;
        float jumpPower = deltaY > 0 ? deltaY + _jumpUpOffset : _jumpDownPeak;

        yield return _transform.DOJump(endPos, jumpPower, 1, jumpDuration).WaitForCompletion();

        _transform.position = endPos;
        _agent.enabled = true;
        _agent.CompleteOffMeshLink();
        _jumpCoroutine = null;

        _view.SetJumpingState(false);
    }
}
