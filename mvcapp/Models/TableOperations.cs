using EntityStores;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Collections.Generic;
using System.Configuration;

namespace mvcapp.Models
{
    /// <summary>
    /// Interface Containing Operations for
    /// 1. Create Entity in Table => CreateEntity
    /// 2. Retrive Entities Based upon the Partition => GetEntities
    /// 3. Get Single Entity based upon partition Key and Row Key => GetEntity
    /// </summary>
    public interface ITableOperations
    {
        void CreateEntity(ProfileEntity entity);
        List<ProfileEntity> GetEntities(string filter);
        ProfileEntity GetEntity(string partitionKey, string rowKey);
       
    }

    public class TableOperations : ITableOperations
    {
        //Represent the Cloud Storage Account, this will be instantiated 
        //based on the appsettings
        CloudStorageAccount storageAccount;
        //The Table Service Client object used to 
        //perform operations on the Table
        CloudTableClient tableClient;

        /// <summary>
        /// COnstructor to Create Storage Account and the Table
        /// </summary>
        public TableOperations()
        {
            //Get the Storage Account from the conenction string
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["webjobstorage"]);
            //Create a Table Client Object
            tableClient = storageAccount.CreateCloudTableClient();

            //Create Table if it does not exist
            CloudTable table = tableClient.GetTableReference("ProfileEntityTable");
            table.CreateIfNotExists();
        }

        /// <summary>
        /// Method to Create Entity
        /// </summary>
        /// <param name="entity"></param>
        public void CreateEntity(ProfileEntity entity)
        {

            CloudTable table = tableClient.GetTableReference("ProfileEntityTable");
            //Create a TableOperation object used to insert Entity into Table
            TableOperation insertOperation = TableOperation.Insert(entity);
            //Execute an Insert Operation
            table.Execute(insertOperation);
        }
        /// <summary>
        /// Method to retrieve entities based on the PartitionKey
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public List<ProfileEntity> GetEntities(string filter)
        {
            List<ProfileEntity> Profiles = new List<ProfileEntity>();
            CloudTable table = tableClient.GetTableReference("ProfileEntityTable");

            TableQuery<ProfileEntity> query = new TableQuery<ProfileEntity>()
            .Where(TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, filter));


            foreach (var item in table.ExecuteQuery(query))
            {
                Profiles.Add(new ProfileEntity()
                {
                    ProfileId = item.ProfileId,
                    FullName = item.FullName,
                    Profession = item.Profession,
                    Qualification = item.Qualification,
                    University = item.University,
                    ContactNo = item.ContactNo,
                    Email = item.Email,
                    YearOfExperience = item.YearOfExperience,
                    ProfilePath = item.ProfilePath,
                    ProfessionalInformation = item.ProfessionalInformation
                });
            }

            return Profiles;
        }

        /// <summary>
        /// Method to get specific entity based on the Row Key and the Partition key
        /// </summary>
        /// <param name="partitionKey"></param>
        /// <param name="rowKey"></param>
        /// <returns></returns>
        public ProfileEntity GetEntity(string partitionKey, string rowKey)
        {
            ProfileEntity entity = null;

            CloudTable table = tableClient.GetTableReference("ProfileEntityTable");

            TableOperation tableOperation = TableOperation.Retrieve<ProfileEntity>(partitionKey, rowKey);
            entity = table.Execute(tableOperation).Result as ProfileEntity;

            return entity;
        }

    }
}