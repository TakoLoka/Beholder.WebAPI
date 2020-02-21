using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Documentation
{
    public class Invocation
    {
        public string MethodName { get; set; }
        public string Description { get; set; }
        public string Authorization { get; set; }
        public List<string> Parameters { get; set; }
        public Response OnSuccess { get; set; }
        public Response OnError { get; set; }
    }
}
