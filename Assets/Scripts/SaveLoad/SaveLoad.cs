using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public static class SaveLoad
{

    public static Game savedgame = new Game();

    private static int numSaveClassesCompleted = 0;
    private static int numLoadClassesCompleted = 0;

    private static Action SaveCallBack;
    private static Action LoadCallBack;

    public static void DeleteGame()
    {
        try
        {
            File.Delete(Application.persistentDataPath + "/savedgame.gd");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private static void SaveHelper()
    {
        numSaveClassesCompleted++;

        if (numSaveClassesCompleted == 1)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/savedgame.gd");
            bf.Serialize(file, SaveLoad.savedgame);
            file.Close();
            SaveCallBack();
        }
    }
    private static void LoadHelper()
    {
        numLoadClassesCompleted++;

        if (numLoadClassesCompleted == 1)
        {
            LoadCallBack();
        }
    }
    public static void Save(Action CallBack)
    {
        Debug.Log("Saving...");
        //MonoBehaviour.print("saving");
        SaveCallBack = CallBack;

        numSaveClassesCompleted = 0;

        PlayerMain.Save(SaveHelper);


    }
    public static void Load(Action CallBack)
    {
        Debug.Log("Loading");
        numLoadClassesCompleted = 0;
        LoadCallBack = CallBack;

        Debug.Log("Attempting Load");

        if (GlobalData.loadGame && File.Exists(Application.persistentDataPath + "/savedgame.gd"))
        {
            Debug.Log("Loading...");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedgame.gd", FileMode.Open);
            SaveLoad.savedgame = (Game)bf.Deserialize(file);
            file.Close();
        }
        else
        {
            SaveLoad.savedgame = new Game();
            Debug.Log("New game created: " + (savedgame != null));
        }

        PlayerMain.Load(LoadHelper);
        //ItemInv.Load(LoadHelper);
        //StartingPos.Load(LoadHelper);
        //MonInv.Load(LoadHelper);
        //PlayerStats.Load(LoadHelper);
        //Debug.Log("both loaded");
    }
}
