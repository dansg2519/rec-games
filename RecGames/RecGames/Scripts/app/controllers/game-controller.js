app.controller('gameController', ['$scope', 'gameFactory', 'playerFactory', 'ngProgressFactory', function ($scope, gameFactory, playerFactory, ngProgressFactory) {

    $scope.getRecommendedGames = function () {
        $scope.progressbar = ngProgressFactory.createInstance();
        var player = playerFactory.getPlayer();
        $scope.progressbar.start();
        gameFactory.postRecommendedGames(player).success(function (data) {
            $scope.progressbar.complete();
            $scope.gamesToRecommend = data.recommendation1;
            $scope.gamesToRecommend2 = data.recommendation2;
        });
    }
}]);

