app.controller('steamIdController', ['$scope', '$http', function ($scope, $http) {
    var urlBase = '/api/player/';

    $scope.submitSteamId = function (steamId) {
        $http.post(urlBase + 'steamId', steamId.$modelValue)
            .success(function (data) {
                return data;
            })
            .error(function (err) {
                return err;
            });
    }
}]);