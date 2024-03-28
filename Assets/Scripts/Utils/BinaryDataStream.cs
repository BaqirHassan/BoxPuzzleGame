using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class BinaryDataStream
{
    public static void Save<T>(T serializeObject, string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        Directory.CreateDirectory(path);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName + ".dat", FileMode.Create);


        try
        {
            formatter.Serialize(fileStream, serializeObject);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Save filed. Error : " + e.ToString());
        }
        finally
        {
            fileStream.Close();
        }
    }

    public static bool Exist(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        string fullFileName = fileName + ".dat";

        return File.Exists(path + fullFileName);
    }

    public static T Read<T>(string fileName)
    {
        string path = Application.persistentDataPath + "/saves/";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path + fileName + ".dat", FileMode.Open);

        T returnType = default(T);

        try
        {
            returnType = (T) formatter.Deserialize(fileStream);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Read filed. Error : " + e.ToString());
        }
        finally
        {
            fileStream.Close();
        }

        return returnType;

    }

}
