using System;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    public event Action<Cell> OnCellClicked;

    private Cell[] _cells;
    private Unit[] _units;

    private void Start()
    {
        _cells = FindObjectsOfType<Cell>();
        _units = FindObjectsOfType<Unit>();

        // Привязываем юниты к начальным клеткам
        foreach (Unit unit in _units)
        {
            foreach (Cell cell in _cells)
            {
                Vector3 unitPosition = unit.transform.position;
                Vector3 cellPosition = cell.transform.position;

                if (Vector3.Distance(
                    unitPosition,
                    cellPosition + Vector3.up * 0.75f) < 0.1f)
                {
                    unit.Cell = cell;
                    cell.Unit = unit;
                    break;
                }
            }
        }

        // Подписываемся на завершение движения
        foreach (Unit unit in _units)
        {
            unit.OnMoveEndCallback += () =>
                Debug.Log($"Движение юнита {unit.name} завершено");
        }

        // Подписываемся на клики по клеткам
        foreach (Cell cell in _cells)
        {
            cell.OnPointerClickEvent += HandleCellClicked;
        }

        FindNeighbours();
    }

    private void FindNeighbours()
    {
        foreach (Cell cell in _cells)
        {
            SetNeighbour(cell, NeighbourType.Top, new Vector3(0, 0, 1));
            SetNeighbour(cell, NeighbourType.Bottom, new Vector3(0, 0, -1));
            SetNeighbour(cell, NeighbourType.Left, new Vector3(-1, 0, 0));
            SetNeighbour(cell, NeighbourType.Right, new Vector3(1, 0, 0));

            SetNeighbour(cell, NeighbourType.TopLeft, new Vector3(-1, 0, 1));
            SetNeighbour(cell, NeighbourType.TopRight, new Vector3(1, 0, 1));
            SetNeighbour(cell, NeighbourType.BottomLeft, new Vector3(-1, 0, -1));
            SetNeighbour(cell, NeighbourType.BottomRight, new Vector3(1, 0, -1));
        }
    }

    private void SetNeighbour(Cell cell, NeighbourType type, Vector3 offset)
    {
        Vector3 targetPosition = cell.transform.position + offset;

        foreach (Cell neighbour in _cells)
        {
            if (neighbour == cell)
                continue;

            if (Vector3.Distance(
                neighbour.transform.position,
                targetPosition) < 0.01f)
            {
                cell.SetNeighbour(type, neighbour);
                return;
            }
        }
    }

    private void HandleCellClicked(Cell cell)
    {
        if (_units.Length > 0)
        {
            _units[0].Move(cell);
        }

        OnCellClicked?.Invoke(cell);
    }

    private void OnDestroy()
    {
        if (_cells == null)
            return;

        foreach (Cell cell in _cells)
        {
            cell.OnPointerClickEvent -= HandleCellClicked;
        }
    }
}
