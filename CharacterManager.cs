using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    static CharacterManager instance = null;

    GameManager gameMgr;
    NoteManager noteMgr;

    public float Speed = 0;
    public Transform testpos;

    [SerializeField] private Character characterObjectPrefab;
    [SerializeField] private Character characterObjectPrefab2;

    Queue<Character> poolingObjectQueue = new Queue<Character>();
    Queue<Character> poolingObjectQueue2 = new Queue<Character>();
    public List<Character> ActiveCharacterList = new List<Character>();

    public int ReadyCharacter = 0;

    public static CharacterManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = FindObjectOfType<CharacterManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(gameObject);
            return;
        }
        gameMgr = GameManager.Instance;
        noteMgr = NoteManager.Instance;

        CharacterInitialize(ReadyCharacter);
    }

    private void Update()
    {
        if (gameMgr.m_bPlayCheck)
        {
            if (ActiveCharacterList != null || ActiveCharacterList.Count > 0)
            {
                for (int i = 0; i < ActiveCharacterList.Count; i++)
                {
                    ActiveCharacterList[i].CharacterMove(Speed, noteMgr.bpm);
                }
            }
        }
    }

    public void Restart()
    {
        ActiveCharacterReset();
        ActiveCharacterList.Clear();
    }

    public Character ResultGetCharacter()
    {
        int test = UnityEngine.Random.Range(1, 3);
        var ResultCharacter = GetCharacter(test);
        ResultCharacter.SetPosX(testpos);
        ResultCharacter.ResultAction();

        return ResultCharacter;
    }

    void ActiveCharacterReset()
    {
        for (int i = 0; i < ActiveCharacterList.Count; i++)
        {
            ActiveCharacterList[i].gameObject.SetActive(false);
            ActiveCharacterList[i].transform.SetParent(transform);
            ActiveCharacterList[i].SetPosX(transform);
            ActiveCharacterList[i].LayerReset();
            if (ActiveCharacterList[i].characternum == 1)
                poolingObjectQueue.Enqueue(ActiveCharacterList[i]);
            else
                poolingObjectQueue2.Enqueue(ActiveCharacterList[i]);
        }
    }

    public Character GetCharacter(int noteNum)
    {
        if (noteNum == 1)
        {
            if (Instance.poolingObjectQueue.Count > 0)
            {
                var Character = Instance.poolingObjectQueue.Dequeue();
                Character.transform.SetParent(null);
                Character.gameObject.SetActive(true);
                ActiveCharacterList.Add(Character);
                return Character;
            }
            else
            {
                var newCharacter = Instance.CreatNewCharacter(characterObjectPrefab);
                newCharacter.gameObject.SetActive(true);
                newCharacter.transform.SetParent(null);
                ActiveCharacterList.Add(newCharacter);
                return newCharacter;
            }
        }
        else
        {
            if (Instance.poolingObjectQueue2.Count > 0)
            {
                var Character = Instance.poolingObjectQueue2.Dequeue();
                Character.transform.SetParent(null);
                Character.gameObject.SetActive(true);
                ActiveCharacterList.Add(Character);
                return Character;
            }
            else
            {
                var newCharacter = Instance.CreatNewCharacter(characterObjectPrefab2);
                newCharacter.gameObject.SetActive(true);
                newCharacter.transform.SetParent(null);
                ActiveCharacterList.Add(newCharacter);
                return newCharacter;
            }

        }
    }

    public void ReturnObject(Character character)
    {
        character.gameObject.SetActive(false);
        character.transform.SetParent(transform);
        character.SetPosX(transform);
        if (character.characternum == 1)
            Instance.poolingObjectQueue.Enqueue(character);
        else
            Instance.poolingObjectQueue2.Enqueue(character);
        ActiveCharacterList.Remove(character);
    }

    public void CharacterInitialize(int CharacterCount)
    {
        for (int i = 0; i < CharacterCount; i++)
        {
            poolingObjectQueue.Enqueue(CreatNewCharacter(characterObjectPrefab));
            poolingObjectQueue2.Enqueue(CreatNewCharacter(characterObjectPrefab2));
        }
    }

    Character CreatNewCharacter(Character character)
    {
        var newCharacter = Instantiate(character);
        newCharacter.gameObject.SetActive(false);
        newCharacter.transform.SetParent(transform);
        newCharacter.SetPosX(transform);

        return newCharacter;
    }
}
