
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;
    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet("")]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost("register")]
        public IActionResult TryRegister(IndexViewModel modelData)
        {
            User regUser = modelData.RegUser;
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == regUser.Email))
                {
                    ModelState.AddModelError("RegUser.Email", "Email already in use!");
                }
                else
                {
                    PasswordHasher<User> Hasher = new PasswordHasher<User>();
                    regUser.Password = Hasher.HashPassword(regUser, regUser.Password);
                    _context.Add(regUser);
                    _context.SaveChanges();
                    User currUser = _context.Users.FirstOrDefault(u => u.Email == regUser.Email);
                    HttpContext.Session.SetInt32("userId", regUser.UserId);
                    return RedirectToAction("Dashboard");
                }
            }
            return View("Index", modelData);
        }

        [HttpPost("login")]
        public IActionResult TryLogin(IndexViewModel modelData)
        {
            LoginUser logUser = modelData.LogUser;
            if(ModelState.IsValid)
            {
                var userInDb = _context.Users.FirstOrDefault(u => u.Email == logUser.Email);

                if (userInDb == null)
                {
                    ModelState.AddModelError("LogUser.Email", "Invalid Email/Password");
                }
                else
                {
                    var hasher = new PasswordHasher<LoginUser>();
                    var result = hasher.VerifyHashedPassword(logUser, userInDb.Password, logUser.Password);

                    if (result == 0)
                    {
                        ModelState.AddModelError("LogUser.Email", "Invalid Email/Password");
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("userId", userInDb.UserId);
                        return RedirectToAction("Dashboard");
                    }
                }
            }
            return View("Index", modelData);
        }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Index");
            }

            List<Wedding> EveryWedding = _context.Weddings
                .Include(w => w.WeddingAttendees)
                .ThenInclude(a => a.User)
                .ToList();

            ViewBag.AllWeddings = EveryWedding;
            ViewBag.UserId = (int)HttpContext.Session.GetInt32("userId");
            return View("Dashboard");
        }

        [HttpGet("delete/{weddId}")]
        public IActionResult DeleteWedding(int weddId)
        {
            Wedding weddToDelete = _context.Weddings.FirstOrDefault(w => w.WeddingId == weddId);
            _context.Weddings.Remove(weddToDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("rsvp/{weddId}")]
        public IActionResult YesWedding(int weddId)
        {
            Attendance attendance = new Attendance();
            attendance.UserId = (int)HttpContext.Session.GetInt32("userId");
            attendance.WeddingId = weddId;
            _context.Attendances.Add(attendance);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("unrsvp/{attId}")]
        public IActionResult NoWedding(int attId)
        {
            Attendance attendance = _context.Attendances.FirstOrDefault(a => a.AttendanceId == attId);
            _context.Attendances.Remove(attendance);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("NewWedding")]
        public IActionResult NewWedding()
        {
            if (HttpContext.Session.GetInt32("userId") == null)
            {
                return RedirectToAction("Index");
            }
            return View("NewWedding");
        }

        [HttpPost("CreateWedding")]
        public IActionResult CreateWedding(Wedding newWedding)
        {
            if(ModelState.IsValid)
            {
                newWedding.PlannerId = (int)HttpContext.Session.GetInt32("userId");
                _context.Add(newWedding);
                _context.SaveChanges();
                Wedding thisWedding = _context.Weddings.OrderByDescending(w => w.CreatedAt).FirstOrDefault();
                return Redirect("/Wedding/"+thisWedding.WeddingId);
            }
            return View("NewWedding", newWedding);
        }

        [HttpGet("Wedding/{weddId}")]
        public IActionResult WeddInfo(int weddId)
        {
            Wedding thisWedding = _context.Weddings.FirstOrDefault(w => w.WeddingId == weddId);
            ViewBag.ThisWedding = thisWedding;

            var weddingGuests = _context.Weddings
                .Include(w => w.WeddingAttendees)
                .ThenInclude(u => u.User)
                .FirstOrDefault(w => w.WeddingId == weddId);
            
            ViewBag.AllGuests = weddingGuests.WeddingAttendees;
            return View("WeddInfo");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
