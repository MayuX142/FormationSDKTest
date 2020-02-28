using DocuWare.Platform.ServerClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace FormationSDKTest
{
    /// <summary>
    /// Description résumée de FormationSDK
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Pour autoriser l'appel de ce service Web depuis un script à l'aide d'ASP.NET AJAX, supprimez les marques de commentaire de la ligne suivante. 
    // [System.Web.Script.Services.ScriptService]
    public class FormationSDK : System.Web.Services.WebService
    {
        static Uri uri = new Uri("https://inovera.docuware.cloud/DocuWare/Platform");
        static string FCIDocs = "275da5d4-d81d-4627-a655-5cba8e126a58";
        static string FCIFolder = "fe565e84-2e0d-448e-9d56-62672b9cb094";
        private static ServiceConnection Connect()
        {
            return ServiceConnection.Create(uri, "admin", "Inovera36");
        }

        [WebMethod]
        public string CreateFolder(string DocumentType)
        {
            ServiceConnection sc = Connect();

            FileCabinet fcDocs = sc.GetFileCabinet(FCIDocs);

            List<Document> docs = getProjectDocs(fcDocs, DocumentType);

            sc.Disconnect();

            return "" + docs.Count;
        }

       
        private List<Document> getProjectDocs (FileCabinet fc, string DocumentType)
        {
            var result = new List<Document>();
            var dialogInfoItems = fc.GetDialogInfosFromSearchesRelation();
            var dialog = dialogInfoItems.Dialog[0].GetDialogFromSelfRelation();

            var q = new DialogExpression()
            {
                Operation = DialogExpressionOperation.And,
                Condition = new List<DialogExpressionCondition>()
                {
                    DialogExpressionCondition.Create("DOCUMENT_TYPE", DocumentType )
                },
                Count = 100,
                SortOrder = new List<SortedField>
                {
                    SortedField.Create("DWSTOREDATETIME", SortDirection.Desc)
                }
            };

            var queryResult = dialog.GetDocumentsResult(q);

            result = queryResult.Items;

            return result;
        }


        /*[WebMethod]
        public string HelloDW()
        {
            ServiceConnection sc = Connect();
            string result = Connect().Organizations.First().Name;

            sc.Disconnect();

            return result;
            
            //return Connect().Organizations.First().Name;
        }*/
    }
}
