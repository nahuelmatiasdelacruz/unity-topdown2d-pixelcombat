public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneEntrance {get; private set;}
    public void SetEntranceName(string sceneEntranceName)
    {
        SceneEntrance = sceneEntranceName;
    }
}
