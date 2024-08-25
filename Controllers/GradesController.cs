using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System;
using System.Linq;
using attendence_sys_api.Models;

public class GradesController : ApiController
{
    Attendence_Management_SystemEntities db = new Attendence_Management_SystemEntities();

    [HttpGet]
    public HttpResponseMessage GetGrades()
    {
        try
        {
            var grades = db.Grades.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, grades);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    public HttpResponseMessage GetGrade(int id)
    {
        try
        {
            var grade = db.Grades.Find(id);
            if (grade == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, grade);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public HttpResponseMessage PostGrade(Grade grade)
    {
        try
        {
            db.Grades.Add(grade);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, grade);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    public HttpResponseMessage PutGrade(int id, Grade grade)
    {
        if (id != grade.GradeID)
        {
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        try
        {
            db.Entry(grade).State = EntityState.Modified;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!GradeExists(id))
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
    public HttpResponseMessage DeleteGrade(int id)
    {
        try
        {
            var grade = db.Grades.Find(id);
            if (grade == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Grades.Remove(grade);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private bool GradeExists(int id)
    {
        return db.Grades.Any(e => e.GradeID == id);
    }
}
