using QFramework;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class DataManager : MonoBehaviour,IController
{
    private static DataManager instance;
    public static DataManager Instance => instance;
    //读取配置文件
    private  string WeaponInfoText = "";
    private  string EnemyInfoText = "";    
    public Dictionary<int, IGunConfigModel> WeaponDic = new Dictionary<int, IGunConfigModel>();
    public Dictionary<int, IEnemyModel> EnemyDic = new Dictionary<int, IEnemyModel>();
    //保存和读取游戏数据    
    private List<Turret> turrets = new List<Turret>();
    private List<Enemy_Drone> drones = new List<Enemy_Drone>();
    private GameObject player;
    private ISaveDataModel saveData;

    private GameObject turretGo;
    private GameObject droneGo;
    private GameObject playerGo;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        ReadJsonPath();        
        ReadJsonData();
        turretGo = Resources.Load<GameObject>("Enemy/turret");
        droneGo = Resources.Load<GameObject>("Enemy/drone");
        playerGo = Resources.Load<GameObject>("Player/Player");        
    }    
    #region 读取配置文件
    private void ReadJsonPath()
    {
        string weaponText = Path.Combine(Application.streamingAssetsPath, "Weapon.json");
        if (File.Exists(weaponText))
        {
            WeaponInfoText = File.ReadAllText(weaponText);            
        }
        string enemyText = Path.Combine(Application.streamingAssetsPath, "Enemy.json");
        if (File.Exists(enemyText))
        {
            EnemyInfoText = File.ReadAllText(enemyText);            
        }
    }
    private void ReadJsonData()
    {
        WeaponDic.Clear();
        EnemyDic.Clear();
        JsonData jsonData = JsonMapper.ToObject(WeaponInfoText);
        JsonData jsonData_02 = JsonMapper.ToObject(EnemyInfoText);
        if (jsonData!=null&&jsonData.Count>0)
        {
            for (int i = 0; i < jsonData.Count; i++)
            {
                int id = int.Parse(jsonData[i]["ID"].ToString());
                string name = jsonData[i]["Name"].ToString();
                int ammunitionCount = int.Parse(jsonData[i]["NumberOfAmmunition"].ToString());
                float cd = float.Parse(jsonData[i]["CD"].ToString());
                string description = jsonData[i]["Description"].ToString();
                int damage = int.Parse(jsonData[i]["Damage"].ToString());
                int magazineCount = int.Parse(jsonData[i]["MagazineCapacity"].ToString());
                int maxMagazineCount = int.Parse(jsonData[i]["MaxMagazineCapcity"].ToString());
                GunConfigModel gunModel = new GunConfigModel(id, name, ammunitionCount, cd, description, damage, magazineCount,maxMagazineCount);
                if (gunModel.gunState == null) gunModel.gunState.Value = GunState.Idle;
                WeaponDic.Add(id, gunModel);
                //Debug.Log(WeaponDic[id].ID);
            }            
        }
        if (jsonData != null)
        {
            for (int i = 0; i < jsonData_02.Count; i++)
            {
                int id = int.Parse(jsonData_02[i]["ID"].ToString());
                string name = jsonData_02[i]["Name"].ToString();
                int hp = int.Parse(jsonData_02[i]["HP"].ToString());
                float moveSpeed = float.Parse(jsonData_02[i]["MoveSpeed"].ToString());
                string description = jsonData_02[i]["Description"].ToString();
                int damage = int.Parse(jsonData_02[i]["Damage"].ToString());
                float itemDropChance = float.Parse(jsonData_02[i]["ItemDropChance"].ToString());
                ItemDropType dropType = (ItemDropType)int.Parse(jsonData_02[i]["ItemDropType"].ToString());                
                IEnemyModel enemyModel = new EnemyModel(id, name, hp, moveSpeed, description, damage, itemDropChance, dropType);                
                EnemyDic.Add(id, enemyModel);                
            }            
        }
    }
    public IGunConfigModel GetWeaponInfoByID(int id)
    {
        return WeaponDic.ContainsKey(id) ? WeaponDic[id] : null;
    }
    public int GetWeaponAllID()
    {
        return WeaponDic.Count>0? WeaponDic.Count:0;
    }
    public IEnemyModel GetEnemyInfoByID(int id)
    {
        return EnemyDic.ContainsKey(id)? EnemyDic[id] :null;
    }
    #endregion
    #region 保存和读取游戏数据    
    public ISaveDataModel SaveGame()
    {
        saveData = this.GetModel<ISaveDataModel>();
        turrets = Object.FindObjectsOfType<Turret>().ToList();
        drones = Object.FindObjectsOfType<Enemy_Drone>().ToList();
        player = GameObject.FindGameObjectWithTag("Player");                
        if (player)
        {
            saveData.PlayerHp = player.GetComponent<Player>().playerModel.HP;
            saveData.PlayerPos = player.transform.position;
        }
        if (turrets != null)
        {
            for (int i = 0; i < turrets.Count; i++)
            {
                saveData.TurretsHp.Add(turrets[i].turrentModel.HP);
                saveData.TurretsPos.Add(turrets[i].transform.position);
            }
        }
        //Debug.Log(turrets.Count);
        if (drones!= null)
        {
            for (int i = 0; i < drones.Count; i++)
            {
                saveData.DronesHp.Add(drones[i].droneModel.HP);
                saveData.DronesPos.Add(drones[i].transform.position);
            }
        }
        var guns = this.GetSystem<IGunSystem>().GunList;
        if (guns!= null)
        {
            for (int i = 0; i < guns.Count; i++)
            {
                saveData.WeaponsID.Add(guns[i].ID);
                saveData.WeaponsName.Add(guns[i].Name);
                saveData.WeaponsBullet.Add(guns[i].BulletCount.Value);
            }
        }
        else
        {
            Debug.LogWarning("CurrentGunList is Null!!!");
        }
        Inventory.Instance.FindNotEmptySlot();
        if (InventoryManager.instance.Slots != null)
        {
            for (int i = 0; i < InventoryManager.instance.Slots.Count; i++)
            {
                saveData.InventoryItemsID.Add(InventoryManager.instance.Slots[i].itemId);
                //if (Inventory.Instance.Slots[i].transform.childCount > 0)
                InventoryManager.instance.Slots[i].currentItem = InventoryManager.instance.Slots[i].transform.GetChild(0).GetComponent<InventoryItemUI>();
                saveData.ItemsAmount.Add(InventoryManager.instance.Slots[i].currentItem.Amount);
                //Debug.Log(Inventory.Instance.Slots[i].itemId);
            }
        }
        return saveData;
    }
   // private string loadPath=Application.streamingAssetsPath+ "/ByJson.json";//TODO
    public void LoadGame()
    {
        turretGo = Resources.Load<GameObject>("Enemy/turret");
        droneGo = Resources.Load<GameObject>("Enemy/drone");
        playerGo = Resources.Load<GameObject>("Player/Player");
        if (File.Exists(GameUIManager.Instance.streamPath))
        {

            StreamReader streamReader = new StreamReader(GameUIManager.Instance.streamPath);
            string jsonsStr = streamReader.ReadToEnd();
            streamReader.Close();
            var saveData = JsonUtility.FromJson<SaveDataModel>(jsonsStr);
            var playerPos = saveData.PlayerPos;
            GameObject playerClo = Instantiate(playerGo, playerPos, Quaternion.identity);
            playerClo.SetActive(true);
            if (playerClo)
            {                
                playerClo.transform.position = playerPos;                
                Player player = playerClo.GetComponent<Player>();
                player.playerModel.HP.Value = saveData.PlayerHp;
                GameUIManager.Instance.UpdateHpSlider();
            }            
            if (turretGo)
            {
                for (int i = 0; i < saveData.TurretsHp.Count; i++)
                {                    
                    var turrtPos = saveData.turretsPos[i];
                    GameObject turretClo = Instantiate(turretGo,turrtPos,Quaternion.identity);                    
                    turretClo.transform.position = turrtPos;
                    turretClo.SetActive(true);                    
                    Turret turret = turretClo.GetComponent<Turret>();
                    turret.turrentModel = GetEnemyInfoByID(2);
                    turret.turrentModel.HP.Value = saveData.TurretsHp[i];
                }
            }
            //Debug.Log(saveData.turretsHp.Count);
            if (droneGo)
            {
                for (int i = 0; i < saveData.DronesHp.Count; i++)
                {
                    var dronePos = saveData.DronesPos[i];
                    GameObject droneClo = Instantiate(droneGo, dronePos, Quaternion.identity);
                    Enemy_Drone drone = droneClo.GetComponent<Enemy_Drone>();
                    drone.droneModel= GetEnemyInfoByID(1);
                    drone.droneModel.HP.Value = saveData.DronesHp[i];
                }
            }
            //Scene scene = SceneManager.GetActiveScene();
            //Debug.Log(scene.name);
            var gun = this.GetSystem<IGunSystem>();
            gun.GunList.Clear();
            for (int i = 0; i < saveData.WeaponsID.Count; i++)
            {
                IGunConfigModel saveGun = GetWeaponInfoByID(saveData.WeaponsID[i]);
                //Debug.Log(saveGun.Name);                
                saveGun.BulletCount.Value = saveData.WeaponsBullet[i];                
                gun.GunList.Add(saveGun);
            }
            UIWeaponItemManager.Instance.StoreWeaponItem("ShotGun");
            //gun.CurrentGun = gun.GunList.Last();
            for (int i = 0; i < saveData.InventoryItemsID.Count; i++)
            {
                for (int j = 0; j < saveData.ItemsAmount[i]; j++)
                {
                    Inventory.Instance.StoreItem(saveData.InventoryItemsID[i]);
                }                
            }
        }
        else
            Debug.LogWarning("The Json File is Not Exist");
    }
    #endregion
    public IArchitecture GetArchitecture()
    {
        return Game2D.Interface;
    }
}

