using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebPrototypeV3.Models;
using System.Xml.XPath;
using Umbraco.Core.Services;
using Umbraco.Web.Mvc;



namespace WebPrototypeV3.Controllers
{
    public class ContactSurfaceController : SurfaceController
    {
        public ActionResult ShowForm()
        {
            ContactModel myModel = new ContactModel();
            //myModel.Email = CurrentPage.GetProperty("userEmail").ToString();
            List<SelectListItem> ListOfGenders = new List<SelectListItem>();
            XPathNodeIterator iterator = umbraco.library.GetPreValues(42);
            iterator.MoveNext();
            XPathNodeIterator preValues = iterator.Current.SelectChildren("preValue", "");
            while (preValues.MoveNext())
            {
                string preValue = preValues.Current.Value;
                ListOfGenders.Add(new SelectListItem
                {
                    Text = preValue,
                    Value = preValue
                });
            }
            myModel.ListOfGenders = ListOfGenders;
            return PartialView("ContactForm", myModel);
        }

        public ActionResult HandleFormPost(ContactModel model)
        {
            //var newComment = Services.ContentService.CreateContent(model.Id+" "+model.Name,CurrentPage.Id,"ContactFormula");
            //var newComment = Services.ContentService.CreateContent(model.Name + " - " + DateTime.Now, CurrentPage.Id, "ContactFormula");
            //google c# dateformat for more options
            var newComment = Services.ContentService.CreateContent(model.Name + " - " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"), CurrentPage.Id, "ContactFormula");
            // DataTypeService myService = new DataTypeService();
            // var SelectedGender = myService.GetAllDataTypeDefinitions().First(x => x.Id == 1099);
            //int SelectedGenderPreValueId = myService.GetPreValuesCollectionByDataTypeId(SelectedGender.Id).PreValuesAsDictionary.Where(x => x.Value.Value == model.SelectedGender).Select(x => x.Value.Id).First();

            //newComment.SetValue("dropdownGender",SelectedGenderPreValueId);
            newComment.SetValue("emailForm", model.Email);
            newComment.SetValue("contactName", model.Name);
            newComment.SetValue("contactMessage", model.Message);
            Services.ContentService.SaveAndPublishWithStatus(newComment);
            return RedirectToCurrentUmbracoPage();
        }
    }
}
