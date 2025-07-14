using DatabaseManager;
using MobilePortalManagementLibrary.Interface;
using MobilePortalManagementLibrary.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePortalManagementLibrary.Implementation
{
    public class ApiAuditManagement: IApiAuditManagement
    {
        public async Task<ResponseModel> CreateUpdateApiAudit(ApiAuditRequest aar)
        {
            ResponseModel response = new ResponseModel();
            try
            {

                ArrayList arrList = new ArrayList();
                

                    DALOR.spArgumentsCollection(arrList, "@p_flag", aar.flag, "Char", "I", 1);
                    
                    DALOR.spArgumentsCollection(arrList, "p_empid", aar.EmpId ?? "", "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_id", aar.Id != null ? aar.Id.ToString() : "0", "INT", "I");
                    DALOR.spArgumentsCollection(arrList, "p_apiname", aar.ApiName, "VARCHAR", "I");
                    DALOR.spArgumentsCollection(arrList, "p_request", aar.Request ?? "", "CLOB", "I");
                    DALOR.spArgumentsCollection(arrList, "p_response", aar.Response ?? "", "CLOB", "I");
                    


                    DALOR.spArgumentsCollection(arrList, "@ret", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "@errormsg", "", "VARCHAR", "O");
                    DALOR.spArgumentsCollection(arrList, "g_ResultSet", "", "REFCURSOR", "O");
                    var res = DALOR.RunStoredProcedureDsRetError("G_SP_API_AUDIT", arrList);
                    response.code = res.Ret;
                    response.msg = res.ErrorMsg;
                

            }
            catch (Exception ex)
            {
                response.code = -1;
                response.data = ex.Message;
            }
            return await Task.FromResult(response);
        }
    }
}
