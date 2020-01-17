using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.IO;

namespace MyASPBackend.Models
{
    /// <summary>
    /// Do serializacji xml.
    /// </summary>
    public static class XmlSerializator
    {
        /// <summary>
        /// Serializuje xml'a z czegokolwiek
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static void SerializeToXml<T>(string _filename, T _objectToSerialize)
        {
            var fs = new FileStream(_filename, FileMode.Create, FileAccess.Write, FileShare.Read);
            var sw = new StreamWriter(fs);
            var xs = new XmlSerializer(typeof(T));

            try
            {
                xs.Serialize(sw, _objectToSerialize);
            }
            catch (System.Exception _e)
            {
                Console.WriteLine(_e);
            }

            fs.Flush();
            fs.Close();
            fs.Dispose();
        }


        /// <summary>
        /// Deserializuje xml'a z czegokolwiek
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string _filename)
        {
            var toRet = default(T);

            //Gdy nie ma pliku.
            if (!File.Exists(_filename))
            {
                return toRet;
            }

            var sr = new StreamReader(_filename);
            var xs = new XmlSerializer(typeof(T));

            try
            {
                toRet = (T)xs.Deserialize(sr);
            }
            catch (System.Exception _e)
            {
                Console.WriteLine(_e);
            }

            sr.Close();
            sr.Dispose();

            return toRet;
        }
    }

    public class User
    {
        /// <summary>
        /// Dostep do wszystkich userow
        /// </summary>
        public static List<User> AllUsers = new List<User>();
        /// <summary>
        /// Sciezka do bazy danych.
        /// </summary>
        public static readonly string DataBaseLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\database.xml";

        /// <summary>
        /// Unikalne ID usera
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Nick usera
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// Dostep do emaila
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Dostep do haselka
        /// </summary>
        public string Password { get; set; }

        static void ReloadDataBase()
        {
            var tmp = XmlSerializator.DeserializeFromXml<List<User>>(DataBaseLocation); ;
            if (tmp != null)
                AllUsers = tmp;
        }

        /// <summary>
        /// Zapisuje aktualna baze danych.
        /// </summary>
        static void SaveDataBase()
        {
            XmlSerializator.SerializeToXml(DataBaseLocation, AllUsers);
        }


        /// <summary>
        /// Sprawdza, czy zadany user juz istnieje.
        /// </summary>
        /// <param name="_nick"></param>
        /// <returns></returns>
        public static bool UserExists(string _nick)
        {
            if (AllUsers.Count <= 0)
                ReloadDataBase();

            for (int i = 0; i < AllUsers.Count; i++)
                if (AllUsers[i].Nick == _nick)
                    return true;
            return false;
        }

        /// <summary>
        /// Rejestruje i dodaje nowego uzytkownika na podstawie danych rejstracji.
        /// </summary>
        /// <param name="_reg"></param>
        /// <returns></returns>
        public static User RegisterNewUser(Registration _reg)
        {
            var toRet = new User();

            toRet.Nick = _reg.Nick;
            toRet.Email = _reg.Email;
            toRet.Password = _reg.Password;

            //Sortujemy userow
            AllUsers.Sort((a, b) => a.ID.CompareTo(b.ID));

            //Mamy juz ID nowego usera.            
            toRet.ID = (AllUsers.Count > 0)? (AllUsers[AllUsers.Count - 1].ID + 1) : 0;

            AllUsers.Add(toRet);

            SaveDataBase();

            return toRet;
        }
    }
}