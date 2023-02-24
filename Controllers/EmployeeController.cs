using PunchPad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace PunchPad;
public class EmployeeController : Controller
{
    private readonly ACE42023Context db;
    private readonly ISession session;
    public EmployeeController(ACE42023Context _db, IHttpContextAccessor httpContextAccessor)
    {
        db = _db;
        session = httpContextAccessor.HttpContext.Session;
    }


    [Route("Login")]
    public IActionResult Login(){
        ViewBag.LoginError = session.GetString("LoginError");
        return View();
    }


    [Route("Login")]
    [HttpPost]
    public IActionResult Login(PremEmployee emp)
    {
        PremEmployee? empInDB = db.PremEmployees.Where((e) => e.Username == emp.Username && e.Password == emp.Password).SingleOrDefault();
        if (empInDB != null)
        {
            var manages = db.PremEmployees.Where((i)=>i.ManagerId==empInDB.Id).ToList().Count;
            session.SetString("userId", empInDB.Id.ToString());
            session.SetString("userName", empInDB.Username);
            session.SetString("name", empInDB.Name);
            if(manages>0)
                session.SetString("isManager", "Yes");
            return RedirectToAction("Dashboard");
        }
        session.SetString("LoginError", "Login Failed - Invalid Username/Password");
        return RedirectToAction("Login");
    }


    [Route("Register")]
    public IActionResult Register(){
        ViewBag.Managers = new SelectList(db.PremEmployees.ToList(), "Id", "Name");
        ViewBag.RegisterError = session.GetString("RegisterError");
        return View();
    }


    [Route("Register")]
    [HttpPost]
    public IActionResult Register(PremEmployee emp)
    {
        PremEmployee? empInDB = db.PremEmployees.Where((e) => e.Username == emp.Username).SingleOrDefault();
        if (empInDB == null)
        {
            db.PremEmployees.Add(emp);
            db.SaveChanges();
            empInDB = db.PremEmployees.Where((e) => e.Username == emp.Username && e.Password == emp.Password).SingleOrDefault();
            session.SetString("userId", empInDB.Id.ToString());
            session.SetString("userName", emp.Username);
            session.SetString("name", emp.Name);
            return RedirectToAction("Dashboard");
        }
        session.SetString("RegisterError", $"Register Failed - username {emp.Username} is already used by another account.");
        return RedirectToAction("Register");
    }


    public IActionResult Logout()
    {
        session.Clear();
        return RedirectToAction("Index", "Home");
    }


    [Route("Profile")]
     public IActionResult Dashboard(){
        string Username=session.GetString("userName");
        if(Username==null)
            return RedirectToAction("Login");
        PremEmployee ?user=db.PremEmployees.Include(i=>i.Manager).Where((i)=>i.Username==Username).SingleOrDefault();
        return View(user);
    }


    [Route("Profile/{name}")]
     public IActionResult Dashboard(string name){
        string Username=session.GetString("userName");
        if(Username==null)
            return RedirectToAction("Login");
        PremEmployee ?user=db.PremEmployees.Include(i=>i.Manager).Where((i)=>i.Username==name).SingleOrDefault();
        return View(user);
    }


    [Route("Profile/Edit")]
     public IActionResult EditProfile(){
        string Username=session.GetString("userName");
        if(Username==null)
            return RedirectToAction("Login");
        PremEmployee ?user=db.PremEmployees.Include(i=>i.Manager).Where((i)=>i.Username==Username).SingleOrDefault();
        ViewBag.Managers = new SelectList(db.PremEmployees.Where((i)=>i.Username!=Username).ToList(), "Id", "Name");
        return View(user);
    }


    [Route("Profile/Edit")]
    [HttpPost]
     public IActionResult EditProfile(PremEmployee p){
        string Username=session.GetString("userName");
        int Id=Convert.ToInt16(session.GetString("userId"));
        if(Username==null)
            return RedirectToAction("Login");
        p.Id=Id;
        try{
            db.PremEmployees.Update(p);
            db.SaveChanges();
            if(Username!=p.Username){
                session.Clear();
                return RedirectToAction("Login");
            }
            return RedirectToAction("Dashboard");
        }catch(Exception e){
            ViewBag.UpdateError="This username is already used by another account.";
            ViewBag.Managers = new SelectList(db.PremEmployees.Where((i)=>i.Username!=Username).ToList(), "Id", "Name");
        }
        return View(p);
    }
}