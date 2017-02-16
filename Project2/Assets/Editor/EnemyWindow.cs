using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Sprites;

public class EnemyWindow : EditorWindow {

    //initialization to pull assets from assetdatabase
    public List<Enemies> enemyList = new List<Enemies>();
    public List<string> enemyNameList = new List<string>();
    public string[] enemyNameArray;

    //stats to be shown in tool
    string myName = "";
    Sprite notMySprite;
    int myHealth = 1;

    //choices for if it's a new enemy or a stored one
    public int currentRoyce = 0;
    int lastRoyce = 0;

    //flags
    bool nameFlag = false;
    bool existingFlag = false;
    bool noSpriteFlag = false;

    [MenuItem("Custom Tools/Enemy Tools %g")]
    static void WindowOpener()
    {
        EditorWindow.GetWindow<EnemyWindow>();
    }
    void Awake()
    {
        getEnemies();
    }

    void OnGUI()
    {
        currentRoyce = EditorGUILayout.Popup(currentRoyce, enemyNameArray);

        EditorGUILayout.Space();

        //showing sprite only if it exists
        if(notMySprite != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Texture2D myTexture = SpriteUtility.GetSpriteTexture(notMySprite, false);
            GUILayout.Label(myTexture);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        notMySprite = (Sprite)EditorGUILayout.ObjectField(notMySprite, typeof(Sprite), false) as Sprite;
        myName = EditorGUILayout.TextField("Name: ", myName);
        myHealth = EditorGUILayout.IntSlider("Health: ", myHealth, 1, 300);

        EditorGUILayout.Space();

        if (currentRoyce == 0)
        {
            if(GUILayout.Button("Create"))
            {
                CreateEnemies();
            }
        }
        else
        {
            if (GUILayout.Button("Save"))
            {
                SaveEnemy();
            }
        }

        if (currentRoyce != lastRoyce)
        {
            switch (currentRoyce)
            {
                case 0:
                    NewEnemy();
                    break;
                default:
                    CurrentEnemy();
                    break;
            }
        }

        //flag errors
        if(nameFlag)
        {
            EditorGUILayout.HelpBox("Name Can not be blank", MessageType.Warning);
        }
        if (existingFlag)
        {
            EditorGUILayout.HelpBox("This Unit Already exists", MessageType.Error);
        }
        if(noSpriteFlag)
        {
            EditorGUILayout.HelpBox("Can't save without a sprite", MessageType.Error);
        }

    }

    void getEnemies()
    {
        enemyList.Clear();
        enemyNameList.Clear();

        string[] guids = AssetDatabase.FindAssets("t:Enemies");
        foreach (string guid in guids)
        {
            string RoyceString = AssetDatabase.GUIDToAssetPath(guid);
            Enemies enemyInst = AssetDatabase.LoadAssetAtPath(RoyceString, typeof(Enemies)) as Enemies;
            enemyNameList.Add(enemyInst.emname);
            enemyList.Add(enemyInst);
        }
        enemyNameList.Insert(0, "new");
        enemyNameArray = enemyNameList.ToArray();

        SwitchFlags();
    }

    private void NewEnemy()
    {
        myHealth = 1;
        myName = "";
        //existingFlag = false;
        //nameFlag = false;
        SwitchFlags();
    }

    private void CurrentEnemy()
    {
        //other stats
        myName = enemyList[currentRoyce - 1].emname;
    }

    private void SaveEnemy()
    {
        //other stats
        enemyList[currentRoyce - 1].emname = myName;

    }

    private bool CreateEnemies()
    {
        if (myName == string.Empty)
        {
            nameFlag = true;
            return false;
        }
        if(notMySprite == null)
        {
            noSpriteFlag = true;
            return false;
        }
        //check if guy already exists
        string[] assetString = AssetDatabase.FindAssets(myName.Replace(" ", "_"));
        if (assetString.Length > 0)
        {
            existingFlag = true;
            return false;
        }
        Enemies myEnemy = ScriptableObject.CreateInstance<Enemies>();
        myEnemy.emname = myName;
        myEnemy.health = myHealth;
        AssetDatabase.CreateAsset(myEnemy, "Assets/Resources/Data/EnemyData/" + myEnemy.emname.Replace(" ", "_") + ".asset");
        nameFlag = false;
        getEnemies();
        for(int i = 0; i < enemyList.Count; i++)
        {
            if(enemyList[i].emname == myName)
            {
                currentRoyce = i + 1;
            }
        }
        return true;
    }

    private void SwitchFlags()
    {
        existingFlag = false;
        nameFlag = false;
        noSpriteFlag = false;
    }
}
