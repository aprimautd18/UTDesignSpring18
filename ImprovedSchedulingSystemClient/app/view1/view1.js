'use strict';

angular.module('myApp.view1', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/view1', {
    templateUrl: 'view1/view1.html',
    controller: 'View1Ctrl'
  });
}])

.controller('View1Ctrl', [function() {

}]);

var app = angular.module('demo', []);
app.controller('demoCtrl', function($scope,$http) {
    $scope.firstname = "John";
    $scope.lastname = "Doe";
    //$scope.data = [{"id":1,"firstName":"Alex","lastName":"Khun","address":"123 street", "apptTime" : { "startTime" : "8:30am", "endTime" : "9:00am"}},
    //    {"id":2,"firstName":"K","lastName":"Felten","address":"123 street", "apptTime" : { "startTime" : "8:30am", "endTime" : "9:00am"}}];

    $http.get("https://seniordesign2018dev.azurewebsites.net/api/Customer")
        .then(function(response) {
            console.log("im here bi");
            console.log(response.data);
            $scope.data = response.data;
        });
});

