app.controller('playerController', ['$rootScope', '$scope', 'playerFactory', function ($rootScope, $scope, playerFactory) {
    $scope.hello = function () {
        return console.log("Hello World");
    }
    
    playerFactory.getInfo().success(function(data) {
        $scope.playerInfo = data.response.players[0];
    });

    playerFactory.getOwnedGames().success(function (data) {
        if (data.owned_games) {
            $scope.playerOwnedGames = data.owned_games.response.games;
        }
        
        $scope.recentGamesText = "Recent Games:";
        $scope.ownedGamesText = "Owned Games:";
        $scope.wishlistGamesText = "Wishlist Games:"
        if (data.recently_played_games) {
            $scope.playerRecentlyPlayedGames = data.recently_played_games.response.games;
            if (!$scope.playerRecentlyPlayedGames) {
                $scope.recentGamesText = "No recent Games played.";
            }
            
        }

        if (data.wishlist_games) {
            console.log(data.wishlist_games);
            $scope.playerWishlistGames = data.wishlist_games;
        }
        else {
            $scope.wishlistGamesText = "No games on wishlist."
        }

        $scope.tagsText = "Loading Portrait Tags...";
        $scope.tagsReady = false;

        if ($scope.playerOwnedGames) {
            console.log("has games");
            playerFactory.postPlayerPortrait(data).success(function (dataPost) {
                $scope.playerPortrait = angular.fromJson(dataPost);
                $scope.tagsText = "Your Common Tags:";
                $scope.tagsReady = true;
                $('.recommend-button').css('z-index', '3');
                $rootScope.getRecommendedGames();
            });
        }
        else {
            console.log("no games");
            playerFactory.postPlayerPortrait(data).success(function (dataPost) {
                $scope.tagsReady = true;
                $scope.recommendReady = true;
                $scope.tagsText = "Unable to retrive your Info. Please, verify if your steam account is private";
                $scope.tagsClass = "no-tags-error";
            });
        }
        playerFactory.getOwnedGames(data).error(function (dataPost) {
            console.log("error");
            $scope.tagsReady = true;
            $scope.recommendReady = true;
            $scope.steamErrorText = "Unable to connect to Steam. Please, try again later.";
            $scope.tagsText = "Error";
            $scope.tagsClass = "no-tags-error";
        });
    });
}]);
