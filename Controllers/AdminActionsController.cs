
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System;
using System.Linq;
using attendence_sys_api.Models;

public class AdminActionsController : ApiController
{
    Attendence_Management_SystemEntities db = new Attendence_Management_SystemEntities();

    [HttpGet]
    public HttpResponseMessage GetAdminActions()
    {
        try
        {
            var adminActions = db.AdminActions.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, adminActions);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    public HttpResponseMessage GetAdminAction(int id)
    {
        try
        {
            var adminAction = db.AdminActions.Find(id);
            if (adminAction == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, adminAction);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public HttpResponseMessage PostAdminAction(AdminAction adminAction)
    {
        try
        {
            db.AdminActions.Add(adminAction);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, adminAction);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    public HttpResponseMessage PutAdminAction(int id, AdminAction adminAction)
    {
        if (id != adminAction.ActionID)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        try
        {
            db.Entry(adminAction).State = EntityState.Modified;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!AdminActionExists(id))
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
    public HttpResponseMessage DeleteAdminAction(int id)
    {
        try
        {
            var adminAction = db.AdminActions.Find(id);
            if (adminAction == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.AdminActions.Remove(adminAction);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private bool AdminActionExists(int id)
    {
        return db.AdminActions.Any(e => e.ActionID == id);
    }
}
