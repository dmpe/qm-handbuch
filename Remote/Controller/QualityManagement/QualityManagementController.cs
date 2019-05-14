using System;
using System.Globalization;
using System.Linq;
using Dvelop.Domain.Vacation;
using Dvelop.Remote.Constraints;
using Dvelop.Remote.Controller.VacationRequest.Dto;
using Dvelop.Remote.Controller.VacationRequest.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dvelop.Remote.Controller.QualityManagement
{
    /// <summary>
    /// Example for a controller using business logic and Views
    /// </summary>
    [Route("qm")]
    [AllowAnonymous]
    public class QualityManagementController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IVacationService _service;
        public const string ValuesRelation = "sources";

        public QualityManagementController(IVacationService service)
        {
            _service = service;
        }

        [HttpGet("dmssources", Name = nameof(QualityManagementController) + "." + nameof(GetDmsSources))]
        public object GetDmsSources()
        {
            return Content("{	\"sources\" : [{		\"id\" : \"/devperts-qmhandbuch/sources/mysource\",		\"displayName\" : \"My Source\",		\"categories\": [{			\"key\": \"qm-documents\", 			\"displayName\": \"QM Dokumente\"		}],		\"properties\" : [{			\"key\" : \"chapter\",			\"displayName\" : \"Kapitelnummer\"		}]	}]}", "application/json");
        }
    }
}