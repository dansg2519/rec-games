app.controller('playerController', ['$rootScope', '$scope', 'playerFactory', function ($rootScope, $scope, playerFactory) {
    $scope.hello = function () {
        return console.log("Hello World");
    }
    
    playerFactory.getInfo().success(function(data) {
        $scope.playerInfo = data.response.players[0];
    });

    playerFactory.getOwnedGames().success(function (data) {
        $scope.playerOwnedGames = data.owned_games.response.games;

        $scope.recentGamesText = "Recent Games:";
        if (!data.recently_played_games.response.games) {
            $scope.recentGamesText = "No Games played recently.";
        }
        $scope.playerRecentlyPlayedGames = data.recently_played_games.response.games;

        $scope.tagsText = "Loading Portrait Tags...";
        $scope.tagsReady = false;
        playerFactory.postPlayerPortrait(data).success(function (dataPost) {
            $scope.playerPortrait = angular.fromJson(dataPost);
            $scope.tagsText = "Your Common Tags:";
            $scope.tagsReady = true;
            $('.recommend-button').css('z-index', '3');
            $rootScope.getRecommendedGames();
        });
        playerFactory.postPlayerPortrait(data).error(function (dataPost) {
            $scope.tagsReady = true;
            $scope.recommendReady = true;
            $scope.tagsText = "Unable to retrive your Info. Please, verify if your steam account is private";
            $scope.tagsClass = "no-tags-error";
        });
    });
}]);
