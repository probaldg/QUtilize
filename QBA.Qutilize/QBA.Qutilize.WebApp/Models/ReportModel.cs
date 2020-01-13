using QBA.Qutilize.WebApp.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace QBA.Qutilize.WebApp.Models
{
    public class ReportModel
    {
        public int ReportType { get; set; }
    }
}