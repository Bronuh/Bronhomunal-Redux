using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bronuh.File
{
    class SaveLoad
    {
        /// <summary>
        /// Метод загрузки файла в строку
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string LoadString(string path)
        {
            string text;
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                StreamReader reader = new StreamReader(fs);
                text = reader.ReadToEnd();
            }

            return text;
        }





        /// <summary>
        /// Метод загрузки файла в поток
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Stream LoadStream(string path)
        {
            Stream stream = new MemoryStream();

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                fs.CopyTo(stream);
            }

            stream.Seek(0,SeekOrigin.Begin);
            return stream;
        }





        /// <summary>
        /// Сохраняет поток в файл
        /// </summary>
        /// <param name="stream">Записываемый поток</param>
        /// <param name="path">Путь к файлу</param>
        public static void SaveStream(Stream stream, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(fs);
            }
        }




        /// <summary>
        /// Записывает строку в файл
        /// </summary>
        /// <param name="text">Записываемая строка</param>
        /// <param name="path">Путь к файлу</param>
        public static void SaveString(String text, string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                StreamWriter writer = new StreamWriter(fs);
                writer.Write(text);
            }
        }





        /// <summary>
        /// Загружает объект какого-то там типа Т, если покайфу
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadObject<T>(string path)
        {
            T loadedObject;
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(T));
                using (StreamReader sr = new StreamReader(path))
                {
                    loadedObject = (T)xs.Deserialize(sr);
                    return loadedObject;
                }
            }
            catch(Exception e)
            {
                Logger.Error("(LOAD) "+e.Message);
                return default(T);
            }
        }



        /// <summary>
        /// Сохраняет объект в файл
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="path"></param>
        public static void SaveObject<T>(T target, string path)
        {
            try
            {
                Logger.Log("Сохранение объекта " + typeof(T));
                XmlSerializer xs = new XmlSerializer(typeof(T));

                using (StreamWriter writer = new StreamWriter(path))
                {
                    xs.Serialize(writer, target);
                }
            }
            catch(Exception e)
            {
                Logger.Error(e.Message);
            }
        }
    }
}
