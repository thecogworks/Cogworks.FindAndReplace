using System.Collections.Generic;
using System.Web.Http;
using System.Web.WebPages;
using Cogworks.FindAndReplace.Models;
using Umbraco.Core.Persistence;
using Umbraco.Core.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Cogworks.FindAndReplace.Web.Controllers.API
{
    [PluginController("FindAndReplace")]
    public class FindAndReplaceAPIController : UmbracoAuthorizedJsonController
    {
        private readonly UmbracoDatabase _db;
        private readonly IContentService _contentService;


        public FindAndReplaceAPIController()
        {
            _db = DatabaseContext.Database;
            _contentService = Services.ContentService;
        }

        [HttpGet]
        public IEnumerable<ContentDataModel> FindPhrase(string phrase, int contentId)
        {
            var queryParamsObj = new
            {
                phrase = "%" + phrase + "%",
                contentId = "%" + contentId + "%"
            };

            var query =
                "SELECT cpt.Alias as PropertyAlias, cpd.dataNvarchar, cpd.dataNtext, ccv.ContentId, un.text as NodeName " +
                "FROM cmsPropertyData cpd " +
                "LEFT JOIN cmsContentVersion ccv ON cpd.versionId = ccv.VersionId " +
                "LEFT JOIN umbracoNode un ON cpd.contentNodeId = un.id " +
                "LEFT JOIN cmsPropertyType cpt ON cpd.propertytypeid = cpt.Id " +
                "WHERE (cpd.dataNtext LIKE @phrase OR cpd.dataNvarchar LIKE @phrase) " +
                "AND ccv.VersionDate = ( SELECT MAX(ccv2.VersionDate) FROM cmsContentVersion ccv2 WHERE ccv2.ContentId = ccv.ContentId) " +
                "AND un.path LIKE @contentId " +
                "ORDER BY ccv.ContentId ASC";

            var result = _db.Fetch<ContentDataModel>(query, queryParamsObj);

            return result;
        }

        [HttpPost]
        public int SetValue(ContentDataModel model)
        {
            //todo check if GetById will not override versions
            var content = _contentService.GetPublishedVersion(model.ContentId);

            var value = model.dataNtext.IsEmpty() ? model.dataNvarchar : model.dataNtext;
            content.SetValue(model.PropertyAlias, value );

            var status = _contentService.SaveAndPublishWithStatus(content);

            return model.ContentId;
        }
    }

}