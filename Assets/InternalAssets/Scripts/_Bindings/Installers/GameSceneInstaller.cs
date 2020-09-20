using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "GameSceneInstaller", menuName = "Installers/GameSceneInstaller")]
public class GameSceneInstaller : ScriptableObjectInstaller<GameSceneInstaller>
{
    public TestSettings testSettings;

    public override void InstallBindings()
    {
        Container.BindInstance(testSettings);
    }
}

[System.Serializable]
public class TestSettings
{
    public int testNum;
    public int EnemiesCreateCount;
}

