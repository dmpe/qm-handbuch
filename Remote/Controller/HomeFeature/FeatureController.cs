using Dvelop.Remote.Controller.HomeFeature.Dto;
using Dvelop.Remote.Controller.VacationRequest;
using Microsoft.AspNetCore.Mvc;

namespace Dvelop.Remote.Controller.HomeFeature
{
    /// <summary>
    /// Controller for /home-App Features
    /// </summary>
    [Route("features")]
    public class HomeFeatureController : ControllerBase
    {
        public const string FeaturesDescription = "featuresdescription";
     
        /// <summary>
        /// Creates a List of Features, to be shown at the /home-App
        /// </summary>
        /// <returns>Description of Features</returns>
        [Route("description", Name = nameof(HomeFeatureController)+"."+nameof(GetFeaturesDescriptions))]
        [HttpGet]
        public FeatureDescriptionDto GetFeaturesDescriptions()
        {
            return BuildFeatureDescriptionDto();
        }

        private FeatureDescriptionDto BuildFeatureDescriptionDto()
        {
            var featureListDto = new FeatureDescriptionDto();
            
            var feature = BuildFeatureDto();
            featureListDto.Features.Add(feature);
            
            return featureListDto;
        }

        private FeatureDto BuildFeatureDto()
        {
            var feature = new FeatureDto
            {
                Title = "QM-Handbuch",
                SubTitle = "Super!!!",
                Summary = "Noch besser!",
                Url = Url.RouteUrl(nameof(VacationRequestController) + "." +nameof(VacationRequestController.GetVacationListView)),
                Color = "pumpkin",
                Icon = "dv-tags",
                Description = "Learn to be the best!"
            };
            return feature;
        }
    }
}