'use strict';

angular.module('myApp.search', ['ngRoute'])

    .config(['$routeProvider', function($routeProvider) {
        $routeProvider.when('/search', {
            templateUrl: 'search/search.html',
            controller: 'SearchCtrl'
        });
    }])
    .controller('SearchCtrl', [function() {

}]);

var app = angular.module('search', []);
app.controller('searchCtrl', function($scope,$http) {
    $scope.input = "searchHere";
    $scope.searchText = "Literally Everything";
    $scope.output = [];

    $scope.searchEngine = function () {
        console.log("in here");
        $http.get("https://seniordesign2018dev.azurewebsites.net/api/Calendar/dateLookup?calName=Ablaseau%20376&startTime=2023-10-09%2000%3A00%3A00&range=1")
            .then(function (response) {
                console.log("im here bi");
                //console.log(response.data);
                var appointments = (response.data);
                console.log("Appointments");
                console.log(appointments);
                $scope.searchResults = appointments[0].appointments;

            });

    }
});