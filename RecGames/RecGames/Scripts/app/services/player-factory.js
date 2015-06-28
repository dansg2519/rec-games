app.factory('playerFactory', ['$http', function($http) {
    var urlBase = '/api/player/';
    var dataFactory = {};
    var postPlayerPortrait = {};

    dataFactory.getInfo = function () {
        return $http.get(urlBase + 'info')
            .success(function (data) {
                return data;
            })
            .error(function (err) {
                return err;
            });
    };

    dataFactory.getOwnedGames = function () {
        return $http.get(urlBase + 'ownedGames')
            .success(function (data) {
                return data;
            }).error(function (err) {
                return err;
            });
    }

    dataFactory.getRecentlyPlayedGames = function () {
        return $http.get(urlBase + 'recentlyPlayedGames')
            .success(function (data) {
                return console.log(data);
            })
            .error(function (err) {
                return err;
            });
    }

    dataFactory.postPlayerPortrait = function (ownedGames) {
        return $http.post(urlBase + 'playerPortrait', ownedGames)
            .success(function (data) {
                return data;
            }).error(function (err) {
                return err;
            });
    }

    return dataFactory;
}]);