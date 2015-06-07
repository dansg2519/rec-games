app.factory('playerFactory', ['$http', function($http) {
    var urlBase = '/api/player/';
    var dataFactory = {};

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
        return $http.get(urlBase + 'ownedgames')
            .success(function (data) {
                return data;
            })
            .error(function (err) {
                return err;
            });
    }

    dataFactory.getRecentlyPlayedGames = function () {
        return $http.get(urlBase + 'recentlyPlayedGames')
            .success(function (data) {
                return data;
            })
            .error(function (err) {
                return err;
            });
    }

    return dataFactory;
}]);