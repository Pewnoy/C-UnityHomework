using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour,
    IPointerEnterHandler,
    IPointerClickHandler,
    IPointerExitHandler
{
    public Cell Cell { get; set; }

    public event Action OnMoveEndCallback;

    [SerializeField] private float _moveSpeed = 5f;

    private Cell _targetCell;
    private bool _isMoving;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Юнит: PointerEnter");
        Cell?.OnPointerEnter(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Юнит: PointerClick");
        Cell?.OnPointerClick(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Юнит: PointerExit");
        Cell?.OnPointerExit(eventData);
    }

    private void Update()
    {
        if (!_isMoving || _targetCell == null)
            return;

        Vector3 targetPosition =
            _targetCell.transform.position + Vector3.up * 0.75f;

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            _moveSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            _isMoving = false;

            if (Cell != null)
                Cell.Unit = null;

            Cell = _targetCell;
            _targetCell.Unit = this;

            OnMoveEndCallback?.Invoke();
        }
    }

    public void Move(Cell cell)
    {
        if (cell == null)
            return;

        _targetCell = cell;
        _isMoving = true;
    }
}
