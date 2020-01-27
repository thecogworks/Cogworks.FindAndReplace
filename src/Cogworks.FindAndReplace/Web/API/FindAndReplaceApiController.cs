using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Cogworks.FindAndReplace.Models.Commands;
using Cogworks.FindAndReplace.Models.Dtos.RequestDtos;
using Umbraco.Core.Scoping;
using Umbraco.Core.Services;
using Umbraco.Web.Editors;
using Umbraco.Web.Mvc;

namespace Cogworks.FindAndReplace.Web.API
{
    [PluginController("FindAndReplace")]
    public class FindAndReplaceApiController : UmbracoAuthorizedJsonController
    {
        private readonly IScopeProvider _scopeProvider;
        private readonly IContentService _contentService;

        public FindAndReplaceApiController(IScopeProvider scopeProvider, IContentService contentService)
        {
            _scopeProvider = scopeProvider;
            _contentService = contentService;
        }

        [HttpGet]
        public async Task<IEnumerable<ContentDataModel>> FindPhrase(string phrase, int contentId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sqlParams = new
                {
                    phrase = $"%{phrase}%",
                    contentId = $"%{contentId}%"
                };

                var sqlQuery = $@"
                    SELECT
                          [upd].[varcharValue] AS [VarcharValue]
                        , [upd].[textValue] AS [TextValue]
                        , [cv].[id] AS [VersionId]
                        , [pcv].[text] AS [NodeName]
                        , [upt].[Alias] AS [PropertyAlias]
                        , [upt].[Name] AS [PropertyName]
                    FROM [umbracoDocument]
                        INNER JOIN [umbracoContent] ON ([umbracoContent].[nodeId] = [umbracoDocument].[nodeId])
                        INNER JOIN [umbracoNode] ON ([umbracoNode].[id] = [umbracoContent].[nodeId])
                        INNER JOIN [umbracoContentVersion] [cv] ON ([cv].[nodeId] = [umbracoDocument].[nodeId])
                        INNER JOIN [umbracoDocumentVersion] ON ([umbracoDocumentVersion].[id] = [cv].[id])
                        LEFT JOIN [umbracoContentVersion] [pcv]
                            INNER JOIN [umbracoDocumentVersion] [pdv] ON (([pdv].[id] = [pcv].[id]) AND [pdv].[published] = 1)
                        ON ([pcv].[nodeId] = [umbracoDocument].[nodeId])
                        LEFT JOIN [umbracoContentVersionCultureVariation] [ccv]
                            INNER JOIN [umbracoLanguage] [lang] ON (([lang].[id] = [ccv].[languageId]) AND ([lang].[languageISOCode] =N'[[[ISOCODE]]]'))
                        ON ([cv].[id] = [ccv].[versionId])
                        INNER JOIN [umbracoPropertyData] [upd] ON ([upd].[versionId] = [cv].[id])
                        INNER JOIN [cmsPropertyType] [upt] ON ([upt].[id] = [upd].[propertyTypeId])
                    WHERE ([cv].[current] = 1)
                        AND ([umbracoDocument].[published] = 1)
                        AND ([umbracoDocument].[edited] = 0)
                        AND ([umbracoNode].[path] LIKE upper(@contentId))
                        AND
                        (
                            (
                                [upd].[textValue] is not null AND [upd].[textValue] LIKE @phrase
                            )
                            OR
                            (
                                [upd].[varcharValue] is not null AND [upd].[varcharValue] LIKE @phrase
                            )
                        )
                    ORDER BY ([umbracoDocument].[nodeId])";

                var sql = scope.SqlContext.Sql(sqlQuery, sqlParams);

                var results = await scope.Database
                    .QueryAsync<ContentDataModel>(sql);

                return results.ToList();
            }
        }

        [HttpPost]
        public UpdatedContentModel SetValue(UpdateCommand command)
        {
            var content = _contentService.GetVersion(command.VersionId);

            var result = new UpdatedContentModel
            {
                PreviousVersionId = command.VersionId,
                Succeeded = content != null
            };

            if (content != null)
            {
                content.SetValue(command.PropertyAlias, command.Value);

                try
                {
                    var status = _contentService.SaveAndPublish(content);

                    result.CurrentVersionId = status.Content.VersionId;
                }
                catch (Exception)
                {
                    result.Succeeded = false;
                }
            }

            return result;
        }
    }
}