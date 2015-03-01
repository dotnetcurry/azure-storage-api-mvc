using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using EntityStores;
using mvcapp.Models;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace mvcapp.Controllers
{
    [Authorize]
    public class ProfileManagerController : Controller
    {
        BlobOperations blobOperations;
        TableOperations tableOperations;
        public ProfileManagerController()
        {
            blobOperations = new BlobOperations();
            tableOperations = new TableOperations(); 
        }
        // GET: ProfileManager
        public ActionResult Index()
        {
            var profiles = tableOperations.GetEntities(User.Identity.Name);
            return View(profiles);
        }

        public ActionResult Create()
        {
            var Profile = new ProfileEntity();
            Profile.ProfileId = new Random().Next(); //Generate the Profile Id Randomly
            Profile.Email = User.Identity.Name; // The Login Email
            ViewBag.Profession = new SelectList(new List<string>()
            {
               "Fresher","IT","Computer Hardware","Teacher","Doctor"
            });
            ViewBag.Qualification = new SelectList(new List<string>()
            {
               "Secondary","Higher Secondary","Graduate","Post-Graduate","P.HD"
            }); 
            return View(Profile);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(
               ProfileEntity obj,
          HttpPostedFileBase profileFile 
            )
        {

            CloudBlockBlob profileBlob = null;
            #region Upload File In Blob Storage
            //Step 1: Uploaded File in BLob Storage
            if (profileFile != null && profileFile.ContentLength != 0)
            {
                profileBlob = await blobOperations.UploadBlob(profileFile);
                obj.ProfilePath = profileBlob.Uri.ToString();
            }
            //Ends Here 
            #endregion

            #region Save Information in Table Storage
            //Step 2: Save the Infromation in the Table Storage

            //Get the Original File Size
            obj.Email = User.Identity.Name; // The Login Email
            obj.RowKey = obj.ProfileId.ToString();
            obj.PartitionKey = obj.Email;
            //Save the File in the Table
            tableOperations.CreateEntity(obj);
            //Ends Here 
            #endregion

             

            return RedirectToAction("Index");
        }
    }
}