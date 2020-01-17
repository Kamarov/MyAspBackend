using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyASPBackend.Models;

namespace MyASPBackend.Controllers
{
    public class HomeController : Controller
    {
        static readonly string TempData_NewUserKey = "newuser";

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Strona rejestracji
        /// </summary>
        /// <returns></returns>
        public ActionResult RegistrationPage(Registration id)
        {
            return View();
        }
        /// <summary>
        /// Zwraca dane o sesji.
        /// </summary>
        /// <param name="Nick"></param>
        /// <returns></returns>
        public JsonResult UserStatus(string Nick)
        {
            return Json(Models.UserSession.GetSession(Nick));
        }

        public ActionResult UserPage(string Nick)
        {
            ViewBag.RequestedUser = Models.UserSession.GetSession(Nick);

            return View();
        }


        public ActionResult SignIn()
        {
            return View();
        }

        /// <summary>
        /// Loguje na stronie wg. danych logowania
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SignIn(LoginData id)
        {
            return View();
        }

        /// <summary>
        /// Probuje zalogowac usera i zwraca informacje w postaci jsona z polem
        /// OperationResult
        /// </summary>
        /// <param name="_loginData"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TryLogIn(LoginData _loginData)
        {
            if (!Models.User.UserExists(_loginData.Nick))
                return Json(new { OperationResult = "UserNotExists" });

            //Przy tym zapytaniu od razu przedluzymy mu sesje logowania
            if (Models.UserSession.UserIsLogon(_loginData.Nick))
            {
                Models.UserSession.LogInUser(_loginData.Nick);
                return Json(new { OperationResult = "UserAlreadyLogon" });
            }

            //Logujemy usera
            Models.UserSession.LogInUser(_loginData.Nick);

            //Zwracamy informacje, ze udalo sie nam zalogowac usera
            return Json(new { OperationResult = "UserLogged" });
        }

        /// <summary>
        /// Odlogowuje usera o podanym nicku.
        /// Zwraca jsona z polem OperationResult
        /// </summary>
        /// <param name="_nick"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult TryLogOut(string Nick)
        {
            if(!Models.UserSession.UserIsLogon(Nick))
                return Json(new { OperationResult = "UserAlreadyLoggedOut" });

            Models.UserSession.LogOutUser(Nick);

            return Json(new { OperationResult = "UserLoggedOut" });
        }

        /// <summary>
        /// Zwraca jsona z zapytania, czy user istnieje.
        /// </summary>
        /// <param name="Nick"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserExists(string Nick)
        {
            var toRet = Models.User.UserExists(Nick);
            return Json(toRet);
        }

        /// <summary>
        /// Zwraca jsona z zapytania, czy user jest zalogowany.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UserLogged(string id)
        {
            //Jak nie istnieje, to nie moze byc zalogowany.
            if (!Models.User.UserExists(id))
                return Json(false, JsonRequestBehavior.AllowGet);

            return Json(Models.UserSession.UserIsLogon(id), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Proba stworzenia usera
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TryRegister(Registration id)
        {
            if (Models.User.UserExists(id.Nick))
            {
                ModelState.AddModelError("", "User already exists!");
                return RedirectToAction("RegistrationPage", id);
            }

            if (!ModelState.IsValid)
                return RedirectToAction("RegistrationPage", id);

            

            var usr = Models.User.RegisterNewUser(id);

            //OK, wiec to tak ogarniasz...
            TempData[TempData_NewUserKey] = usr;

            return RedirectToAction("RegistrationSucces");
        }

        /// <summary>
        /// Gdy rejestracja sie udala
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RegistrationSucces()
        {
            var tmpPtr = (Models.User)TempData[TempData_NewUserKey];

            if(tmpPtr == null)
                ViewBag.NewUserNick = null;
            else
                ViewBag.NewUserNick = tmpPtr.Nick;
            
            TempData[TempData_NewUserKey] = null;
            return View();
        }

        /// <summary>
        /// Gdy rejestracja sie nie udala.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult RegistrationFailed(Registration id)
        {
            ViewBag.FailedRegistration = id;

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}