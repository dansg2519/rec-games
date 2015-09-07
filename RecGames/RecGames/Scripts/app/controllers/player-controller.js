app.controller('playerController', ['$scope', 'playerFactory', function ($scope, playerFactory) {
    $scope.hello = function () {
        return console.log("Hello World");
    }
    
    playerFactory.getInfo().success(function(data) {
        $scope.playerInfo = data.response.players[0];
        $scope.showTags = false;
        setTimeout(function () {
            $scope.showTags = true;
        }, 2000);
    });

    playerFactory.getOwnedGames().success(function (data) {
        $scope.playerOwnedGames = data.response.games;
        playerFactory.postPlayerPortrait(data.response.games).success(function (dataPost) {
            $scope.playerPortrait = angular.fromJson(dataPost);
        });
    });

    playerFactory.getRecentlyPlayedGames().success(function (data) {
        $scope.playerRecentlyPlayedGames = data.response.games;
    });
}]);

