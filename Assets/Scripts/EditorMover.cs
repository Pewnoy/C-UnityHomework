
using UnityEngine;

namespace DefaultNamespace
{
    [RequireComponent(typeof(PositionSaver))]
    public class EditorMover : MonoBehaviour
    {
        private PositionSaver _save;
        private float _currentDelay;

        //todo comment: Что произойдёт, если _delay > _duration?
        //за время работы компонента не успеет пройти даже один полный интервал записи позиции.в зависимости от начального значения _currentDelay может быть записана одна точка или ни одной

        [SerializeField]
        [Range(0.2f, 1f)]
        private float _delay = 0.2f;

        [SerializeField]
        [Min(0.2f)]
        private float _duration = 5f;

        private void Start()
        {
            Debug.Log("EditorMover Start запущен");

            if (_duration <= _delay)
            {
                _duration = _delay * 5f;
            }

            Debug.Log($"Duration после проверки: {_duration}");
            //todo comment: Почему этот поиск производится здесь,а не в начале метода Update?
            //потому что Start вызывается один раз перед первым обновлением компонента. полученную ссылку можно сохранить в _save и использовать в каждом последующем вызове Update не выполняя повторный поиск

            _save = GetComponent<PositionSaver>();
            _save.Records.Clear();
        }

        private void Update()
        {
            _duration -= Time.deltaTime;
            if (_duration <= 0f)
            {
                enabled = false;
                Debug.Log($"<b>{name}</b> finished", this);
                return;
            }
            //todo comment: Почему не написать (_delay -= Time.deltaTime;) по аналогии с полем _duration?
            //Потому что _delay хранит постоянный интервал между записями, а _currentDelay — оставшееся время до следующей записи; если изменять сам _delay интервал будет постоянно уменьшаться и позиции начнут записываться с неправильной частотой
            _currentDelay -= Time.deltaTime;
            if (_currentDelay <= 0f)
            {
                _currentDelay = _delay;
                _save.Records.Add(new PositionSaver.Data
                {
                    Position = transform.position,
                    //todo comment: Для чего сохраняется значение игрового времени?
                    //чтобы знать в какой момент была записана конкретная позиция; позже ReplayMover использует временные метки для определения скорости воспроизведения и интерполяции между точками
                    Time = Time.time,
                });
            }
        }
    }
}