(function () {
    'use strict';

    angular
		.module('umbraco')
		.factory('FindAndReplaceService', FindAndReplaceService);

    FindAndReplaceService.$inject = ['$http', 'umbRequestHelper'];

    function FindAndReplaceService($http, umbRequestHelper) {

        var service = {};

        service.SearchForPhrase = searchForPhrase;
        service.SetValue = setValue;

        return service;

        function searchForPhrase(phrase, contentId) {

            var config = {
                params: {
                    phrase: phrase,
                    contentId: contentId
                },
                headers: { 'Accept': 'application/json' }
            };

            return umbRequestHelper.resourcePromise($http.get("/umbraco/backoffice/FindAndReplace/FindAndReplaceApi/FindPhrase", config),
                "Failed to find phrase");
        }

        function setValue(contentId, propertyAlias, valueField, value) {

            var config = {
                PropertyAlias: propertyAlias,
                VersionId: contentId,
                Value: value
            };

            return umbRequestHelper.resourcePromise($http.post("/umbraco/backoffice/FindAndReplace/FindAndReplaceApi/SetValue", config),
                "Failed to replace phrase");
        }
    }
})();