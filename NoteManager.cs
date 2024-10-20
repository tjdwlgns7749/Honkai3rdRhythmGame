using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NoteData
{
    public float NoteTiming = 400;

    public NoteData() { NoteTiming = 0;}
    public NoteData(float NoteTiming) { this.NoteTiming = NoteTiming;}
}

public class NoteManager : MonoBehaviour
{
    static NoteManager instance = null;

    public int ReadyNote = 0;
    float NoteTime = 0.03f;//��������
    float m_speed;

    public float noteSpeed;
    public int bpm { get; private set; }

    AudioManager audioMgr;
    CharacterManager characterMgr;
    GameManager gameMgr;

    [SerializeField] Transform NoteSpawnPos = null;
    [SerializeField] private Note poolingObjectPrefab;
    [SerializeField] private Note poolingObjectPrefab2;

    Queue<Note> poolingObjectQueue = new Queue<Note>();
    Queue<Note> poolingObjectQueue2 = new Queue<Note>();
    List<Note> ActiveNoteList = new List<Note>();
    List<NoteData> noteDatas = new();
    int noteDataIndex = 0;
    
    public static NoteManager Instance
    {
        get
        {
            if (null == instance)
            {
                instance = FindObjectOfType<NoteManager>();
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
        audioMgr = AudioManager.Instance;
        characterMgr = CharacterManager.Instance;
        gameMgr = GameManager.Instance;

        NoteInitialize(ReadyNote);
    }

    void Update()
    {
        if (gameMgr.m_bPlayCheck)
        {
            if (noteDataIndex < noteDatas.Count && Mathf.Abs(audioMgr.bgmSource.time - (noteDatas[noteDataIndex].NoteTiming - audioMgr.MusicDataList[audioMgr.MusicIndexNum].realMusic)) < NoteTime)
            {
                int rNum = UnityEngine.Random.Range(0, 9);
                var test = GetNote(rNum);
                characterMgr.GetCharacter(test.noteNum);
                noteDataIndex++;
            }

            if (ActiveNoteList != null || ActiveNoteList.Count > 0)
            {
                for (int i = 0; i < ActiveNoteList.Count; i++)
                {
                    ActiveNoteList[i].NoteMove(bpm, noteSpeed * m_speed);
                }
            }
        }
    }

    public void Restart()
    {
        ActiveNoteReset();
        ActiveNoteList.Clear();
        noteDataIndex = 0;
    }

    void ActiveNoteReset()
    {
        for (int i = 0; i < ActiveNoteList.Count; i++)
        {
            ActiveNoteList[i].gameObject.SetActive(false);
            ActiveNoteList[i].SetSpawnPosX(NoteSpawnPos);
            if (ActiveNoteList[i].noteNum == 1)
                poolingObjectQueue.Enqueue(ActiveNoteList[i]);
            else
                poolingObjectQueue2.Enqueue(ActiveNoteList[i]);
        }
    }

    public void GetNoteData(float speed)
    {
        m_speed = speed;
        bpm = audioMgr.MusicDataList[audioMgr.MusicIndexNum].bpm - 73;
        noteDatas = XML<NoteData>.Read(audioMgr.MusicDataList[audioMgr.MusicIndexNum].name);
    }

    public Note GetNote(int ranNum)
    {
        if (ranNum < 5)
        {
            if (Instance.poolingObjectQueue.Count > 0)
            {
                var note = Instance.poolingObjectQueue.Dequeue();
                note.gameObject.SetActive(true);
                ActiveNoteList.Add(note);
                return note;

            }
            else
            {
                var newNote = Instance.CreatNewNode(poolingObjectPrefab);
                newNote.gameObject.SetActive(true);
                ActiveNoteList.Add(newNote);
                return newNote;
            }
        }
        else
        {
            if (Instance.poolingObjectQueue2.Count > 0)
            {
                var note = Instance.poolingObjectQueue2.Dequeue();
                note.gameObject.SetActive(true);
                ActiveNoteList.Add(note);
                return note;

            }
            else
            {
                var newNote = Instance.CreatNewNode(poolingObjectPrefab2);
                newNote.gameObject.SetActive(true);
                ActiveNoteList.Add(newNote);
                return newNote;
            }

        }
    }

    public void ReturnObject(Note note)
    {
        note.gameObject.SetActive(false);
        note.SetSpawnPosX(NoteSpawnPos);
        if (note.noteNum == 1)
            Instance.poolingObjectQueue.Enqueue(note);
        else
            Instance.poolingObjectQueue2.Enqueue(note);
        ActiveNoteList.Remove(note);
    }


    void NoteInitialize(int NoteCount)
    {
        for (int i = 0; i < NoteCount; i++)
        {
            poolingObjectQueue.Enqueue(CreatNewNode(poolingObjectPrefab));
            poolingObjectQueue2.Enqueue(CreatNewNode(poolingObjectPrefab2));
        }
    }

    Note CreatNewNode(Note note)
    {
        var newNote = Instantiate(note);
        newNote.gameObject.SetActive(false);
        newNote.transform.SetParent(transform.Find("Notecanvas"),false);
        newNote.SetSpawnPosX(NoteSpawnPos);

        return newNote;
    }
}

public class XML<T> where T : new()
{
    /// <summary>
    /// Resources �������� xml ������ ã�´�.
    /// </summary>
    public static List<T> Read(string name)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

        // name���� Ȯ���ڸ� ����.
        name = System.IO.Path.GetFileNameWithoutExtension(name);
        var xmlText = Resources.Load<TextAsset>(name);
        if (!xmlText) throw new Exception(string.Format("Not Find TextAsset : {0}", name));

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlText.text); // text������ xml ������ ���·� ��ȯ.
        var root = xmlDoc.DocumentElement;
        List<T> dataList = new List<T>();
        foreach (XmlElement node in root.ChildNodes)
        {
            // Reflection�� �̿��Ͽ�,
            T data = new T();
            var type = data.GetType();   //  T�� ��������(Type)��,
            var fields = type.GetFields(); // ����(Field)���� ã�´�.
            foreach (var field in fields)
            {
                // xml���� ���� ��� ���� Attribute�� ã�� ���� ���� �´�.
                var strValue = node.GetAttribute(field.Name);
                if (null != strValue) // �� ���ڿ��� ���� �� ���� ������ null Ȯ�θ� �Ѵ�.
                {
                    object value = strValue;
                    if (int.TryParse(strValue, out int i)) value = i;
                    else if (float.TryParse(strValue, out float f)) value = f;
                    else if (double.TryParse(strValue, out double d)) value = d;

                    // �ش� field�� �´� ������ �����͸� �Է�.
                    field.SetValue(data, value);
                }
            }
            // List�� ������ �߰�.
            dataList.Add(data);
        }

        return dataList;
    }

