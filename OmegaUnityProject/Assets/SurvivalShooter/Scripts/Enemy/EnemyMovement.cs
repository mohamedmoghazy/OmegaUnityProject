using UnityEngine;
using System.Collections;
using OmegaGame.DataReaders;

public class EnemyMovement : GameObjectController
{
    Transform player;
    // PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;

    private GameDataReader _gameDataReader;

    public override void Init(OnSwitchGameModeNotification onSwitchGameModeNotification)
    {
        base.Init(onSwitchGameModeNotification);
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
        _gameDataReader = DataReaders.Cache.Get<GameDataReader>();
    }

    // void Awake ()
    // {
    //     player = GameObject.FindGameObjectWithTag ("Player").transform;
    //     // playerHealth = player.GetComponent <PlayerHealth> ();
    //     enemyHealth = GetComponent <EnemyHealth> ();
    //     nav = GetComponent <UnityEngine.AI.NavMeshAgent> ();
    // }

    protected override void SetCreatorModeActive(bool isCreatorMode)
    {
        base.SetCreatorModeActive(isCreatorMode);
        
        if (isCreatorMode)
        {
            ResetInit();
        }
        else if(_gameDataReader.GetCurrentActiveCharacter())
        {
            player = _gameDataReader.GetCurrentActiveCharacter().gameObject.transform;
            nav.enabled = true;
        }
    }

    private void ResetInit()
    {
        player = null;
        enemyHealth.ResetInit();
        // gameObject.SetActive(true);
        nav.enabled = false;
        
    }
    
    void Update ()
    {
        if(enemyHealth?.currentHealth > 0 && player)
        {
            nav?.SetDestination (player.position);
        }
    }
}
