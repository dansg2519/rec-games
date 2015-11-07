app.controller('gameController', ['$rootScope', '$scope', 'gameFactory', 'playerFactory', 'ngProgressFactory', function ($rootScope, $scope, gameFactory, playerFactory, ngProgressFactory) {

    $rootScope.getRecommendedGames = function () {
        $scope.progressbar = ngProgressFactory.createInstance();
        $scope.recommendReady = false;
        var player = playerFactory.getPlayer();
        $scope.progressbar.start();
        gameFactory.postRecommendedGames(player).success(function (data) {
            $scope.progressbar.complete();
            $scope.gamesToRecommend = data.recommendation;
            $scope.recommendReady = true;
        });
    }
}]);

