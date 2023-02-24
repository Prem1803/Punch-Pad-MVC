using PunchPad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace PunchPad;
public class AttendanceController : Controller
{
    private readonly ACE42023Context db;
    private readonly ISession session;
    public AttendanceController(ACE42023Context _db, IHttpContextAccessor httpContextAccessor)
    {
        db = _db;
        session = httpContextAccessor.HttpContext.Session;
    }


     [Route("Punch")]
    public IActionResult Punch(){
        if(session.GetString("userName")==null)
            return RedirectToAction("Login","Employee");
        return View();
    }

    PremAttendance GenerateAttendance(string userName,string type){
        PremEmployee e = db.PremEmployees.Where((e)=>e.Username==userName).SingleOrDefault();
        PremAttendance attendance = new PremAttendance();
        attendance.EmpId=e.Id;
        attendance.ManagerId=e.ManagerId;
        attendance.AttendanceDay = DateTime.Now;
        attendance.AttendanceTime = DateTime.Now;
        attendance.AttendanceStatus = "Pending";
        attendance.AttendanceType = type;
        return attendance;
    }


    public IActionResult AddAttendance(string id){
        string ?userName=session.GetString("userName");
        if(userName==null)
            return RedirectToAction("Login","Employee");
        
        db.PremAttendances.Add(GenerateAttendance(userName,id));
        db.SaveChanges();
        session.SetString("Punch",$"Punched {id} Successfully.");
        return RedirectToAction("Punch");
    }

    public IActionResult Delete(int id){
        string ?userName=session.GetString("userName");
        if(userName==null)
            return RedirectToAction("Login","Employee");
        
        db.PremAttendances.Remove(db.PremAttendances.Find(id));
        db.SaveChanges();
        return RedirectToAction("DailyAttendance");
    }


    [Route("Attendance")]
    public IActionResult DailyAttendance(){
        string ?userId=session.GetString("userId");
        if(userId==null)
            return RedirectToAction("Login","Employee");
        ViewBag.date=DateTime.Today;
        var attendances = db.PremAttendances.Where((a)=>a.EmpId.ToString()==userId && a.AttendanceDay==DateTime.Today).ToList();
        return View(attendances);
    }


    [Route("Attendance")]
    [HttpPost]
    public IActionResult DailyAttendance(IFormCollection f){
        string ?userId=session.GetString("userId");
        if(userId==null)
            return RedirectToAction("Login","Employee");
        DateTime date=Convert.ToDateTime(f["Date"].ToString());
        ViewBag.date=date;
        var attendances = db.PremAttendances.Where((a)=>a.EmpId.ToString()==userId && a.AttendanceDay==date).ToList();
        return View(attendances);
    }


    [Route("Team-Attendance")]
    public IActionResult ManageTeamAttendance(){
        string ?userId=session.GetString("userId");
        if(userId==null)
            return RedirectToAction("Login","Employee");
        ViewBag.date=DateTime.Today;
        var attendances = db.PremAttendances.Include(a=>a.Emp).Where((a)=>a.ManagerId.ToString()==userId && a.AttendanceDay==DateTime.Today && a.AttendanceStatus=="Pending").ToList();
        return View(attendances);
    }


    [Route("Team-Attendance")]
    [HttpPost]
    public IActionResult ManageTeamAttendance(IFormCollection f){
        string ?userId=session.GetString("userId");
        if(userId==null)
            return RedirectToAction("Login","Employee");
        DateTime date=Convert.ToDateTime(f["Date"].ToString());
        string keyword = f["keyword"].ToString();
        ViewBag.date=date;
        var attendances = db.PremAttendances.Include(a=>a.Emp).Where((a)=>a.ManagerId.ToString()==userId && a.AttendanceDay==date && a.AttendanceStatus=="Pending" && a.Emp.Name.Contains(keyword)).ToList();
        ViewBag.attendances=attendances;
        ViewBag.keyword=keyword;
        return View(attendances);
    }


    public IActionResult UpdateAttendance(int id,string status){
        PremAttendance ?p = db.PremAttendances.Find(id);
        if(p!=null)
        p.AttendanceStatus = status;
        db.SaveChanges();
        return RedirectToAction("ManageTeamAttendance");
    }
}