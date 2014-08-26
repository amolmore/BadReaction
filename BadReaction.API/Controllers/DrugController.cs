using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Http.Cors;
using BadReaction.API.Models;

namespace BadReaction.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DrugController : ApiController
    {
        // GET: api/Drug/5
        public List<string> Get(string query)
        {
            string ConnectionString = @"Data Source=(LocalDb)\v11.0;AttachDbFilename=E:\Projects\XMLParser\XMLParser\Database1.mdf;Initial Catalog=Database1;Integrated Security=True";
            SqlConnection con = new SqlConnection(ConnectionString);
            string SQL = "SELECT  DISTINCT TOP(10) DrugName FROM Drugs WHERE DrugName LIKE @SearchTerm ";
            using (SqlCommand cmd = new SqlCommand(SQL, con))
            {
                cmd.Parameters.Add("@SearchTerm", SqlDbType.Text);
                cmd.Parameters["@SearchTerm"].Value = "%" + query + "%";
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                List<string> drugNames = new List<string>();
                while (dr.Read())
                {
                    drugNames.Add(dr["DrugName"].ToString());
                }
                //var jsonData = Serialize(dr);
                //string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                //return json;

                return drugNames; 
            }

        }


        public List<Drug> GetDetails(string drugName)
        {
            List<Drug> DrugList = new List<Drug>();
            string ConnectionString = @"Data Source=(LocalDb)\v11.0;AttachDbFilename=E:\Projects\XMLParser\XMLParser\Database1.mdf;Initial Catalog=Database1;Integrated Security=True";
            SqlConnection con = new SqlConnection(ConnectionString);
            string SQL = "Select * FROM Drugs WHERE DrugName LIKE @drugName";
            using (SqlCommand cmd = new SqlCommand(SQL, con))
            {
                cmd.Parameters.Add("@drugName", SqlDbType.Text);
                cmd.Parameters["@drugName"].Value = drugName ;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Drug d = new Drug();
                    d.DrugName = dr["DrugName"].ToString();
                    d.ReactionName = dr["Reaction"].ToString();
                    DrugList.Add(d);
                }
                //var jsonData = Serialize(dr);
                //string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                //return json;
                return DrugList;
            }
        }

        public IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            var results = new List<Dictionary<string, object>>();
            var cols = new List<string>();
            for (var i = 0; i < reader.FieldCount; i++)
                cols.Add(reader.GetName(i));

            while (reader.Read())
                results.Add(SerializeRow(cols, reader));

            return results;
        }
        private Dictionary<string, object> SerializeRow(IEnumerable<string> cols,
                                                        SqlDataReader reader)
        {
            var result = new Dictionary<string, object>();
            foreach (var col in cols)
                result.Add(col, reader[col]);
            return result;
        }
    }
}
