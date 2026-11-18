using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneMusicConfig", menuName = "Sound/Scene Music Config")]
public class SceneMusicConfig : ScriptableObject
{
    public List<MusicByType> musicByTypes;

    public AudioClip GetClip(SceneType sceneType)
    {
        return musicByTypes.FirstOrDefault(m => m.Type == sceneType).Clip;
    }
}

