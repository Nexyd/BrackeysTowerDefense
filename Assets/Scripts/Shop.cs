using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    private BuildManager manager;

    private void Start()
    {
        manager = BuildManager.instance;
    }

    public void SelectStandardTurret()
    {
        Debug.Log("Standard Turret Selected");
        manager.SelectTurretToBuild(standardTurret);
    }

    public void SelectMissileLauncher()
    {
        Debug.Log("Missile Launcher Selected");
        manager.SelectTurretToBuild(missileLauncher);
    }
}