using pathfinding;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [field: SerializeField]
    private TileGrid grid;

    [field: SerializeField]
    private MapGenerator mapGenerator;
    [field: SerializeField]
    private SettingsManager settingsManager;
    [SerializeField]
    private CharacterMovement player;
    public override void InstallBindings()
    {

        Container.BindFactory<CharacterMovement, CharacterMovement.Factory>().FromComponentInNewPrefab(player).AsSingle().NonLazy();
        Container.BindInstance(grid).AsSingle().NonLazy();
        Container.BindInstance(mapGenerator).AsSingle().NonLazy();
        Container.BindInstance(settingsManager).AsSingle().NonLazy();
    }
}
