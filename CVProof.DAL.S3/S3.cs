using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Encryption;

namespace CVProof.DAL.AWS
{

    public static class S3
    {
        // Change the AWSProfileName to the profile you want to use in the App.config file.
        // See http://aws.amazon.com/credentials  for more details.
        // You must also sign up for an Amazon S3 account for this to work
        // See http://aws.amazon.com/s3/ for details on creating an Amazon S3 account
        // Change the bucketName and keyName fields to values that match your bucketname and keyname
        static string keyContainerName = "S3SampleKeyContainer";

        static string accessKey = ""; // AWS user keys
        static string secretKey = "";

        static IAmazonS3 client;

        public static async Task GetObject(string bucketName, string id)
        {
            try
            {
                GetObjectRequest request = new GetObjectRequest()
                {
                    BucketName = bucketName,
                    Key = id
                };

                using (GetObjectResponse response = await client.GetObjectAsync(request))
                {
                    string title = response.Metadata["x-amz-meta-title"];
                    Console.WriteLine("The object's title is {0}", title);
                    string dest = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), id);
                    if (!File.Exists(dest))
                    {
                        var cancelSource = new CancellationTokenSource();
                        var token = cancelSource.Token;

                        await response.WriteResponseStreamToFileAsync(dest, false, token);
                    }
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when reading an object", amazonS3Exception.Message);
                }
            }
        }
        public static async Task DeleteObject(string bucketName, string id)
        {
            try
            {
                DeleteObjectRequest request = new DeleteObjectRequest()
                {
                    BucketName = bucketName,
                    Key = id
                };

                await client.DeleteObjectAsync(request);
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null &&
                    (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                    amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when deleting an object", amazonS3Exception.Message);
                }
            }
        }
        public static async Task ListObjects(string bucketName)
        {
            try
            {
                ListObjectsRequest request = new ListObjectsRequest();
                request.BucketName = bucketName;
                ListObjectsResponse response = await client.ListObjectsAsync(request);
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                }

                // list only things starting with "foo"
                request.Prefix = "foo";
                response = await client.ListObjectsAsync(request);

                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                }

                // list only things that come after "bar" alphabetically
                request.Prefix = null;
                request.Marker = "bar";
                response = await client.ListObjectsAsync(request);
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                }

                // only list 3 things
                request.Prefix = null;
                request.Marker = null;
                request.MaxKeys = 3;
                response = await client.ListObjectsAsync(request);
                foreach (S3Object entry in response.S3Objects)
                {
                    Console.WriteLine("key = {0} size = {1}", entry.Key, entry.Size);
                }
            }
            catch (AmazonS3Exception amazonS3Exception)
            {
                if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") || amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                {
                    Console.WriteLine("Please check the provided AWS Credentials.");
                    Console.WriteLine("If you haven't signed up for Amazon S3, please visit http://aws.amazon.com/s3");
                }
                else
                {
                    Console.WriteLine("An error occurred with the message '{0}' when listing objects", amazonS3Exception.Message);
                }
            }
        }
        public static async Task WriteObject(Stream fileStream, string bucketName, string id, string contentType)
        {                       
            using (client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.EUWest1))
            {
                try
                {
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        ContentType = String.IsNullOrEmpty(contentType) ? "application/octet-stream" : contentType,
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = id,
                        InputStream = fileStream
                    };

                    request.Metadata.Add("Id", id);

                    PutObjectResponse response = await client.PutObjectAsync(request);
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null &&
                        (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                        amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        Console.WriteLine("Please check the provided AWS Credentials.");
                    }
                    else
                    {
                        Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
                    }
                }
            }
        }

        public static async Task WriteObjectEncrypted(Stream fileStream, string bucketName, string id, string contentType)
        {
            RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider();
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = keyContainerName;
            RSACryptoServiceProvider key = new RSACryptoServiceProvider(cp);

            using (client = new AmazonS3EncryptionClient(new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.EUWest1, new EncryptionMaterials(key)))
            {
                try
                {
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        ContentType = String.IsNullOrEmpty(contentType) ? "application/octet-stream" : contentType,
                        BucketName = bucketName,
                        CannedACL = S3CannedACL.PublicRead,
                        Key = id,
                        InputStream = fileStream
                    };

                    request.Metadata.Add("Id", id);

                    PutObjectResponse response = await client.PutObjectAsync(request);
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null &&
                        (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                        amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        Console.WriteLine("Please check the provided AWS Credentials.");
                    }
                    else
                    {
                        Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
                    }
                }
            }

            key.PersistKeyInCsp = false;
            key.Clear();
        }
        public static async Task WriteObject(string text, string bucketName, string id, string contentType)
        {
            RSACryptoServiceProvider rsaKey = new RSACryptoServiceProvider();
            CspParameters cp = new CspParameters();
            cp.KeyContainerName = keyContainerName;
            RSACryptoServiceProvider key = new RSACryptoServiceProvider(cp);

            using (client = new AmazonS3EncryptionClient(new Amazon.Runtime.BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.EUWest1, new EncryptionMaterials(key)))
            {
                try
                {
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        ContentType = "text/plain",
                        ContentBody = text,
                        BucketName = bucketName,
                        Key = id
                    };

                    request.Metadata.Add("Id", id);

                    PutObjectResponse response = await client.PutObjectAsync(request);
                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null &&
                        (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                        amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        Console.WriteLine("Please check the provided AWS Credentials.");
                    }
                    else
                    {
                        Console.WriteLine("An error occurred with the message '{0}' when writing an object", amazonS3Exception.Message);
                    }
                }
                key.PersistKeyInCsp = false;
                key.Clear();
            }
        }

    }    
}
