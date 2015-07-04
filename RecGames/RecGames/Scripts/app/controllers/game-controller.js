app.controller('gameController', ['$scope', 'gameFactory', 'playerFactory', function ($scope, gameFactory, playerFactory) {
    $scope.getRecommendedGames = function () {
        var player = playerFactory.getPlayer();
        gameFactory.postRecommendedGames(player).success(function (data) {
            $scope.gamesToRecommend = data;
        });
    }
}]);

