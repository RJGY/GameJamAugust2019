using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class PlayerSaveToBinary
{
    public static void SavePlayerData(PlayerHandler player)
    {
        // Creates a binary formatter object
        BinaryFormatter formatter = new BinaryFormatter();

        // Location to save
        string path = Application.persistentDataPath + "/" + player.name + ".png";

        // Creates a stream object
        FileStream stream = new FileStream(path, FileMode.Create);

        // Data to write.
        PlayerDataToSave data = new PlayerDataToSave(player);

        // Saves data into the specific path we want it to into a specific format.  
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerDataToSave LoadData(PlayerHandler player)
    {
        string path = Application.persistentDataPath + "\\" + player.name + ".png";

        if (File.Exists(path))
        {
            // Creates objects.
            BinaryFormatter formatter = new BinaryFormatter();

            // Creates filepath at the specific place where the file was saved and opens it.
            FileStream stream = new FileStream(path, FileMode.Open);

            // Deserialize data in a certain format that the PlayerDataToSave can read.
            PlayerDataToSave data = formatter.Deserialize(stream) as PlayerDataToSave;

            // Close Stream
            stream.Close();

            // Return the data to the function which called it.
            return data;
        }

        else
        {
            // Return nothing cause it doesnt exist.
            return null;
        }
    }
}
