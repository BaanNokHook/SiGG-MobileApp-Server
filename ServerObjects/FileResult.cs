// Satellite-Communication-Server //

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SocketAppServer.ServerObjects
{
    public class FileResult : ActionResult
    {
        public FileResult(string filePath, int status = ResponseStatus.SUCCESS,
            string message = "Request success")
        {
            try
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                this.Content = bytes; //Convert.ToBase64String(bytes);
            }
            catch(Exception ex)
            {
                this.Status = (int) ResponseStatus.ERROR;
                this.Message = ex.Message;
                return;
            }

            this.Type = 1;
            this.Status = status;
            this.Message = message;
        }
    }
}
