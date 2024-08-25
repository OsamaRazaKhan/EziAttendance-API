using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System;
using System.Linq;
using attendence_sys_api.Models;

public class LeaveRequestsController : ApiController
{
    Attendence_Management_SystemEntities db = new Attendence_Management_SystemEntities();

    [Route("api/leaverequests/getleaverequests")]
    [HttpGet]
    public HttpResponseMessage GetLeaveRequests()
    {
        try
        {
            var leaveRequests = db.LeaveRequests.Where(l=>l.Status== "pending").ToList();
            return Request.CreateResponse(HttpStatusCode.OK, leaveRequests);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    public HttpResponseMessage GetLeaveRequest(int id)
    {
        try
        {
            var leaveRequest = db.LeaveRequests.Find(id);
            if (leaveRequest == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, leaveRequest);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }



    [HttpPost]
    public HttpResponseMessage PostLeaveRequest(LeaveRequest leaveRequest)
    {
        try
        {
            var existingleaverequest = db.LeaveRequests.SingleOrDefault(l => l.UserID == leaveRequest.UserID && l.LeaveDate == leaveRequest.LeaveDate);
            if (existingleaverequest != null)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "requested");
            }
            db.LeaveRequests.Add(leaveRequest);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, leaveRequest);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
    [Route("api/leaverequests/putleaverequest")]
    [HttpPut]
    public HttpResponseMessage PutLeaveRequest( LeaveRequest leaveRequest)
    {

        try
        {
            db.Entry(leaveRequest).State = EntityState.Modified;
            db.SaveChanges();

            if(leaveRequest.Status== "Approved")
            {
                Attendance atd = new Attendance();
                atd.UserID = leaveRequest.UserID;
                atd.Date = leaveRequest.LeaveDate;
                atd.Status = "Leave";
                atd.LeaveRequest = true;
                db.Attendances.Add(atd);
                db.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.NoContent);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }
            
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!LeaveRequestExists(leaveRequest.LeaveRequestID))
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
    public HttpResponseMessage DeleteLeaveRequest(int id)
    {
        try
        {
            var leaveRequest = db.LeaveRequests.Find(id);
            if (leaveRequest == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.LeaveRequests.Remove(leaveRequest);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private bool LeaveRequestExists(int id)
    {
        return db.LeaveRequests.Any(e => e.LeaveRequestID == id);
    }
}
