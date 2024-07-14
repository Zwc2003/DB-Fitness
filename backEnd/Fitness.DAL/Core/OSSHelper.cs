using Aliyun.OSS;
using Aliyun.OSS.Common;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Fitness.DAL.Core
{
    public static class OSSHelper
    {
          

        public static bool PutObjectFromLocalFile(string objectName, string localFilename)
        {
            OssClient client = new(endpoint, accessKeyId, accessKeySecret);
            try
            {
                var obj = client.PutObject(bucketName, objectName, localFilename);
                if (obj != null && obj.HttpStatusCode == HttpStatusCode.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (OssException ex)
            {
                Console.WriteLine(string.Format("Failed with error code: {0}; Error info: {1}. \nRequestID:{2}\tHostID:{3},BucketName:{4},fileName:{5}",
                    ex.ErrorCode, ex.Message, ex.RequestId, ex.HostId, bucketName, objectName));
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed with error info: {0},BucketName:{1},fileName:{2}", ex.Message, bucketName, objectName));
                return false;
            }
        }

        public static string GetPublicObjectUrl(string objectName)
        {
            return $"https://{bucketName}.{endpoint}/{objectName}";
        }
    }
}
