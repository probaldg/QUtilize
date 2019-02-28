using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QBA.Qutilize.WebApp.DAL;
using System.Data;
using QBA.Qutilize.WebApp.Helper;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LoadSideMenu()
        {
            AccountViewModels obj = new Models.AccountViewModels();
            List<int> myList = (List<int>)Session["Modules"];
            string strMenu = string.Empty;
            string strMenuPart1 = string.Empty;
            string strMenuPart2 = string.Empty;
            string strMain = string.Empty;
            int i = 0;
            int x = 1;
            DataTable dt = obj.GetDashBoardMenu(Convert.ToInt32(Session["sessUser"]));
            foreach (DataRow dr in dt.Rows)
            {
                strMenu += "<div class='row lien'>" +
                           "<div class='col-md-12'>" +
                           "<a href='" + dr["URL"] + "' class='fa " + dr["DisplayCSS"].ToString() + "'></a> &nbsp;" +
                           "<a href='" + dr["URL"] + "'>" + dr["DisplayName"].ToString() + "</a>" +
                           "<hr>" +
                           "</div>" +
                           "</div>";
                //if (int.Parse(Session["sessUser"].ToString()) != 1)
                //{
                //    foreach (int list in myList)
                //    {
                //        if (int.Parse(dr["id"].ToString()) == list)
                //        {

                //            strMenu += "<div class='row lien'>" +
                //            "<div class='col-md-12'>" +
                //            "<a href='" + dr["URL"] + "' class='fa " + dr["DisplayCSS"].ToString() + "'></a> &nbsp;" +
                //            "<a href='" + dr["URL"] + "'>" + dr["DisplayName"].ToString() + "</a>" +
                //            "<hr>" +
                //            "</div>" +
                //            "</div>";
                //        }
                //    }
                //}
                //else
                //{
                //    strMenu += "<div class='row lien'>" +
                //             "<div class='col-md-12'>" +
                //             "<a href='" + dr["URL"] + "' class='fa " + dr["DisplayCSS"].ToString() + "'></a> &nbsp;" +
                //             "<a href='" + dr["URL"] + "'>" + dr["DisplayName"].ToString() + "</a>" +
                //             "<hr>" +
                //             "</div>" +
                //             "</div>";
                //}
                i++;
            }
            return Content(strMenu);
        }
    }
}