using Project.Scripts.Model;
using Project.Scripts.Utils;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingScene", menuName = "ScriptableObjects/SettingScene", order = 1)]

public class SettingsScene : ScriptableObject
{
    //сохранялся ли этот сеттигс?
    public bool isSave;
    
    //имя сцены
    public string sceneName;
    //текущий стейт сцены
    public GameTypes.Game stateGame;
    
    //все динамические объекты которые присутвовали в сцене последний раз
    public ItemData[] items;
    
    //данные по игроку
    public PlayerData playerData;
    //данные по воркеру
    public WorkerData workerData;
}
