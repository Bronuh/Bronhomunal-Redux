using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bronuh.File
{
    /// <summary>
    /// Класс, отвечающий за сериализацию и десериализацию объектов
    /// </summary>
    class Serializer
    {


        /// <summary>
        /// Сериализация XML как строки
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string SerializeToXmlString<T>(T target)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));

            string text;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, target);
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                text = reader.ReadToEnd();
            }
            return text;
        }



        /// <summary>
        /// Сериализация XML как строки
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string SerializeToXmlString(object target)
        {
            XmlSerializer formatter = new XmlSerializer(target.GetType());

            string text;
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, target);
                stream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(stream);
                text = reader.ReadToEnd();
            }
            return text;
        }



        /// <summary>
        /// Десериализация XML как строки в объект типа T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeFromXmlString<T>(string xml)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(xml);
                stream.Seek(0, SeekOrigin.Begin);

                return (T) formatter.Deserialize(stream);
            }
        }


        /// <summary>
        /// Десериализация XML как строки в объект типа T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object DeserializeFromXmlString(string xml, Type type)
        {
            XmlSerializer formatter = new XmlSerializer(type);
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(xml);
                stream.Seek(0, SeekOrigin.Begin);

                return formatter.Deserialize(stream);
            }
        }




        /// <summary>
        /// Сериализует и десериализует XML как поток памяти
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static T DeserializeFromXmlStream<T>(Stream stream)
        {

            XmlSerializer formatter = new XmlSerializer(typeof(T));
            stream.Seek(0, SeekOrigin.Begin);

            return (T)formatter.Deserialize(stream);
            
        }




        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Stream SerializeToXmlStream<T>(T target)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(T));

            Stream stream = new MemoryStream();
            
                formatter.Serialize(stream, target);
                stream.Seek(0, SeekOrigin.Begin);
            
            return stream;
        }

    }
}
