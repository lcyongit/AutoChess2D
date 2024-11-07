using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager>
{
    [Header("Character Info")]
    public List<CharacterStateMachine> playerCharacterList = new List<CharacterStateMachine>();
    public List<CharacterStateMachine> enemyCharacterList = new List<CharacterStateMachine>();

    //[Header("Event Sub")]
    //public CharacterStateMachineEventSO characterStateMachineEventSO;

    private void OnEnable()
    {

    }

    private void OnDisable()
    {


    }



    protected override void Awake()
    {
        base.Awake();
        

    }

    private void Start()
    {

    }

    public void RegisterInList(CharacterStateMachine character)
    {
        if (character.CompareTag("PlayerUnit"))
        {
            if (!playerCharacterList.Contains(character))
                playerCharacterList.Add(character);

        }
        else if (character.CompareTag("EnemyUnit"))
        {
            if (!enemyCharacterList.Contains(character))
                enemyCharacterList.Add(character);
        }
    }

    public void UnRegisterFromList(CharacterStateMachine character)
    {
        if (character.CompareTag("PlayerUnit"))
            playerCharacterList.Remove(character);
        else if (character.CompareTag("EnemyUnit"))
            enemyCharacterList.Remove(character);

    }

    public CharacterStateMachine SearchTarget(CharacterStateMachine character)
    {
        CharacterStateMachine target = null;
        float closestDistance = Mathf.Infinity;

        if (character.CompareTag("PlayerUnit"))
        {
            foreach (var targetStateMachine in enemyCharacterList)
            {
                float distance = Vector3.Distance(character.transform.position, targetStateMachine.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = targetStateMachine;
                }

            }
        }
        else if (character.CompareTag("EnemyUnit"))
        {
            foreach (var targetStateMachine in playerCharacterList)
            {
                float distance = Vector3.Distance(character.transform.position, targetStateMachine.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = targetStateMachine;
                }

            }
        }



        return target;

    }


    #region Events


    #endregion

}
