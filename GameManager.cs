using QFramework;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(0)]
public class GameManager : MonoBehaviour,IController
{
    private static GameManager instance;
    public static GameManager Instance => instance;
    private IPlayerModel playerModel;
    private IGunSystem gunSystem;    
    private AudioSource audioSource;
    public AudioClip explosion;
    public AudioClip playerExplosion;
    public AudioClip Bgm;
    public AudioClip playerShootClip;
    public AudioClip TurretClip;
    public AudioClip playerHurtClip;    
    public AudioClip ClipButtonClip;    
    public AudioClip PistolReloadClip;
    public AudioClip ShotGunReloadClip;
    public AudioClip BulletHitClip;
    public AudioClip PickUpClip;
    public AudioClip JumpClip;
    public AudioClip UseItemClip;
    public float VolumeScale = 0.5f;
    //玩家属性            
    private GameObject playerGo;
    //public GameUIManager gameUIManager;    
    public bool isPlayerDie = false;
    private GameObject turretGo;
    private GameObject droneGo;
    public int TurretCount=3;
    public Vector3[] SpawnTurretPos;
    public int DroneCount;
    public Vector3[] SpawnDronePos;
    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        //instance = this;
        if (Instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(this);
        playerModel = this.GetModel<IPlayerModel>();
        gunSystem = this.GetSystem<IGunSystem>();
        playerGo = Resources.Load<GameObject>("Player/Player");
        turretGo = Resources.Load<GameObject>("Enemy/turret");
        droneGo = Resources.Load<GameObject>("Enemy/drone");
        audioSource = GetComponent<AudioSource>();
        //SceneManager.sceneLoaded += OnSceneLoaded;        
    }
    private void Start()
    {
        //GameUIManager.Instance.SwitchGameMainUI();
        GameUIManager.Instance.InventoryPanel.SetActive(false);
        audioSource.Play();
        audioSource.loop = true;        
        if (!File.Exists(GameUIManager.Instance.streamPath))
        {
            GameObject playerClone=Instantiate(playerGo, new Vector3(-2.0f, 0, 0), Quaternion.identity);
            Player player = playerClone.GetComponent<Player>();
            player.playerModel.HP.Value = playerModel.MaxHp;
            for (int i = 0; i < TurretCount; i++)
            {
                Instantiate(turretGo, SpawnTurretPos[i], Quaternion.identity);
            }
            for (int i = 0; i < DroneCount; i++)
            {
                Instantiate(droneGo, SpawnDronePos[i], Quaternion.identity);
            }
            InventoryManager.instance.ResetInventory();
            UIWeaponItemManager.Instance.ClearSlot();
            this.GetSystem<IGunSystem>().ResetGun();
            this.GetSystem<IStateSystem>().KillCount.Value = 0;            
        }
        else
        {
            DataManager.Instance.LoadGame();
        }
    }
    private void Update()
    {
        LimiteUpdateHp();
        LimiteUpdateBullet();
        audioSource.volume = VolumeScale;
    }
    //public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.name == "GameStart"||scene.name=="GameOver")
    //    {
    //        GameUIManager.Instance.SwitchGameMainUI();
    //        audioSource.Stop();
    //    }
    //}
    #region 音频管理    
    public void PlaySound(AudioSource audioSource)
    {        
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void StopSound(AudioSource audioSource)
    {        
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(JumpClip,VolumeScale);
    }
    /// <summary>
    /// 播放玩家死亡音效
    /// </summary>
    public void PlayerExplosionAudio()
    {
        audioSource.PlayOneShot(playerExplosion, VolumeScale);
    }
    /// <summary>
    /// 播放爆炸音效
    /// </summary>
    public void EnemyExplosionAudio()
    {
        audioSource.PlayOneShot(explosion, VolumeScale);
    }
    /// <summary>
    /// 玩家射击音效
    /// </summary>
    public void PlayerShootAudio()
    {
        audioSource.PlayOneShot(playerShootClip, VolumeScale);
    }
    /// <summary>
    /// 炮塔射击音效
    /// </summary>
    public void PlayTurretShootAudio()
    {
        audioSource.PlayOneShot(TurretClip, VolumeScale);
    }
    /// <summary>
    /// 播放玩家受击音效
    /// </summary>
    public void PlayHurtAudio()
    {
        audioSource.PlayOneShot(playerHurtClip, VolumeScale);
    }    
    public void PlayClikAudio()
    {
        audioSource.PlayOneShot(ClipButtonClip, VolumeScale);
    }    
    public void PlayBulletHitAudio()
    {
        audioSource.PlayOneShot(BulletHitClip, VolumeScale);
    }
    public void PlayPistolReloadAudio()
    {
        audioSource.PlayOneShot(PistolReloadClip, VolumeScale);
    }
    public void PlayShotGunAudio()
    {
        audioSource.PlayOneShot(ShotGunReloadClip, VolumeScale);
    }
    public void PlayPickUpItemAudio()
    {
        audioSource.PlayOneShot(PickUpClip, VolumeScale);
    }
    public void PlayUseItemAudio()
    {
        audioSource.PlayOneShot(UseItemClip, VolumeScale);
    }
    #endregion
    public void LimiteUpdateHp()
    {
        if (playerModel.HP.Value <= 0)
            playerModel.HP.Value = 0;
        else if (playerModel.HP.Value >= playerModel.MaxHp)
            playerModel.HP.Value = playerModel.MaxHp;        
    }
    public void LimiteUpdateBullet()
    {
        if (gunSystem.CurrentGun.BulletInGun.Value <= 0)
            gunSystem.CurrentGun.BulletInGun.Value = 0;
        else if (gunSystem.CurrentGun.BulletInGun.Value >= 10)
            gunSystem.CurrentGun.BulletInGun.Value = 10;
    }
    public IArchitecture GetArchitecture()
    {
        return Game2D.Interface;
    }
    private void OnDestroy()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
