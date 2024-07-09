using QFramework;
public class Game2D : Architecture<Game2D>
{
    protected override void Init()
    {
        this.RegisterModel<IPlayerModel>(new PlayerModel());
        this.RegisterModel<IEnemyModel>(new EnemyModel());
        this.RegisterModel<ISaveDataModel>(new SaveDataModel());
        this.RegisterModel<IGunConfigModel>(new GunConfigModel());
        this.RegisterSystem<IDropItemSystem>(new DropItemSystem());
        this.RegisterSystem<IStateSystem>(new StateSystem());
        this.RegisterSystem<IGunSystem>(new GunSystem(1));
        this.RegisterSystem<ITimeSystem>(new TimeSystem());
        //this.RegisterModel<IGunConfigModel>(new GunConfigModel());
    }
}
