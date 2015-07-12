app.controller('steamIdController', ['$scope', '$state', 'playerFactory', function ($scope, $state, playerFactory) {
    $scope.submitSteamId = function(steamId) {
        playerFactory.postSteamId(steamId.$modelValue).success(function (data){
            $scope.validSteamId = data;
            if ($scope.validSteamId) {
                $state.go("player");
            } else {
                steamId.$setValidity("steamid_invalid", false);
            }
        });
    }
}]);