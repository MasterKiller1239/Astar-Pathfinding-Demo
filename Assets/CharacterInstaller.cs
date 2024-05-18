//using pathfinding;
//using UnityEngine;
//using Zenject;

//public class CharacterInstaller : MonoInstaller
//{
//    [SerializeField]
//    private Pathfinder pathfinder;
//    [ SerializeField]
//    private CharacterMovement characterController;
//    [SerializeField]
//    private Animator animator;
//    [SerializeField]
//    private ParticleSystem particles;

//    public override void InstallBindings()
//    {
//        Container.BindInstance(particles).AsSingle().NonLazy();
//        Container.BindInstance(animator).AsSingle().NonLazy();
//        Container.BindInstance(pathfinder).AsSingle().NonLazy();
//        Container.BindInstance(characterController).AsSingle().NonLazy();
//    }
//}
