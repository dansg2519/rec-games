app.factory('playerFactory', ['$http', function($http) {
    var urlBase = '/api/player/';
    var dataFactory = {};
    var player = {};

    dataFactory.getPlayer = function () {
        return player;
    }

    dataFactory.getInfo = function () {
        return $http.get(urlBase + 'info')
            .success(function (data) {
                player['info'] = data;
                return data;
            })
            .error(function (err) {
                return err;
            });
    };

    dataFactory.getOwnedGames = function () {
        return $http.get(urlBase + 'ownedGames')
            .success(function (data) {
                player['ownedGames'] = data;
                return data;
            }).error(function (err) {
                return err;
            });
    }

    dataFactory.getRecentlyPlayedGames = function () {
        return $http.get(urlBase + 'recentlyPlayedGames')
            .success(function (data) {
                player['recentlyPlayedGames'] = data;
                return console.log(data);
            })
            .error(function (err) {
                return err;
            });
    }

    dataFactory.postPlayerPortrait = function (ownedGames) {
        return $http.post(urlBase + 'playerPortrait', ownedGames)
            .success(function (data) {
                player['portrait'] = data;
                return data;
            }).error(function (err) {
                return err;
            });
    }

    return dataFactory;
}]);