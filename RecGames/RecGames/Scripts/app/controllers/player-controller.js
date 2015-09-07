app.controller('playerController', ['$scope', 'playerFactory', function ($scope, playerFactory) {
    $scope.hello = function () {
        return console.log("Hello World");
    }
    
    playerFactory.getInfo().success(function(data) {
        $scope.playerInfo = data.response.players[0];
    });

    playerFactory.getOwnedGames().success(function (data) {
        $scope.playerOwnedGames = data.response.games;
        $scope.tagsText = "Loading Portrait Tags...";
        playerFactory.postPlayerPortrait(data.response.games).success(function (dataPost) {
            $scope.playerPortrait = angular.fromJson(dataPost);
            $scope.tagsText = "Your Common Tags:";
            $('.recommend-button').css('z-index', '3');
            
        });
    });

    playerFactory.getRecentlyPlayedGames().success(function (data) {
        $scope.playerRecentlyPlayedGames = data.response.games;
        $scope.recentGamesText = "Recent Games:";
        if (!data.response.games) {
            $scope.recentGamesText = "No recent Games played.";
        }
    });
}]);

