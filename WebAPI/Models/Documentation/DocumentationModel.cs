using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models.Documentation
{
    public class DocumentationModel
    {
        public string Title { get; set; }
        public string Version { get; set; }
        public string BattlemapRoute { get; set; }
        public List<Instruction> Instructions { get; set; }
        public List<string> Notes { get; set; }
        public List<Invocation> Invocations { get; set; }
        public List<ReceiveMethod> ReceiveMethods { get; set; }
    }
}
