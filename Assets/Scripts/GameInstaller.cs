using UnityEngine;
using UnityEngine.InputSystem;

public class GameInstaller : MonoBehaviour
{
    public Controls Controls { get; private set; }

    private void Awake()
    {
        Controls = new Controls();
        Controls.Enable();

        Debug.Log("GameInstaller: Controls создан");
    }

    private void OnDestroy()
    {
        if (Controls != null)
        {
            Controls.Game.Disable();
            Controls.Dispose();
            Controls = null;
        }

        Debug.Log("GameInstaller: Controls уничтожен");
    }
}
