using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SaveMyPosition : MonoBehaviour
{
    string savePath;
    //savePath is the location the save file will be.
    SaveData data;
    Scene scene;

    void Start()
    {
        //Adding gameObject.name allows us to make multiple files based on the object names
        savePath = Application.persistentDataPath + "/" + gameObject.name + "mySave.dat";
        Debug.Log(savePath);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Save();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }
    void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file;
        //If file path doesn't exist, create it. Otherwise open the file.
        //If the file already exists, the path will exist.
        if (!File.Exists(savePath))
        {
            file = File.Create(savePath);
        }
        else
        {
            file = File.Open(savePath, FileMode.Open);
        }
        data = new SaveData(transform.position);
        bf.Serialize(file, data);
        file.Close();
    }
    void Load()
    {
        if (File.Exists(savePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(savePath, FileMode.Open);
            data = (SaveData)bf.Deserialize(file);
            file.Close();
            transform.position = data.GetVector3();
        }
    }
}
[System.Serializable]
public class SaveData
{
    public float x;
    public float y;
    public float z;

    public SaveData(Vector3 position)
    {
        x = position.x;
        y = position.y;
        z = position.z;
    }
    public Vector3 GetVector3()
    {
        return new Vector3(x, y, z);
    }
}
