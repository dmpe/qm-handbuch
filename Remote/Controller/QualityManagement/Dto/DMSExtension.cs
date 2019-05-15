using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dvelop.Remote.Controller.QualityManagement.Dto
{
    public class DMSExtension
    {
        public List<DMSExtensionExtension> extensions { get; set; }
    }

    public class DMSExtensionExtension
    {
        public string id { get; set; }
        public List<DMSExtensionCaption> captions { get; set; }
        public string context { get; set; }
        public string uriTemplate { get; set; }
        public string iconUri { get; set; }
    }

    public class DMSExtensionCaption
    {
        public string culture { get; set; }
        public string caption { get; set; }
    }
}
