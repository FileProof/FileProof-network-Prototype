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
                                    "[IssuerUUID]," +
                                    "[ValidatorName]," +
                                    "[ValidatorUUID]," +
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
                                    "[Nonce]," +
                                    "[Stored]," +
                                    "[GlobalHash]" +
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
                            IssuerUuid = reader.GetString(3),
                            ValidatorName = reader.GetString(4),                            
                            ValidatorUuid = reader.GetString(5),
                            ValidatorLegitimationId = reader.GetString(6),
                            RecipientName = reader.GetString(7),
                            RecipientUuid = reader.GetString(8),
                            PreviousHeaderId = reader.GetString(9),
                            ValidationCounter = reader.GetString(10),
                            NextHeaderId = reader.GetString(11),
                            Timestamp = reader.GetString(12),
                            BlockNumber = reader.GetString(13),
                            DataAddress = reader.GetString(14),
                            ValidationExpiry = reader.GetString(15),
                            DataHash = reader.GetString(16),
                            Nonce = reader.GetString(17),
                            Stored = reader.GetBoolean(18),
                            GlobalHash = reader.GetString(19)
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
                                    "[IssuerUUID]," +
                                    "[ValidatorName]," +
                                    "[ValidatorUUID]," +
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
                                    "[Nonce]," +
                                    "[Stored]," +
                                    "[GlobalHash]" +
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
                            IssuerUuid = reader.GetString(3),
                            ValidatorName = reader.GetString(4),                            
                            ValidatorUuid = reader.GetString(5),
                            ValidatorLegitimationId = reader.GetString(6),
                            RecipientName = reader.GetString(7),
                            RecipientUuid = reader.GetString(8),
                            PreviousHeaderId = reader.GetString(9),
                            ValidationCounter = reader.GetString(10),
                            NextHeaderId = reader.GetString(11),
                            Timestamp = reader.GetString(12),
                            BlockNumber = reader.GetString(13),
                            DataAddress = reader.GetString(14),
                            ValidationExpiry = reader.GetString(15),
                            DataHash = reader.GetString(16),
                            Nonce = reader.GetString(17),
                            Stored = reader.GetBoolean(18),
                            GlobalHash = reader.GetString(19),
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
                                    "[IssuerUUID]," +
                                    "[ValidatorName]," +
                                    "[ValidatorUUID]," +
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
                                    "[Nonce]," +
                                    "[Stored]," +
                                    "[GlobalHash]" +
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
                            IssuerUuid = reader.GetString(3),
                            ValidatorName = reader.GetString(4),                            
                            ValidatorUuid = reader.GetString(5),
                            ValidatorLegitimationId = reader.GetString(6),
                            RecipientName = reader.GetString(7),
                            RecipientUuid = reader.GetString(8),
                            PreviousHeaderId = reader.GetString(9),
                            ValidationCounter = reader.GetString(10),
                            NextHeaderId = reader.GetString(11),
                            Timestamp = reader.GetString(12),
                            BlockNumber = reader.GetString(13),
                            DataAddress = reader.GetString(14),
                            ValidationExpiry = reader.GetString(15),
                            DataHash = reader.GetString(16),
                            Nonce = reader.GetString(17),
                            Stored = reader.GetBoolean(18),
                            GlobalHash = reader.GetString(19)
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
                                "[IssuerUUID]," +
                                "[ValidatorName]," +
                                "[ValidatorUUID]," +
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
                                "[Nonce]," +
                                "[Stored]," +
                                "[GlobalHash]" +
                                " ) values (" +
                                "'" + header.HeaderId + "'," +
                                "'" + header.Category + "'," +
                                "'" + header.IssuerName + "'," +                                
                                "'" + header.IssuerUuid + "'," +
                                "'" + header.ValidatorName + "'," +
                                "'" + header.ValidatorUuid + "'," +
                                "'" + header.ValidatorLegitimationId + "'," +
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
                                "'" + header.Nonce.Replace("'", @"#") + "'," + // Preventing single quote in sql clause
                                "'" + header.Stored + "'," +
                                "'" + header.GlobalHash + "'" +
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
                                "[IssuerUUID] = @issuerid," +
                                "[ValidatorName] = @validatorname," +
                                "[ValidatorUUID] = @validatorid," +
                                "[ValidatorLegitimationHeaderID] = @validatorlegitimationid," +
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
                                "[Nonce] = @nonce," +
                                "[Stored] = @stored," +
                                "[GlobalHash] = @globalhash " +
                                "Where Id = @id";                  
                    cmd.Parameters.AddWithValue("@category", header.Category ?? String.Empty);
                    cmd.Parameters.AddWithValue("@issuername", header.IssuerName ?? String.Empty);
                    cmd.Parameters.AddWithValue("@validatorname", header.ValidatorName ?? String.Empty);
                    cmd.Parameters.AddWithValue("@issuerid", header.IssuerUuid ?? String.Empty);
                    cmd.Parameters.AddWithValue("@validatorid", header.ValidatorUuid ?? String.Empty);
                    cmd.Parameters.AddWithValue("@validatorlegitimationid", header.ValidatorLegitimationId ?? String.Empty);
                    cmd.Parameters.AddWithValue("@recipientname", header.RecipientName ?? String.Empty);
                    cmd.Parameters.AddWithValue("@recipientid", header.RecipientUuid ?? String.Empty);
                    cmd.Parameters.AddWithValue("@previd", header.PreviousHeaderId ?? String.Empty);
                    cmd.Parameters.AddWithValue("@validationcounter", header.ValidationCounter ?? String.Empty);
                    cmd.Parameters.AddWithValue("@nextid", header.NextHeaderId ?? String.Empty);
                    cmd.Parameters.AddWithValue("@timestamp", header.Timestamp ?? String.Empty);
                    cmd.Parameters.AddWithValue("@blocknumber", header.BlockNumber ?? String.Empty);
                    cmd.Parameters.AddWithValue("@dataaddress", header.DataAddress ?? String.Empty);
                    cmd.Parameters.AddWithValue("@validationexpiry", header.ValidationExpiry ?? String.Empty);
                    cmd.Parameters.AddWithValue("@datahash", header.DataHash ?? String.Empty);
                    cmd.Parameters.AddWithValue("@nonce", header.Nonce ?? String.Empty);
                    cmd.Parameters.AddWithValue("@stored", header.Stored);
                    cmd.Parameters.AddWithValue("@globalhash", header.GlobalHash ?? String.Empty);
                    cmd.Parameters.AddWithValue("@id", header.HeaderId ?? String.Empty);
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
                    cmd.CommandText = "delete from [dbo].[Header]";
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "INSERT [dbo].[Header] ([Id], [Category], [IssuerName], [IssuerUUID], [ValidatorName], [ValidatorUUID], [ValidatorLegitimationHeaderID], [RecipientName], [RecipientUUID], [PreviousHeaderID], [ValidationCounter], [NextHeaderID], [Timestamp], [BlockNumber], [DataAddress], [ValidationExpiry], [DataHash], [Nonce], [Stored], [GlobalHash]) VALUES (N'0x0100000000000000000000000000000000000000000000000000000000000000', N'Root', N'', N'', N'0x0100000000000000000000000000000000000000000000000000000000000000', N'0x0100000000000000000000000000000000000000000000000000000000000000', N'', N'', N'', N'', N'1', N'', N'1536625353', N'4014541', N'0xb5d5052f9417fc4b52fe06dc2ad82d849b03661ab452a2a81256d49818dcb4ed', N'', N'', N'', 0, N'0xb5d5052f9417fc4b52fe06dc2ad82d849b03661ab452a2a81256d49818dcb4ed')";
                    cmd.ExecuteNonQuery();
                }

            }
        }

    }
}
