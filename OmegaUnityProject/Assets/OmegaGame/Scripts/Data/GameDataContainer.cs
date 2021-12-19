using System.Collections;
using System.Collections.Generic;
using Omega.Types;
using UnityEngine;

public class GameDataContainer
{
    public List<GameObjectController> AllGameObjectsControllers { get; set; } = new List<GameObjectController>();
    public List<CharacterObjectController> CharacterObjectControllers { get; set; } = new List<CharacterObjectController>();
    public GameMode CurrentGameMode { get; set; } = GameMode.CreatorMode;
    public GameObject CurrentSelectedGameObject {get; set;} = null;
    public CharacterObjectController CurrentActivePlayer { get; set; } = null;
}