    /// <summary>
    /// Assets/Resources/...������ name���� xml ���� ����.
    /// </summary>
    public static void Write(string name, List<T> dataList)
    {
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
        if (null == dataList || 1 > dataList.Count) throw new ArgumentNullException("dataList");

        var xmlDoc = new XmlDocument();
        var root = xmlDoc.CreateElement("root"); // xml �������� root ����.
        xmlDoc.AppendChild(root);                    // xml �����Ϳ� root �߰�.

        var type = typeof(T); // Reflection�� �̿��Ͽ� �����͸� ���ϱ� ���� Type�� ���Ѵ�.
        foreach (var data in dataList)
        {
            // T�� ���������� �̸����� �ϴ� Element�� ����.
            var node = xmlDoc.CreateElement(type.Name);

            var fields = type.GetFields();
            foreach (var field in fields)
            {
                // �������� �̸����� �ϴ� Attribute�� ����.
                var attr = xmlDoc.CreateAttribute(field.Name);
                // ������ ���ϰ� �ִ� ���� ���ڿ����·� ��ȯ�Ͽ� Attribute�� ����.
                attr.Value = field.GetValue(data).ToString();
                // Element�� Attribute �߰�.
                node.Attributes.Append(attr);
            }
            // root�� Element �߰�.
            root.AppendChild(node);
        }

        // Ȯ���ڰ� ������ ".xml"�� ����, ���ٸ� �߰�.
        name = System.IO.Path.ChangeExtension(name, ".xml");
        // Application.dataPath : Assets ���������� ���.
        // .../Assets/Resources/name.xml
        var path = string.Format(@"{0}/Resources/{1}", Application.dataPath, name);
        // ���ϸ��� ������ ������ ���������� ��θ��� ���Ѵ�.
        var directoryPath = System.IO.Path.GetDirectoryName(path);
        // Assets ������ �ٷ� ������ Resources ������ ���ٸ� ����.
        if (!System.IO.Directory.Exists(directoryPath)) System.IO.Directory.CreateDirectory(directoryPath);

        // �ش� ������Ʈ�� Resources/...������ xml ���� ����.
        xmlDoc.Save(path);
    }
}