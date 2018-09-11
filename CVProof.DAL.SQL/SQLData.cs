using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using CVProof.Models;

namespace CVProof.DAL.SQL
{
    public static class SQLData
    {
        public static string connectionString { get; set; }

        public static List<HeaderModel> GetHeaders()
        {
            List<HeaderModel> ret = new List<HeaderModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "select " +
                                    "[Id]," +
                                    "[Category]," +
                                    "[IssuerName]," +
                                    "[ValidatorName]," +
                                    "[IssuerUUID]," +
                                    "[ValidatorLegitimationHeaderID]," +
                                    "[RecipientName]," +
                                    "[RecipientUUID]," +
                                    "[PreviousHeaderID]," +
                                    "[ValidationCounter]," +
                                    "[NextHeaderID]," +
                                    "[Timestamp]," +
                                    "[BlockNumber]," +
                                    "[DataAddress]," +
                                    "[ValidationExpiry]," +
                                    "[DataHash]," +
                                    "[Nonce]" +
                                    " from Header";
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ret.Add(new HeaderModel()
                        {
                            HeaderId = reader.GetString(0),
                            Category = reader.GetString(1),
                            IssuerName = reader.GetString(2),
                            ValidatorName = reader.GetString(3),
                            IssuerUuid = reader.GetString(4),
                            ValidatorUuid = reader.GetString(5),
                            RecipientName = reader.GetString(6),
                            RecipientUuid = reader.GetString(7),
                            PreviousHeaderId = reader.GetString(8),
                            ValidationCounter = reader.GetString(9),
                            NextHeaderId = reader.GetString(10),
                            Timestamp = reader.GetString(11),
                            BlockNumber = reader.GetString(12),
                            DataAddress = reader.GetString(13),
                            ValidationExpiry = reader.GetString(14),
                            DataHash = reader.GetString(15),
                            Nonce = reader.GetString(16)
                        });
                    }
                }
            }
            return ret;
        }

        public static HeaderModel GetHeaderById(string id)
        {
            HeaderModel ret = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "select " +
                                    "[Id]," +
                                    "[Category]," +
                                    "[IssuerName]," +
                                    "[ValidatorName]," +
                                    "[IssuerUUID]," +
                                    "[ValidatorLegitimationHeaderID]," +
                                    "[RecipientName]," +
                                    "[RecipientUUID]," +
                                    "[PreviousHeaderID]," +
                                    "[ValidationCounter]," +
                                    "[NextHeaderID]," +
                                    "[Timestamp]," +
                                    "[BlockNumber]," +
                                    "[DataAddress]," +
                                    "[ValidationExpiry]," +
                                    "[DataHash]," +
                                    "[Nonce]" +
                                    " from [dbo].[Header]" +
                                    "where id = @id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ret = new HeaderModel()
                        {
                            HeaderId = reader.GetString(0),
                            Category = reader.GetString(1),
                            IssuerName = reader.GetString(2),
                            ValidatorName = reader.GetString(3),
                            IssuerUuid = reader.GetString(4),
                            ValidatorUuid = reader.GetString(5),
                            RecipientName = reader.GetString(6),
                            RecipientUuid = reader.GetString(7),
                            PreviousHeaderId = reader.GetString(8),
                            ValidationCounter = reader.GetString(9),
                            NextHeaderId = reader.GetString(10),
                            Timestamp = reader.GetString(11),
                            BlockNumber = reader.GetString(12),
                            DataAddress = reader.GetString(13),
                            ValidationExpiry = reader.GetString(14),
                            DataHash = reader.GetString(15),
                            Nonce = reader.GetString(16)
                        };
                    }
                }
            }
            return ret;
        }

        public static HeaderModel GetHeaderByData(string datahash) // Temp function till the smart contract interface implemented
        {
            HeaderModel ret = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "select " +
                                    "[Id]," +
                                    "[Category]," +
                                    "[IssuerName]," +
                                    "[ValidatorName]," +
                                    "[IssuerUUID]," +
                                    "[ValidatorLegitimationHeaderID]," +
                                    "[RecipientName]," +
                                    "[RecipientUUID]," +
                                    "[PreviousHeaderID]," +
                                    "[ValidationCounter]," +
                                    "[NextHeaderID]," +
                                    "[Timestamp]," +
                                    "[BlockNumber]," +
                                    "[DataAddress]," +
                                    "[ValidationExpiry]," +
                                    "[DataHash]," +
                                    "[Nonce]" +
                                    " from [dbo].[Header]" +
                                    "where [DataHash] = @datahash";

                cmd.Parameters.AddWithValue("@datahash", datahash);
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ret = new HeaderModel()
                        {
                            HeaderId = reader.GetString(0),
                            Category = reader.GetString(1),
                            IssuerName = reader.GetString(2),
                            ValidatorName = reader.GetString(3),
                            IssuerUuid = reader.GetString(4),
                            ValidatorUuid = reader.GetString(5),
                            RecipientName = reader.GetString(6),
                            RecipientUuid = reader.GetString(7),
                            PreviousHeaderId = reader.GetString(8),
                            ValidationCounter = reader.GetString(9),
                            NextHeaderId = reader.GetString(10),
                            Timestamp = reader.GetString(11),
                            BlockNumber = reader.GetString(12),
                            DataAddress = reader.GetString(13),
                            ValidationExpiry = reader.GetString(14),
                            DataHash = reader.GetString(15),
                            Nonce = reader.GetString(16)
                        };
                    }
                }
            }
            return ret;
        }

        public static void InsertHeader(HeaderModel header)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into [dbo].[Header] ( " +
                                "[Id]," +
                                "[Category]," +
                                "[IssuerName]," +
                                "[ValidatorName]," +
                                "[IssuerUUID]," +
                                "[ValidatorLegitimationHeaderID]," +
                                "[RecipientName]," +
                                "[RecipientUUID]," +
                                "[PreviousHeaderID]," +
                                "[ValidationCounter]," +
                                "[NextHeaderID]," +
                                "[Timestamp]," +
                                "[BlockNumber]," +
                                "[DataAddress]," +
                                "[ValidationExpiry]," +
                                "[DataHash]," +
                                "[Nonce]" +
                                " ) values (" +
                                "'" + header.HeaderId + "'," +
                                "'" + header.Category + "'," +
                                "'" + header.IssuerName + "'," +
                                "'" + header.ValidatorName + "'," +
                                "'" + header.IssuerUuid + "'," +
                                "'" + header.ValidatorUuid + "'," +
                                "'" + header.RecipientName + "'," +
                                "'" + header.RecipientUuid + "'," +
                                "'" + header.PreviousHeaderId + "'," +
                                "'" + header.ValidationCounter + "'," +
                                "'" + header.NextHeaderId + "'," +
                                "'" + header.Timestamp + "'," +
                                "'" + header.BlockNumber + "'," +
                                "'" + header.DataAddress + "'," +
                                "'" + header.ValidationExpiry + "'," +
                                "'" + header.DataHash + "'," +
                                "'" + header.Nonce + "'" +
                                ")";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateHeader(HeaderModel header)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "update [dbo].[Header] set " +
                                "[Category] = @category," +
                                "[IssuerName] = @issuername," +
                                "[ValidatorName] = @validatorname," +
                                "[IssuerUUID] = @issuerid," +
                                "[ValidatorLegitimationHeaderID] = @validatorid," +
                                "[RecipientName] = @recipientname," +
                                "[RecipientUUID] = @recipientid," +
                                "[PreviousHeaderID] = @previd," +
                                "[ValidationCounter] = @validationcounter," +
                                "[NextHeaderID] = @nextid," +
                                "[Timestamp] = @timestamp," +
                                "[BlockNumber] = @blocknumber," +
                                "[DataAddress] = @dataaddress," +
                                "[ValidationExpiry] = @validationexpiry," +
                                "[DataHash] = @datahash," +
                                "[Nonce] = @nonce " +
                                "Where Id = @id";                  
                    cmd.Parameters.AddWithValue("@category", header.Category);
                    cmd.Parameters.AddWithValue("@issuername", header.IssuerName);
                    cmd.Parameters.AddWithValue("@validatorname", header.ValidatorName);
                    cmd.Parameters.AddWithValue("@issuerid", header.IssuerUuid);
                    cmd.Parameters.AddWithValue("@validatorid", header.ValidatorUuid);
                    cmd.Parameters.AddWithValue("@recipientname", header.RecipientName);
                    cmd.Parameters.AddWithValue("@recipientid", header.RecipientUuid);
                    cmd.Parameters.AddWithValue("@previd", header.PreviousHeaderId);
                    cmd.Parameters.AddWithValue("@validationcounter", header.ValidationCounter);
                    cmd.Parameters.AddWithValue("@nextid", header.NextHeaderId);
                    cmd.Parameters.AddWithValue("@timestamp", header.Timestamp);
                    cmd.Parameters.AddWithValue("@blocknumber", header.BlockNumber);
                    cmd.Parameters.AddWithValue("@dataaddress", header.DataAddress);
                    cmd.Parameters.AddWithValue("@validationexpiry", header.ValidationExpiry);
                    cmd.Parameters.AddWithValue("@datahash", header.DataHash);
                    cmd.Parameters.AddWithValue("@nonce", header.Nonce);
                    cmd.Parameters.AddWithValue("@id", header.HeaderId);
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }

                }
            }
        }

        public static void DeleteAll()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "delete from [dbo].[Header] where [Category] <> 'root'";
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
