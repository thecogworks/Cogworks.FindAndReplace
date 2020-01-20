using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Cogworks.FindAndReplace.Models.Commands;
using Cogworks.FindAndReplace.Models.Dtos.DatabaseDtos;
using Cogworks.FindAndReplace.Models.Dtos.RequestDtos;
using NPoco;
using Umbraco.Core.Persistence;
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
        public IEnumerable<ContentDataModel> FindPhrase(string phrase, int contentId)
        {
            using (var scope = _scopeProvider.CreateScope(autoComplete: true))
            {
                var sql = scope.SqlContext.Sql();

                sql = sql.Select<PropertyDataDto>("upd", upd => upd.VarcharValue);
                sql = sql.AndSelect<PropertyDataDto>("upd", upd => upd.TextValue);
                sql = sql.AndSelect<ContentVersionDto>("cv", cv => cv.Id); // VersionId
                sql = sql.AndSelect<ContentVersionDto>("pcv", pcv => pcv.Text); // NodeName
                sql = sql.AndSelect<PropertyTypeDto>("upt", upt => upt.Alias); // PropertyAlias
                sql = sql.AndSelect<PropertyTypeDto>("upt", upt => upt.Name); // Name

                sql = sql.From<DocumentDto>();

                sql = sql.InnerJoin<ContentDto>()
                    .On<ContentDto, DocumentDto>((left, right) => left.NodeId == right.NodeId);

                sql = sql.InnerJoin<NodeDto>()
                    .On<NodeDto, ContentDto>((left, right) => left.NodeId == right.NodeId);

                sql = sql.InnerJoin<ContentVersionDto>("cv")
                    .On<ContentVersionDto, DocumentDto>((left, right) => left.NodeId == right.NodeId, "cv");

                sql = sql.InnerJoin<DocumentVersionDto>()
                    .On<DocumentVersionDto, ContentVersionDto>((left, right) => left.Id == right.Id, aliasRight: "cv");

                sql = sql.LeftJoin<ContentVersionDto>(nested =>
                        nested.InnerJoin<DocumentVersionDto>("pdv")
                            .On<DocumentVersionDto, ContentVersionDto>(
                                (left, right) => left.Id == right.Id && left.Published, "pdv", "pcv"), "pcv")
                    .On<ContentVersionDto, DocumentDto>((left, right) => left.NodeId == right.NodeId, "pcv");

                sql = sql.LeftJoin<ContentVersionCultureVariationDto>(nested =>
                        nested.InnerJoin<LanguageDto>("lang")
                            .On<LanguageDto, ContentVersionCultureVariationDto>(
                                (left, right) => left.Id == right.LanguageId && left.IsoCode == "[[[ISOCODE]]]", "lang",
                                "ccv"), "ccv")
                    .On<ContentVersionDto, ContentVersionCultureVariationDto>(
                        (left, right) => left.Id == right.VersionId, "cv", "ccv");

                sql = sql.InnerJoin<PropertyDataDto>("upd")
                    .On<PropertyDataDto, ContentVersionDto>((left, right) => left.VersionId == right.Id, "upd", "cv");

                sql = sql.InnerJoin<PropertyTypeDto>("upt")
                    .On<PropertyTypeDto, PropertyDataDto>((left, right) => left.Id == right.PropertyTypeId, "upt", "upd");

                sql = sql.Where<ContentVersionDto>(c => c.Current, "cv");
                sql = sql.Where<DocumentDto>(d => d.Published);
                sql = sql.Where<NodeDto>(n => n.Path.Contains(contentId.ToString()));

                var likePhrase = $"'%{phrase}%'";

                var textQuery = new Sql($@"AND
                (
                    (
                        ([upd].[textValue] is not null)
                        AND [upd].[textValue] LIKE {likePhrase}
                    )
                    OR
                    (
                        ([upd].[varcharValue] is not null)
                        AND [upd].[varcharValue] LIKE {likePhrase}
                    )
                )");

                sql = sql.Append(textQuery);

                // TODO: Find better way to represent it with strongly typed way
                // Better would be one of those bellow, but NPoco translate it badly (ads upper function to column and search phrase -> ntext don't allow to use this function on it)

                // ntext and upper function issue -> NPoco adds it to contains methods
                //                sql = sql.Where<PropertyDataDto>(p =>
                //                    (p.TextValue != null &&
                //                     p.TextValue.InvariantContains(phrase)) ||
                //                    (p.VarcharValue != null &&
                //                     p.VarcharValue.InvariantContains(phrase)));

                // Not implemented translation of IndexOf exception
                //                sql = sql.Where<PropertyDataDto>(p =>
                //                    (p.TextValue != null &&
                //                     p.TextValue.IndexOf(phrase, StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                //                    (p.VarcharValue != null &&
                //                     p.VarcharValue.IndexOf(phrase, StringComparison.CurrentCultureIgnoreCase) >= 0));

                sql = sql.OrderBy<DocumentDto>(d => d.NodeId);

                var result = scope.Database
                    .Query<dynamic>(sql)
                    .Select(x => new ContentDataModel
                    {
                        VarcharValue = x.VarcharValue,
                        TextValue = x.TextValue,
                        VersionId = x.Id,
                        NodeName = x.Text,
                        PropertyAlias = x.Alias,
                        PropertyName = x.Name,
                    });

                return result.ToList();
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