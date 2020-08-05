using System.Collections.Generic;

[System.Serializable]
public class ZenSceneData 
{
    public List<RailSaveData> railsData = new List<RailSaveData>();
    public List<EnvSaveData> envsData = new List<EnvSaveData>();
    public List<TrainSaveData> trainsData = new List<TrainSaveData>();
    public PlaygroundSaveData playgroundData = new PlaygroundSaveData();
}
