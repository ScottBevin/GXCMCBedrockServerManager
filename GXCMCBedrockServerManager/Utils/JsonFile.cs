using System;
using Newtonsoft.Json;
using System.IO;
using System.Windows.Forms;

namespace GXCMCBedrockServerManager.Utils
{
    public class JsonFile<T> where T : class, new()
    {
        static string FileExtension { get { return ".json"; } }

        public static T LoadJsonFile(string path, string name, bool allowCreateDefault)
        {
            string fullPath = Path.Combine(path, $"{name}{FileExtension}");

            if (File.Exists(fullPath))
            {
                try
                {
                    string fileContents = File.ReadAllText(fullPath);
                    T jsonObj = JsonConvert.DeserializeObject<T>(fileContents);
                    return jsonObj;
                }
                catch (Exception e)
                {
                    if (MessageBox.Show(
                        $"Error loading {fullPath}:\n\n{e.Message}\n\nReset file to default?",
                        $"Error loading {fullPath}",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        T jsonObj = new T();
                        if (SaveJsonFile(jsonObj, path, name, false))
                        {
                            return jsonObj;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            else if (allowCreateDefault)
            {
                T jsonObj = new T();
                if (SaveJsonFile(jsonObj, path, name, false))
                {
                    return jsonObj;
                }
            }

            return null;
        }

        public static bool SaveJsonFile(T jsonObj, string path, string name, bool saveOld)
        {
            string fullPath = Path.Combine(path, $"{name}{FileExtension}");
            string fullPathTemp = Path.Combine(path, $"{name}.tmp{FileExtension}");
            string fullOldPath = Path.Combine(path, $"{name}.old{FileExtension}");

            bool renameOldSuccessful = false;

            try
            {
                File.Move(fullPath, fullOldPath);
                renameOldSuccessful = true;
            }
            catch
            {

            }

            try
            {
                string json = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
                File.WriteAllText(fullPathTemp, json);
                File.Move(fullPathTemp, fullPath);

                return true;
            }
            catch (Exception e)
            {
                if(renameOldSuccessful)
                {
                    File.Move(fullOldPath, fullPath);
                }

                MessageBox.Show(
                    $"Error saving {fullPath}",
                    e.Message);

                return false;
            }
        }
    }
}
