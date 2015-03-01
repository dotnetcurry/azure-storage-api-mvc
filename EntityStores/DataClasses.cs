using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;

namespace EntityStores
{
    public class ProfileEntity : TableEntity
    {
        public ProfileEntity()
        {

        }

        public ProfileEntity(int profid, string email)
        {
            this.RowKey = profid.ToString();
            this.PartitionKey = email;
        }


        public int ProfileId { get; set; }
        [Required(ErrorMessage="FullName is Must")]
        public string FullName { get; set; }

        public string Profession { get; set; }
        public string Qualification { get; set; }
        [Required(ErrorMessage = "University is Must")]
        public string University { get; set; }
        [Required(ErrorMessage = "ContactNo is Must")]
        public string ContactNo { get; set; }
        [Required(ErrorMessage = "Email is Must")]
        public string Email { get; set; }
        public int YearOfExperience { get; set; }
        public string ProfessionalInformation { get; set; }
        public string ProfilePath { get; set; }

    }

     



    //public class BlobInfo
    //{

    //    public int ProfileId { get; set; }
    //    public Uri uriBlob { get; set; }

    //    public string Profession { get; set; }

    //    public string BLOBName
    //    {
    //        get
    //        {
    //            return uriBlob.Segments[uriBlob.Segments.Length - 1];
    //        }
    //    }
    //    public string BlobNameWithNoExtension
    //    {
    //        get
    //        {
    //            return Path.GetFileNameWithoutExtension(BLOBName);
    //        }
    //    }

    //}
    
}
