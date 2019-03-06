using QBA.Qutilize.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QBA.Qutilize.WebApp.Helper
{
    public class ModuleMappingHelper
    {
        public static bool IsUserMappedToModule(int userId, string moduleUrl)
        {
            bool IsExist = false;
            try
            {
                AccountViewModels obj = new Models.AccountViewModels();
                DataTable dt = obj.GetDashBoardMenu(userId);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        if (moduleUrl.ToLower().Contains(item["URL"].ToString().ToLower()))
                        {
                            IsExist = true;
                        }
                    }
                }

                return IsExist;
            }
            catch (Exception ex)
            {
                return IsExist;
            }

        }
    }
}