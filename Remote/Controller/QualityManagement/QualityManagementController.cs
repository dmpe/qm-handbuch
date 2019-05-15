using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Dvelop.Domain.Repositories;
using Dvelop.Domain.Vacation;
using Dvelop.Remote.Constraints;
using Dvelop.Remote.Controller.VacationRequest.Dto;
using Dvelop.Remote.Controller.VacationRequest.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Dvelop.Remote.Controller.QualityManagement.Dto;

namespace Dvelop.Remote.Controller.QualityManagement
{
    /// <summary>
    /// Example for a controller using business logic and Views
    /// </summary>
    [Route("qm")]
    public class QualityManagementController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IUserRepository _user;
        private readonly ITenantRepository _tenant;

        public const string ValuesRelation = "sources";

        public QualityManagementController(IUserRepository user, ITenantRepository tenant)
        {
            _user = user;
            _tenant = tenant;
        }

        [AllowAnonymous]
        [HttpGet("dmssources", Name = nameof(QualityManagementController) + "." + nameof(GetDmsSources))]
        public object GetDmsSources()
        {
            SourcesList listOfListOfSources = new SourcesList()
            {
                sources = new List<Source>()
                {
                    new Source()
                    {
                        id = "/devperts-qmhandbuch/sources/mysource",
                        displayName = "QM-Handbuch",
                        categories = new List<Category>()
                        {
                            new Category()
                            {
                                key = "qm-documents",
                                displayName = "QM Documents"
                            }
                        },
                        properties = new List<Property>()
                        {
                            new Property("chapter", "Chapter Number" ),
                            new Property("headline", "Headline" ),
                            new Property("parent", "Parent Chapter" )
                        }
                    }
                }
            };

            string requestDMSApp = JsonConvert.SerializeObject(listOfListOfSources);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(listOfListOfSources, Formatting.Indented));
            return Content(requestDMSApp);
//            return Content("{	\"sources\" : [{		\"id\" : \"/devperts-qmhandbuch/sources/mysource\",		\"displayName\" : \"QM-Handbuch\",		\"categories\": [{			\"key\": \"qm-documents\", 			\"displayName\": \"QM Dokumente\"		}],		\"properties\" : [{			\"key\" : \"chapter\",			\"displayName\" : \"Kapitelnummer\"		},{			\"key\" : \"headline\",			\"displayName\" : \"Überschrift\"		},{			\"key\" : \"parent\",			\"displayName\" : \"Parent Chapter\"		}]	}]}", "application/json");
        }

        [HttpGet("chapter", Name = nameof(QualityManagementController) + "." + nameof(GetTopLevelElements))]
        public async Task<object> GetTopLevelElements()
        {
            ViewData["Title"] = "QM-Dokumente";
            /*
            ViewData["Js"] = "vacationrequestlist.js";
            ViewData["Css"] = "vacationrequestlist.css";
            */

            HttpClient httpClient = new HttpClient();
            // In der Doku steht wie man an die ID kommt 
            // https://developer.d-velop.de/documentation/dmsap/de/dms-api-126976273.html
            var searchTopLevelURL = "/dms/r/a0a074f8-5dbf-4af6-9896-fb499047496e/srm/?sourceid=%2fdevperts-qmhandbuch%2fsources%2fmysource&sourcecategories=" + HttpUtility.UrlEncode("[\"qm-documents\"]") + "&sourceproperties=" + HttpUtility.UrlEncode($"{{\"parent\":[\"0\"]}}");
            var url = _tenant.SystemBaseUri.OriginalString + searchTopLevelURL;
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _user.CurrentUser.DvBearer);

            var resonse = await httpClient.SendAsync(request);

            var ret2 = await resonse.Content.ReadAsAsync<SearchReturnDto>();

            var result = ret2.items.Select(x => new ChapterResultDto()
            {
                ChapterNo = x.sourceProperties.Single(y => y.key == "chapter").value,
                Headline = x.sourceProperties.Single(y => y.key == "headline").value,
                DocumentLink = x._links["self"].Href
            }).ToList();

            return View("QualityDocuments", result);
            //return Content(_user.CurrentUser.DvBearer);
            return result;
        }

        [HttpGet("chapter/{parentId}", Name = nameof(QualityManagementController) + "." + nameof(GetSubElements))]
        public async Task<object> GetSubElements(string parentId)
        {
            ViewData["Title"] = "QM-Dokumente";
            /*
            ViewData["Js"] = "vacationrequestlist.js";
            ViewData["Css"] = "vacationrequestlist.css";
            */

            HttpClient httpClient = new HttpClient();
            // In der Doku steht wie man an die ID kommt 
            // https://developer.d-velop.de/documentation/dmsap/de/dms-api-126976273.html
            var searchParentDocsURL = "/dms/r/a0a074f8-5dbf-4af6-9896-fb499047496e/srm/?sourceid=%2fdevperts-qmhandbuch%2fsources%2fmysource&sourcecategories=" + HttpUtility.UrlEncode("[\"qm-documents\"]") + "&sourceproperties=" + HttpUtility.UrlEncode($"{{\"parent\":[\"{parentId}\"]}}");
            var url = _tenant.SystemBaseUri.OriginalString + searchParentDocsURL;
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _user.CurrentUser.DvBearer);

            var resonse = await httpClient.SendAsync(request);

            var ret2 = await resonse.Content.ReadAsAsync<SearchReturnDto>();

            var result = ret2.items.Select(x => new ChapterResultDto()
            {
                ChapterNo = x.sourceProperties.Single(y => y.key == "chapter").value,
                Headline = x.sourceProperties.Single(y => y.key == "headline").value,
                DocumentLink = x._links["self"].Href
            }).ToList();

            return View("SubQualityDocuments", result);
            //return Content(_user.CurrentUser.DvBearer);
            return result;
        }
    }

    public class ChapterResultDto
    {
        public string ChapterNo { get; set; }
        public string Headline { get; set; }
        public string DocumentLink { get; set; }
    }

    public class SearchReturnDto
    {
        public List<SearchResultItemDto> items { get; set; }
    }
    public class SearchResultItemDto : HalJsonDto
    {
        public string id { get; set; }
        public List<SearchResultItemSourceProperty> sourceProperties { get; set; }
    }
    public class SearchResultItemSourceProperty
    {
        public bool isMultiValue { get; set; }
        public string key { get; set; }
        public string value { get; set; }
    }
}
