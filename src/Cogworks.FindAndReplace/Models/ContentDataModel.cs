using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cogworks.FindAndReplace.Models
{
    public class ContentDataModel
    {
        public string PropertyAlias { get; set; }

        public string dataNvarchar { get; set; }

        public string dataNtext { get; set; }

        public int ContentId { get; set; }

        public string NodeName { get; set; }

        public string Value => dataNvarchar ?? dataNtext;

        public string ValueField => dataNvarchar != null ? "dataNvarchar" : "dataNtext";
    }
}
