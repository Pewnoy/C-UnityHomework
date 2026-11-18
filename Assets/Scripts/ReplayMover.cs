using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
            ////todo comment: зачем нужны эти проверки?
            ///TryGetComponent проверяет наличие компонента PositionSaver и пытается получить ссылку на него; Records.Count == 0 проверяет, есть ли сохранённые позиции для воспроизведения
            if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
                //todo comment: Для чего выключается этот компонент?
                //чтобы остановить выполнение методов компонента в данном случае Update(), если необходимые данные отсутствуют или некорректны
                enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
            //todo comment: Что проверяет это условие (с какой целью)? 
            //проверка прошло ли достаточно времени с начала воспроизведения, чтобы перейти к следующей сохранённой позиции
            if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
                //todo comment: Для чего нужна эта проверка?
                //проверяет, не вышел ли индекс за пределы списка сохранённых позиций. Если все позиции пройдены воспроизведение завершается и компонент отключается
                if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
            //todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
            //вычисляется коэффициент интерполяции между предыдущей и текущей точками; он показывает, насколько далеко должно продвинуться перемещение объекта между двумя позициями с учётом прошедшего времени
            var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
            //todo comment: Зачем нужна эта проверка?
            //Она проверяет, не получилось ли значение "not a number"; она может произойти, например, при делении на ноль; если это произошло, коэффициент устанавливается в 0, чтобы избежать некорректного результата интерполяци
            if (float.IsNaN(delta)) delta = 0f;
            //todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
            //transform.position — позиция объекта в мировых координатах; Vector3.Lerp — метод линейной интерполяции между двумя векторами; _prev.Position — начальная позиция; curr.Position — конечная позиция; delta — коэффициент интерполяции;
            //При delta = 0 объект находится в начальной точке, при delta = 1 — в конечной. При промежуточных значениях объект располагается между ними. Из этого выходит, объект перемещается между записанными точками
            transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}