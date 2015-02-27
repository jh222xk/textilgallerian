﻿using System.Web;
﻿using System.Web.Mvc;
﻿using Domain.Entities;

namespace AdminView.Annotations
{
    public class LoggedIn : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Controller.TempData.Add("error", "Du måste logga in!");
            filterContext.Result = new RedirectResult("~/");
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session == null) return false;

            var user = httpContext.Session["user"] as User;

            return user != null && user.IsActive;
        }
    }
}