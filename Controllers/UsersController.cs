using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using System;
using System.Linq;
using attendence_sys_api.Models;

public class UsersController : ApiController
{
    Attendence_Management_SystemEntities db = new Attendence_Management_SystemEntities();

    [Route("api/users/login")]
    [HttpGet]
    public HttpResponseMessage Login(string email, string password)
    {
        try
        {
            var user = db.Users.SingleOrDefault(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid credentials");
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPut]
    public HttpResponseMessage Putuser(User user)
    {

        try
        {
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!UserExists(user.UserID))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }

    [Route("api/users/getusers")]
    [HttpGet]
    public HttpResponseMessage GetUsers()
    {
        try
        {
            var users = db.Users.Where(u=>u.Role== "Student").ToList();
            return Request.CreateResponse(HttpStatusCode.OK, users.Select(u => new {u.UserID, u.Username, u.FullName, u.Email, u.Password, u.ProfilePicture, u.Role  }));
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpGet]
    public HttpResponseMessage GetUser(int id)
    {
        try
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            return Request.CreateResponse(HttpStatusCode.OK, user);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public HttpResponseMessage PostUser(User user)
    {
        try
        {
            var existingUser = db.Users.SingleOrDefault(u => u.Username == user.Username || u.Email == user.Email);
            if (existingUser != null)
            {
                return Request.CreateResponse(HttpStatusCode.Conflict, "Username or Email already exists.");
            }

            db.Users.Add(user);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }


    [HttpDelete]
    public HttpResponseMessage DeleteUser(int id)
    {
        try
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Users.Remove(user);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.NoContent);
        }
        catch (Exception ex)
        {
            return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    private bool UserExists(int id)
    {
        return db.Users.Any(e => e.UserID == id);
    }
}
