using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameInstaller _gameInstaller;
    [SerializeField] private GameObject _restartBackground;
    [SerializeField] private Image _restartFill;
    [SerializeField] private float _restartDuration = 2f;

    private float _timer;
    private bool _isRestarting;

    private void OnEnable()
    {
        _gameInstaller.Controls.Game.Restart.started += OnRestartStarted;
        _gameInstaller.Controls.Game.Restart.canceled += OnRestartCanceled;
    }

    private void OnDisable()
    {
        _gameInstaller.Controls.Game.Restart.started -= OnRestartStarted;
        _gameInstaller.Controls.Game.Restart.canceled -= OnRestartCanceled;
    }

    private void Update()
    {
        if (!_isRestarting)
            return;

        _timer += Time.deltaTime;
        _restartFill.fillAmount = _timer / _restartDuration;

        if (_timer >= _restartDuration)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnRestartStarted(
        UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isRestarting = true;
        _timer = 0f;
        _restartFill.fillAmount = 0f;
        _restartBackground.SetActive(true);
    }

    private void OnRestartCanceled(
        UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        _isRestarting = false;
        _timer = 0f;
        _restartBackground.SetActive(false);
    }
}