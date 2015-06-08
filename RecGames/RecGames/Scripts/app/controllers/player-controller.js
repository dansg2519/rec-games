app.controller('playerController', ['$scope', 'playerFactory', function ($scope, playerFactory) {
    $scope.hello = function () {
        return console.log("Hello World");
    }
    
    $scope.info = function () {
        playerFactory.getInfo().success(function(data) {
            $scope.playerInfo = data.response.players[0];
        });
    };

    $scope.ownedGames = function () {
        playerFactory.getOwnedGames().success(function (data) {
            $scope.playerOwnedGames = data.response.games;
        });
    };

    $scope.recentlyPlayedGames = function () {
        playerFactory.getRecentlyPlayedGames().success(function (data) {
            $scope.playerRecentlyPlayedGames = data.response.games;
        });
    };
}]);

