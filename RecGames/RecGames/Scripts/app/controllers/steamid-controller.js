app.controller('steamIdController', ['$scope', 'playerFactory', function ($scope, playerFactory) {
    $scope.submitSteamId = function(steamId) {
        playerFactory.postSteamId(steamId.$modelValue).success(function (data){
            $scope.validSteamId = data;
            if ($scope.validSteamId) {
                $scope.signup_form.$valid = true;
            }
        });
    }
}]);