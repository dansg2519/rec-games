app.controller('playerController', ['$scope', 'playerFactory', function ($scope, playerFactory) {
    $scope.hello = function () {
        return console.log("Hello World");
    }
    
    $scope.info = function () {
        var data = playerFactory.getInfo();
        return console.log(data);
    };

    $scope.ownedGames = function () {
        playerFactory.getOwnedGames;
    };

    $scope.recentlyPlayedGames = playerFactory.getRecentlyPlayedGames;    
    }]);

