using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
//            return Content("{	\"sources\" : [{		\"id\" : \"/devperts-qmhandbuch/sources/mysource\",		
//\"displayName\" : \"QM-Handbuch\",		
//\"categories\": [{			\"key\": \"qm-documents\", 			\"displayName\": \"QM Dokumente\"		}],		
//\"properties\" : [{			\"key\" : \"chapter\",			\"displayName\" : \"Kapitelnummer\"		},
//{			\"key\" : \"headline\",			\"displayName\" : \"Überschrift\"		},
//{			\"key\" : \"parent\",			\"displayName\" : \"Parent Chapter\"		}]	}]}", "application/json");

namespace Dvelop.Remote.Controller.QualityManagement.Dto
{
    public class SourcesList
    {
        public List<Source> sources { get; set; }
    }

    public class Source
    {
        public string id { get; set; }
        public string displayName { get; set; }
        // not sure, it could be array too
        public List<Category> categories { get; set; }
        public List<Property> properties { get; set; }
    }

    public class Category
    {
        public string key { get; set; }
        public string displayName { get; set; }
    }

    public class Property
    {
        public string key { get; set; }
        public string displayName { get; set; }

        public Property(string key, string displayName)
        {
            this.key = key;
            this.displayName = displayName;
        }
    }
}
