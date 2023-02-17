using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SiGG_MobileApp_Server.ServerObjects
{
    public class JsonResult : ActionResult
    {
        public double bytesUsed = 0;

        public JsonResult(ref object obj, ref int status, ref string message)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                using (StringWriter writer = new StringWriter(sb))
                {
                    using (JsonWriter jsonWriter = new JsonTextWriter(writer))
                    {
                        var serializer = new JsonSerializer();
                        serializer.ApplyCustomSettings();
                        serializer.Serialize(writer, obj);
                    }
                }

                string json = sb.ToString();//JsonConvert.SerializeObject(obj);
                this.Content = json;
            }
            catch (Exception ex)
            {
                Status = ResponseStatus.ERROR;
                Message = ex.Message;

                return;
            }

            Status = status;
            Message = message;
        }
    }
}
