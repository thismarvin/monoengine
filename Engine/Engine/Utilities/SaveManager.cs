
using System;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;

namespace Engine.Engine.Utilities
{
    public class SaveManager
    {

        public SaveManager()
        {

        }

        public void Save(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                var XML = new XmlSerializer(typeof(SaveManager));
                XML.Serialize(stream, this);
            }
        }

        public static SaveManager LoadFromFile(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var XML = new XmlSerializer(typeof(SaveManager));
                return (SaveManager)XML.Deserialize(stream);
            }
        }
    }
}
