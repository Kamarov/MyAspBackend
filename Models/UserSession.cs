using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyASPBackend.Models
{
    /// <summary>
    /// Sesja zalogowania uzytkownika.
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// Dostep do wszystkich aktualnych sesji uzytkownikow
        /// </summary>
        public static Dictionary<string, UserSession> AllSesions = new Dictionary<string, UserSession>();
        /// <summary>
        /// Po jakim czasie nastapi automatyczne wylogowanie
        /// </summary>
        public static readonly int AutoLogOutTimeInMinutes = 10;

        /// <summary>
        /// Nick usera zalogowanego
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// Czas, kiedy usera nalezy odlogowac
        /// </summary>
        public DateTime LogoutTime { get; set; }

        /// <summary>
        /// Sprawdza, czy user jest zalogowany.
        /// </summary>
        /// <param name="_nick"></param>
        /// <returns></returns>
        public static bool UserIsLogon (string _nick)
        {
            if (string.IsNullOrEmpty(_nick))
                return false;

            if (!AllSesions.ContainsKey(_nick))
                return false;

            var user = AllSesions[_nick];

            //No to usuwamy drania, ktory smie byc nam null'em
            if(user == null)
            {
                AllSesions.Remove(_nick);
                return false;
            }


            //User jeszcze jest zalogowany i ma ku temu prawo
            if (DateTime.Compare(DateTime.Now, user.LogoutTime) < 0)
                return true;

            //No to precz
            LogOutUser(_nick);

            return false;
        }

        /// <summary>
        /// Zwraca sesje uzytkownika
        /// Jak nie ma, to null.
        /// </summary>
        /// <param name="_nick"></param>
        /// <returns></returns>
        public static UserSession GetSession(string _nick)
        {
            if (string.IsNullOrEmpty(_nick))
                return null;

            return (AllSesions.ContainsKey(_nick))? AllSesions[_nick] : null;
        }

        /// <summary>
        /// Wylogowuje usera
        /// </summary>
        /// <param name="_nick"></param>
        public static void LogOutUser(string _nick)
        {
            var user = GetSession(_nick);

            if (user == null)
                return;

            //By w razie czego odlogowac go przy sprawdzeniu
            user.LogoutTime = DateTime.MinValue;

            //Usuwamy ze slownika
            AllSesions.Remove(_nick);
        }

        /// <summary>
        /// Loguje usera
        /// </summary>
        /// <param name="_nick"></param>
        public static void LogInUser(string _nick)
        {
            var session = GetSession(_nick);

            //No to tworzymy sesje i dodajemy
            if(session == null)
            {
                session = new UserSession();
                session.Nick = _nick;
                AllSesions.Add(session.Nick, session);
            }

            //Ogarniamy czas.
            var timeOffset = new TimeSpan(0, 0, AutoLogOutTimeInMinutes, 0);
            session.LogoutTime = DateTime.Now.Add(timeOffset);
        }
    }
}