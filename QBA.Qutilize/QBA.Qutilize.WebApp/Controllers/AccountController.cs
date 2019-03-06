﻿using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QBA.Qutilize.WebApp.DAL;
using System.Data;
using QBA.Qutilize.WebApp.Helper;
using System.Configuration;
using System.Text;

namespace QBA.Qutilize.WebApp.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult LoadSideMenu()
        {
            AccountViewModels obj = new Models.AccountViewModels();
            StringBuilder strMenu =new StringBuilder();
            DataTable dt = obj.GetDashBoardMenu(Convert.ToInt32(Session["sessUser"]));
            if (dt != null && dt.Rows.Count > 0)
            {
                try {
                    strMenu.Append(GetSideMenuContent(dt,""));
                }
                catch (Exception exx) { }
            }
            return Content(strMenu.ToString());
        }
        public string GetSideMenuContent(DataTable orgDT, string parentID)
        {
            StringBuilder strMenu = new StringBuilder();
            if (parentID.Trim() == "")
            {
                DataRow[] drParentAll = orgDT.Select("lvl = 0");
                if (drParentAll.Length > 0)
                {
                    foreach (DataRow dr in drParentAll)
                    {
                        strMenu.Append(GetSideMenuContent(orgDT, Convert.ToString(dr["ID"])));
                    }
                }
            }
            else
            {
                DataRow[] drParent = orgDT.Select("ID = " + parentID);
                if (drParent.Length > 0)
                {
                    strMenu.Append("<div class='row lien'>");
                    if (Convert.ToInt32(drParent[0]["lvl"]) == 0)
                        strMenu.Append("<div class='col-md-12'>");
                    else
                    {
                        int lvl = Convert.ToInt32(drParent[0]["lvl"]);
                        strMenu.Append("<div class='col-md-12' style='padding-left: " + (lvl * 35) + "px;'>");
                    }
                    strMenu.Append("<a href='" + drParent[0]["URL"] + "' class='fa " + drParent[0]["DisplayCSS"].ToString() + "'></a> &nbsp;");
                    strMenu.Append(" <a href = '" + ConfigurationSettings.AppSettings["SiteAddress"] + drParent[0]["URL"] + "'> " + drParent[0]["DisplayName"].ToString() + " </a> ");
                    strMenu.Append("<hr>");
                    strMenu.Append("</div>");
                    strMenu.Append("</div>");
                }
                DataRow[] dtChild = orgDT.Select("ParentID = " + parentID);
                foreach (DataRow dr in dtChild)
                {
                    strMenu.Append(GetSideMenuContent(orgDT, Convert.ToString(dr["ID"])));
                }
            }
            return strMenu.ToString();
        }

        public ActionResult MyAccount()
        {

            if (System.Web.HttpContext.Current.Session["sessUser"] != null)
            {
                UserModel user = new UserModel();
                try
                {
                    DataTable dt = user.GetMyAccountData(int.Parse(System.Web.HttpContext.Current.Session["sessUser"].ToString()));
                    if (dt.Rows.Count > 0)
                    {
                        user.ID = int.Parse(dt.Rows[0]["Id"].ToString());
                        user.Name = dt.Rows[0]["Name"].ToString();
                       
                        user.EmailId = dt.Rows[0]["EmailId"].ToString();
                      

                        ViewBag.ID = int.Parse(dt.Rows[0]["ID"].ToString());
                        ViewBag.Name = dt.Rows[0]["Name"].ToString();
                        ViewBag.Email = dt.Rows[0]["EmailId"].ToString();
                        //  ViewBag.OrgName = dt.Rows[0]["orgname"].ToString();
                        // ViewBag.Designation = dt.Rows[0]["Designation"].ToString();
                    }
                    return View(user);
                }
                catch (Exception)
                {

                    throw;
                }
               
            }
            else
            {
                return Redirect("/Home/Index");
            }
        }
        public JsonResult UpdatePassword(int id, string password)
        {
            UserModel user = new UserModel();
            try
            {
                password = EncryptionHelper.ConvertStringToMD5(password);

                int editedBy = int.Parse(Session["sessUser"].ToString());
                DateTime editedTS = DateTime.Now;

                DataTable dt = user.updatePassword(id, password, editedBy, editedTS);
            }
            catch (Exception)
            {

                //throw;
            }
           
            return Json("updated");
        }

    }
}