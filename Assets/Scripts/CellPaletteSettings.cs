using UnityEngine;

[CreateAssetMenu(fileName = "CellPaletteSettings",
    menuName = "Settings/Cell Palette Settings")]
public class CellPaletteSettings : ScriptableObject
{
    public Material Selected;
    public Material MoveOrAttack;
    public Material MoveAndAttack;
}