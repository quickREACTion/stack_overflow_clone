using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CoderHelp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace CoderHelp.Controllers
{
    public class HomeController : Controller
    {
        private UserContext _context;

        public HomeController(UserContext context) {
            _context = context;
        }


        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("Register")]
        public IActionResult Register(UsersValidator userV) {
            Users newUser = new Users();
            newUser.Username = userV.Username;
            newUser.Email = userV.Email;
            newUser.Password = userV.Password;

            if(ModelState.IsValid) {
                newUser.created_at = DateTime.Now;
                newUser.updated_at = DateTime.Now;
                newUser.Employment = null;
                newUser.Education = null;
                newUser.Experience = null;
                newUser.Bio = null;
                PasswordHasher<Users> Hasher = new PasswordHasher<Users>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("user_id", newUser.usersId);
                return RedirectToAction("Dashboard");
            } else {
                return View("Index");
            }
        }            

        [Route("Login")]
        public IActionResult Login() {
            
            return View("Login");
        }

        [HttpPost("Login")]
        public IActionResult Login(string Email, string Password)
        {
            Users currentUser = _context.users.Where(cu => cu.Email == Email).SingleOrDefault();
            // Attempt to retrieve a user from your database based on the Email submitted
            if(currentUser != null && Password != null)
            {
                var Hasher = new PasswordHasher<Users>();
                // Pass the user object, the hashed password, and the PasswordToCheck
                if(0 != Hasher.VerifyHashedPassword(currentUser, currentUser.Password, Password))
                {
                    HttpContext.Session.SetInt32("user_id", currentUser.usersId);
                    var newSession = HttpContext.Session.GetInt32("user_id");
                    System.Console.WriteLine(newSession);
                    return RedirectToAction("Dashboard");
                }
            }
            return View("Index");
        }

        [Route("Dashboard")]
        public IActionResult Dashboard(int fullpageHiddenId) {
            var allUsers = _context.users.SingleOrDefault(name => name.usersId == HttpContext.Session.GetInt32("user_id"));
            ViewBag.user = allUsers;

            List<Messages> allmessages = _context.messages.Include(cm => cm.Comments).ToList();
            ViewBag.allMessages = allmessages;
            

            return View("Dashboard");
        }

        [Route("Create")]
        public IActionResult Create()
        {
            var allUsers = _context.users.SingleOrDefault(name => name.usersId == HttpContext.Session.GetInt32("user_id"));
            ViewBag.user = allUsers;

            return View("Create");
        }

        [HttpPost("createMessage")]
        public IActionResult createMessage(MessageValidator newM) {
            Users currentUser = _context.users.SingleOrDefault(cur => cur.usersId == HttpContext.Session.GetInt32("user_id")); 
            Messages message = new Messages();
            message.usersId = currentUser.usersId;
            message.Message = newM.Message;
            message.Description = newM.Description;
            message.CodeSnippit = newM.CodeSnippit;
            message.Bug = newM.Bug;

            if(ModelState.IsValid) {
                message.created_at = DateTime.Now;
                message.updated_at = DateTime.Now;
                _context.messages.Add(message);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            } else {
                return RedirectToAction("Create");
            }

        }

        [Route("FullPage/{fullpageHiddenId}")]
        public IActionResult FullPage(int fullpageHiddenId) {
            var allUsers = _context.users.SingleOrDefault(name => name.usersId == HttpContext.Session.GetInt32("user_id"));
            ViewBag.user = allUsers;

            Messages currentMessage = _context.messages.SingleOrDefault(p => p.messagesId == fullpageHiddenId);
            ViewBag.curMessage = currentMessage;

            List<Messages> allComments = _context.messages.Include(cm => cm.Comments).ThenInclude(a => a.User).Include(u => u.User).Where(m => m.messagesId == fullpageHiddenId).ToList();
            ViewBag.ac = allComments;

            return View("FullPage");
        }

        [HttpPost("Answer")]
        public IActionResult Answer(Comments createComment, int mid) {
            createComment.usersId = (int)HttpContext.Session.GetInt32("user_id");
            createComment.messagesId = mid;
            createComment.created_at = DateTime.Now;
            createComment.updated_at = DateTime.Now;
            _context.comments.Add(createComment);
            _context.SaveChanges();

            return Redirect("FullPage/" + mid);
        }

        [Route("Profile/{id}")]
        public IActionResult Profile(int id) {
            var curUser = _context.users.SingleOrDefault(name => name.usersId == id);
            ViewBag.cu = curUser;

            var currentUser = _context.users.SingleOrDefault(n => n.usersId == HttpContext.Session.GetInt32("user_id"));
            ViewBag.cuser = currentUser;

            bool isUser = false;
            if (curUser == currentUser) {
                isUser = true;
            }
            ViewBag.isUser = isUser;

            List<Users> allUsers = _context.users.ToList();
            ViewBag.au = allUsers;   

            List<Messages> allmessages = _context.messages.Include(cm => cm.Comments).Include(u => u.User).ToList();
            ViewBag.allMessages = allmessages;

            return View("Profile");
        }

        [Route("Edit")]
        public IActionResult Edit(int edituserEmp) {
            var allUsers = _context.users.SingleOrDefault(name => name.usersId == HttpContext.Session.GetInt32("user_id"));
            ViewBag.user = allUsers;

            return View("Edit");
        }

        [HttpPost("EditProfile")]
        public IActionResult EditProfile(UsersValidator updateUser, int edituserEmp) {
            var allUsers = _context.users.SingleOrDefault(name => name.usersId == edituserEmp);
            ViewBag.user = allUsers;

            Users RetrievedUser = _context.users.SingleOrDefault(user => user.usersId == edituserEmp);
            ViewBag.grabUser = RetrievedUser;

            RetrievedUser.Employment = updateUser.Employment;
            RetrievedUser.Education = updateUser.Education;
            RetrievedUser.Bio = updateUser.Bio;
            RetrievedUser.Experience = updateUser.Experience;
            RetrievedUser.updated_at = DateTime.Now;
            _context.SaveChanges();
            return RedirectToAction("Profile", new {id = edituserEmp});
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
