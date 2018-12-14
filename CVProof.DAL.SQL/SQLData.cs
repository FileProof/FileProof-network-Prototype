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

        #region Header
        public static List<HeaderModel> GetHeadersWithImages()
        {
            List<HeaderModel> ret = new List<HeaderModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "select " +
                                    "h.[Id]," +
                                    "h.[Category]," +
                                    "h.[IssuerName]," +
                                    "h.[IssuerUUID]," +
                                    "h.[ValidatorName]," +
                                    "h.[ValidatorUUID]," +
                                    "h.[ValidatorLegitimationHeaderID]," +
                                    "h.[RecipientName]," +
                                    "h.[RecipientUUID]," +
                                    "h.[PreviousHeaderID]," +
                                    "h.[ValidationCounter]," +
                                    "h.[NextHeaderID]," +
                                    "h.[Timestamp]," +
                                    "h.[BlockNumber]," +
                                    "h.[DataAddress]," +
                                    "h.[ValidationExpiry]," +
                                    "h.[DataHash]," +
                                    "h.[Nonce]," +
                                    "h.[Stored]," +
                                    "h.[Attachment]," +
                                    "h.[GlobalHash]," +
                                    "pv.[Id]," +
                                    "pv.[Name]," +
                                    "pv.[Roles]," +
                                    "pv.[Image]," +
                                    "pi.[Id]," +
                                    "pi.[Name]," +
                                    "pi.[Roles]," +
                                    "pi.[Image]," +
                                    "pr.[Id]," +
                                    "pr.[Name]," +
                                    "pr.[Roles]," +
                                    "pr.[Image]" +
                                    " from Header h" +
                                    " left join [dbo].[Profile] pv on h.[ValidatorUUID] = pv.[Id]" +
                                    " left join [dbo].[Profile] pi on h.[IssuerUUID] = pi.[Id]" +
                                    " left join [dbo].[Profile] pr on h.[RecipientUUID] = pr.[Id]" +
                                    " where [Category] <> 'Attachment'";
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                try
                {
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
                                Attachment = reader.GetString(19),
                                GlobalHash = reader.GetString(20),

                                ValidatorProfile = reader.IsDBNull(21) ? new UserProfileModel()
                                                                       : new UserProfileModel()
                                                                       {
                                                                           Id = reader.GetString(21),
                                                                           Name = reader.GetString(22),
                                                                           Roles = reader.GetString(23),
                                                                           Image = reader.IsDBNull(24) ? new byte[0] : (byte[])(reader[24]),
                                                                       },

                                IssuerProfile = reader.IsDBNull(25) ? new UserProfileModel()
                                                                    : new UserProfileModel()
                                                                    {
                                                                        Id = reader.GetString(25),
                                                                        Name = reader.GetString(26),
                                                                        Roles = reader.GetString(27),
                                                                        Image = reader.IsDBNull(28) ? new byte[0] : (byte[])(reader[28]),
                                                                    },
                                RecipientProfile = reader.IsDBNull(29) ? new UserProfileModel()
                                                                       : new UserProfileModel()
                                                                       {
                                                                           Id = reader.GetString(29),
                                                                           Name = reader.GetString(30),
                                                                           Roles = reader.GetString(31),
                                                                           Image = reader.IsDBNull(32) ? new byte[0] : (byte[])(reader[32]),
                                                                       }
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return ret;
        }

        public static List<HeaderModel> GetHeadersWithImagesByIssuer(string id)
        {
            List<HeaderModel> ret = new List<HeaderModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "select " +
                                    "h.[Id]," +
                                    "h.[Category]," +
                                    "h.[IssuerName]," +
                                    "h.[IssuerUUID]," +
                                    "h.[ValidatorName]," +
                                    "h.[ValidatorUUID]," +
                                    "h.[ValidatorLegitimationHeaderID]," +
                                    "h.[RecipientName]," +
                                    "h.[RecipientUUID]," +
                                    "h.[PreviousHeaderID]," +
                                    "h.[ValidationCounter]," +
                                    "h.[NextHeaderID]," +
                                    "h.[Timestamp]," +
                                    "h.[BlockNumber]," +
                                    "h.[DataAddress]," +
                                    "h.[ValidationExpiry]," +
                                    "h.[DataHash]," +
                                    "h.[Nonce]," +
                                    "h.[Stored]," +
                                    "h.[Attachment]," +
                                    "h.[GlobalHash]," +
                                    "pv.[Id]," +
                                    "pv.[Name]," +
                                    "pv.[Roles]," +
                                    "pv.[Image]," +
                                    "pi.[Id]," +
                                    "pi.[Name]," +
                                    "pi.[Roles]," +
                                    "pi.[Image]," +
                                    "pr.[Id]," +
                                    "pr.[Name]," +
                                    "pr.[Roles]," +
                                    "pr.[Image]" +
                                    " from [dbo].[Header] h" +
                                    " left join [dbo].[Profile] pv on h.[ValidatorUUID] = pv.[Id]" +
                                    " left join [dbo].[Profile] pi on h.[IssuerUUID] = pi.[Id]" +
                                    " left join [dbo].[Profile] pr on h.[RecipientUUID] = pr.[Id]" +
                                    " where (([IssuerUUID] = @id) OR ([ValidatorUUID] = @id) OR ([RecipientUUID] = @id))" +
                                    " AND (Category <> 'Attachment')";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();
                try
                { 
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
                                Attachment = reader.GetString(19),
                                GlobalHash = reader.GetString(20),
                                ValidatorProfile = reader.IsDBNull(21) ? new UserProfileModel()
                                                                       : new UserProfileModel()
                                                                       {
                                                                           Id = reader.GetString(21),
                                                                           Name = reader.GetString(22),
                                                                           Roles = reader.GetString(23),
                                                                           Image = reader.IsDBNull(24) ? new byte[0] : (byte[])(reader[24]),
                                                                       },

                                IssuerProfile = reader.IsDBNull(25) ? new UserProfileModel()
                                                                    : new UserProfileModel()
                                                                    {
                                                                        Id = reader.GetString(25),
                                                                        Name = reader.GetString(26),
                                                                        Roles = reader.GetString(27),
                                                                        Image = reader.IsDBNull(28) ? new byte[0] : (byte[])(reader[28]),
                                                                    },
                                RecipientProfile = reader.IsDBNull(29) ? new UserProfileModel()
                                                                       : new UserProfileModel()
                                                                       {
                                                                           Id = reader.GetString(29),
                                                                           Name = reader.GetString(30),
                                                                           Roles = reader.GetString(31),
                                                                           Image = reader.IsDBNull(32) ? new byte[0] : (byte[])(reader[32]),
                                                                       }
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
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
                                    "h.[Id]," +
                                    "h.[Category]," +
                                    "h.[IssuerName]," +
                                    "h.[IssuerUUID]," +
                                    "h.[ValidatorName]," +
                                    "h.[ValidatorUUID]," +
                                    "h.[ValidatorLegitimationHeaderID]," +
                                    "h.[RecipientName]," +
                                    "h.[RecipientUUID]," +
                                    "h.[PreviousHeaderID]," +
                                    "h.[ValidationCounter]," +
                                    "h.[NextHeaderID]," +
                                    "h.[Timestamp]," +
                                    "h.[BlockNumber]," +
                                    "h.[DataAddress]," +
                                    "h.[ValidationExpiry]," +
                                    "h.[DataHash]," +
                                    "h.[Nonce]," +
                                    "h.[Stored]," +
                                    "h.[Attachment]," +
                                    "h.[GlobalHash]" +
                                    " from [dbo].[Header] h" +
                                    " where h.[Id] = @id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();

                try
                {
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
                                Attachment = reader.GetString(19),
                                GlobalHash = reader.GetString(20)
                            };
                        }
                    }
                }                
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }
            return ret;
        }

        public static HeaderModel GetHeaderWithImageById(string id)
        {
            HeaderModel ret = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "select " +
                                    "h.[Id]," +
                                    "h.[Category]," +
                                    "h.[IssuerName]," +
                                    "h.[IssuerUUID]," +
                                    "h.[ValidatorName]," +
                                    "h.[ValidatorUUID]," +
                                    "h.[ValidatorLegitimationHeaderID]," +
                                    "h.[RecipientName]," +
                                    "h.[RecipientUUID]," +
                                    "h.[PreviousHeaderID]," +
                                    "h.[ValidationCounter]," +
                                    "h.[NextHeaderID]," +
                                    "h.[Timestamp]," +
                                    "h.[BlockNumber]," +
                                    "h.[DataAddress]," +
                                    "h.[ValidationExpiry]," +
                                    "h.[DataHash]," +
                                    "h.[Nonce]," +
                                    "h.[Stored]," +
                                    "h.[Attachment]," +
                                    "h.[GlobalHash]," +
                                    "pv.[Id]," +
                                    "pv.[Name]," +
                                    "pv.[Roles]," +
                                    "pv.[Image]," +
                                    "pi.[Id]," +
                                    "pi.[Name]," +
                                    "pi.[Roles]," +
                                    "pi.[Image]," +
                                    "pr.[Id]," +
                                    "pr.[Name]," +
                                    "pr.[Roles]," +
                                    "pr.[Image]" +
                                    " from [dbo].[Header] h" + 
                                    " left join [dbo].[Profile] pv on h.[ValidatorUUID] = pv.[Id]" +
                                    " left join [dbo].[Profile] pi on h.[IssuerUUID] = pi.[Id]" +
                                    " left join [dbo].[Profile] pr on h.[RecipientUUID] = pr.[Id]" +
                                    " where h.[Id] = @id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();

                try
                {
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
                                Attachment = reader.GetString(19),
                                GlobalHash = reader.GetString(20),
                                ValidatorProfile = reader.IsDBNull(21) ? new UserProfileModel()
                                                                       : new UserProfileModel()
                                                                       {
                                                                           Id = reader.GetString(21),
                                                                           Name = reader.GetString(22),
                                                                           Roles = reader.GetString(23),
                                                                           Image = reader.IsDBNull(24) ? new byte[0] : (byte[])(reader[24]),
                                                                       },

                                IssuerProfile = reader.IsDBNull(25) ? new UserProfileModel()
                                                                    : new UserProfileModel()
                                                                    {
                                                                        Id = reader.GetString(25),
                                                                        Name = reader.GetString(26),
                                                                        Roles = reader.GetString(27),
                                                                        Image = reader.IsDBNull(28) ? new byte[0] : (byte[])(reader[28]),
                                                                    },
                                RecipientProfile = reader.IsDBNull(29) ? new UserProfileModel()
                                                                       : new UserProfileModel()
                                                                       {
                                                                           Id = reader.GetString(29),
                                                                           Name = reader.GetString(30),
                                                                           Roles = reader.GetString(31),
                                                                           Image = reader.IsDBNull(32) ? new byte[0] : (byte[])(reader[32]),
                                                                       }
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
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
                                    "[Attachment]," +
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
                            Attachment = reader.GetString(19),
                            GlobalHash = reader.GetString(20)
                        };
                    }
                }
            }
            return ret;
        }

        public static HeaderModel GetHeaderByNonce(string nonce)
        {
            HeaderModel ret = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {

                cmd.CommandText = "select " +
                                    "h.[Id]," +
                                    "h.[Category]," +
                                    "h.[IssuerName]," +
                                    "h.[IssuerUUID]," +
                                    "h.[ValidatorName]," +
                                    "h.[ValidatorUUID]," +
                                    "h.[ValidatorLegitimationHeaderID]," +
                                    "h.[RecipientName]," +
                                    "h.[RecipientUUID]," +
                                    "h.[PreviousHeaderID]," +
                                    "h.[ValidationCounter]," +
                                    "h.[NextHeaderID]," +
                                    "h.[Timestamp]," +
                                    "h.[BlockNumber]," +
                                    "h.[DataAddress]," +
                                    "h.[ValidationExpiry]," +
                                    "h.[DataHash]," +
                                    "h.[Nonce]," +
                                    "h.[Stored]," +
                                    "h.[Attachment]," +
                                    "h.[GlobalHash]," +
                                    "p.[Id]," +
                                    "p.[Name]," +
                                    "p.[Roles]," +
                                    "p.[Image]" +
                                    " from [dbo].[Header] h" +
                                    " left join [Profile] p on p.Id = h.Id" +
                                    " where Nonce = @nonce";

                cmd.Parameters.AddWithValue("@nonce", nonce);
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
                            Attachment = reader.GetString(19),
                            GlobalHash = reader.GetString(20),
                            SelfProfile = new UserProfileModel()
                            {
                                Id = reader.GetString(21),
                                Name = reader.GetString(22),
                                Roles = reader.GetString(23),
                                Image = reader.IsDBNull(24) ? new byte[0] : (byte[])(reader[24]),
                            },
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
                                "[Attachment]," +
                                "[GlobalHash]" +
                                " ) values (" +
                                "@headerId," +
                                "@category," +
                                "@issuerName," +                                
                                "@issuerUuid," +
                                "@validatorName," +
                                "@validatorUuid," +
                                "@validatorLegitimationId," +
                                "@recipientName," +
                                "@recipientUuid," +
                                "@previousHeaderId," +
                                "@validationCounter," +
                                "@nextHeaderId," +
                                "@timestamp," +
                                "@blockNumber," +
                                "@dataAddress," +
                                "@validationExpiry," +
                                "@dataHash," +
                                "@nonce," +
                                "@stored," +
                                "@attachment," +
                                "@globalHash" +
                                ")";

                        cmd.Parameters.AddWithValue("@headerId", header.HeaderId ?? String.Empty );
                        cmd.Parameters.AddWithValue("@category", header.Category ?? String.Empty);
                        cmd.Parameters.AddWithValue("@issuerName", header.IssuerName ?? String.Empty);
                        cmd.Parameters.AddWithValue("@issuerUuid", header.IssuerUuid ?? String.Empty);
                        cmd.Parameters.AddWithValue("@validatorName", header.ValidatorName ?? String.Empty);
                        cmd.Parameters.AddWithValue("@validatorUuid", header.ValidatorUuid ?? String.Empty);
                        cmd.Parameters.AddWithValue("@validatorLegitimationId", header.ValidatorLegitimationId ?? String.Empty);
                        cmd.Parameters.AddWithValue("@recipientName", header.RecipientName ?? String.Empty);
                        cmd.Parameters.AddWithValue("@recipientUuid", header.RecipientUuid ?? String.Empty);
                        cmd.Parameters.AddWithValue("@previousHeaderId", header.PreviousHeaderId ?? String.Empty);
                        cmd.Parameters.AddWithValue("@validationCounter", header.ValidationCounter ?? String.Empty);
                        cmd.Parameters.AddWithValue("@nextHeaderId", header.NextHeaderId ?? String.Empty);
                        cmd.Parameters.AddWithValue("@timestamp", header.Timestamp ?? String.Empty);
                        cmd.Parameters.AddWithValue("@blockNumber", header.BlockNumber ?? String.Empty);
                        cmd.Parameters.AddWithValue("@dataAddress", header.DataAddress ?? String.Empty);
                        cmd.Parameters.AddWithValue("@validationExpiry", header.ValidationExpiry ?? String.Empty);
                        cmd.Parameters.AddWithValue("@dataHash", header.DataHash ?? String.Empty);
                        cmd.Parameters.AddWithValue("@nonce", header?.Nonce?.Replace("'",@"''") ?? String.Empty);
                        cmd.Parameters.AddWithValue("@stored", header.Stored);
                        cmd.Parameters.AddWithValue("@attachment", header.Attachment ?? String.Empty);
                        cmd.Parameters.AddWithValue("@globalHash", header.GlobalHash ?? String.Empty);
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
                                "[Attachment] = @attachment," +
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
                    cmd.Parameters.AddWithValue("@attachment", header.Attachment ?? String.Empty);
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

        public static void DeleteDoc(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "delete from [dbo].[Header] where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id ?? String.Empty);
                    cmd.ExecuteNonQuery();
                }

            }
        }
        #endregion

        #region Profile
        public static UserProfileModel GetProfileById(string id)
        {
            UserProfileModel ret = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select " +
                                    "[Id]," +
                                    "[Roles]," +
                                    "[Name]," +
                                    "[Image]" +
                                    " from [dbo].[Profile]" +
                                    " where Id = @id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.CommandType = System.Data.CommandType.Text;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        ret = new UserProfileModel()
                        {
                            Id = reader.GetString(0),
                            Roles = reader.GetString(1),
                            Name = reader.GetString(2),
                            Image = reader.IsDBNull(3)? new byte[0] : (byte[])(reader[3])                              
                        };
                    }
                }
            }
            return ret;
        }

        public static void InsertProfile(UserProfileModel profile)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "insert into [dbo].[Profile] ( " +
                                "[Id]," +
                                "[Roles]," +
                                "[Name]," +
                                "[Image]" +
                                " ) values (" +
                                "@id," +                                
                                "@roles," +
                                "@name," +
                                "@image" +
                                ")";
                    cmd.Parameters.AddWithValue("@id", profile.Id);
                    cmd.Parameters.AddWithValue("@roles", profile.Roles);
                    cmd.Parameters.AddWithValue("@name", profile.Name);
                    cmd.Parameters.AddWithValue("@image", profile.Image);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateProfile(UserProfileModel profile)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "update [dbo].[Profile] set " +
                    "[Roles] = @roles," +
                    "[Name] = @name" +
                    (profile.Image == null ? "" : ", [Image] = @image") +
                    " Where Id = @id";
                    cmd.Parameters.AddWithValue("@roles", profile.Roles ?? String.Empty);
                    cmd.Parameters.AddWithValue("@name", profile.Name);

                    if(profile.Image != null)
                        cmd.Parameters.AddWithValue("@image", profile.Image);

                    cmd.Parameters.AddWithValue("@id", profile.Id ?? String.Empty);

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

        public static void DeleteProfile(string id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "delete from [dbo].[Profile] where Id = @id";
                    cmd.Parameters.AddWithValue("@id", id ?? String.Empty);
                    cmd.ExecuteNonQuery();
                }

            }
        }
        #endregion
    }
}
