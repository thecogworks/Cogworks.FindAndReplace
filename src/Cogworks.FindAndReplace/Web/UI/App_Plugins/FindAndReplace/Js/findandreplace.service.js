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

            return umbRequestHelper.resourcePromise($http.get("/umbraco/backoffice/FindAndReplace/FindAndReplaceAPI/FindPhrase", config),
                "Failed to find phrase");
        }

        function setValue(contentId, propertyAlias, valueField, value) {

            var config = {
                PropertyAlias: propertyAlias,
                ContentId: contentId,
                dataNvarchar: valueField === "dataNvarchar" ? value : "",
                dataNtext: valueField === "dataNtext" ? value : ""
            };

            return umbRequestHelper.resourcePromise($http.post("/umbraco/backoffice/FindAndReplace/FindAndReplaceAPI/SetValue", config),
                "Failed to replace phrase");
        }
    }
})();