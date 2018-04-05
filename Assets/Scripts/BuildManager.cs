using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public GameObject standardTurretPrefab;
    public GameObject missileLauncherPrefab;
    public GameObject buildEffect;

    private UIData data;
    private TurretBlueprint turretToBuild;

    private void Start()
    {
        data = UIData.GetInstance();
    }

    private void Awake()
    {
        if (instance != null)
            Debug.LogError("More than one BuildManager");
        else
            instance = this;
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }

    public void BuildTurretOn(Node node)
    {
        if (PlayerHasMoney) {

            GameObject turret = Instantiate(turretToBuild.prefab,
                node.GetBuildPosition(), Quaternion.identity);

            node.SetTurret(turret);
            GameObject effect = Instantiate(buildEffect, 
                node.GetBuildPosition(), Quaternion.identity);

            Destroy(effect, 5f);
            PlayerStats.Money -= turretToBuild.cost;

        } else
            data.GameLoggerText = "Not enough money!";
    }

    public bool CanBuild
    {
        get { return turretToBuild != null; }
    }

    public bool PlayerHasMoney
    {
        get { return PlayerStats.Money >= turretToBuild.cost; }
    }
}