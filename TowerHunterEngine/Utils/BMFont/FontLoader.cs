// ---- AngelCode BmFont XML serializer ----------------------
// ---- By DeadlyDan @ deadlydan@gmail.com -------------------
// ---- There's no license restrictions, use as you will. ----
// ---- Credits to http://www.angelcode.com/ -----------------

using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace TowerHunterEngine.Utils.BMFont
{
    public class FontLoader
    {
        public static FontFile Load(String filename)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(FontFile));
            TextReader textReader = new StreamReader(filename);
            FontFile file = (FontFile)deserializer.Deserialize(textReader);
            textReader.Close();
            return file;
        }
    }
}
