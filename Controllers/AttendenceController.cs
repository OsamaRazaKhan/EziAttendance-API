using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System;
using System.Linq;
using attendence_sys_api.Models;

public class AttendanceController : ApiController
{
    Attendence_Management_SystemEntities db = new Attendence_Management_SystemEntities();

    [Route("api/attendance/getattendances")]
    [HttpGet]
    public HttpResponseMessage GetAttendances(int id)
    {
        
        try
        { 
            var attendances = db.Attendances.Where(a=>a.UserID==id).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, attendances.Select(s => new { s.AttendanceID, s.UserID, s.Date, s.Status, s.LeaveRequest }));
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
    [Route("api/attendance/getreport")]
    [HttpGet]
    public HttpResponseMessage GetReport()
    {

        try
        {
            var attendances = db.Attendances.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, attendances.Select(s => new { s.AttendanceID, s.UserID, s.Date, s.Status, s.LeaveRequest }));
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    public HttpResponseMessage GetAttendance(int id)
    {
        try
        {
            var attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, attendance);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public HttpResponseMessage PostAttendance(Attendance attendance)
    {
        try
        {
            var existingattend = db.Attendances.SingleOrDefault(a => a.UserID == attendance.UserID && a.Date == attendance.Date);
            if (existingattend != null)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "marked");
            }
            db.Attendances.Add(attendance);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, attendance);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    public HttpResponseMessage PutAttendance(int id, Attendance attendance)
    {
        if (id != attendance.AttendanceID)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        try
        {
            db.Entry(attendance).State = EntityState.Modified;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!AttendanceExists(id))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }

    [HttpDelete]
    public HttpResponseMessage DeleteAttendance(int id)
    {
        try
        {
            var attendance = db.Attendances.Find(id);
            if (attendance == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Attendances.Remove(attendance);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private bool AttendanceExists(int id)
    {
        return db.Attendances.Any(e => e.AttendanceID == id);
    }
}
