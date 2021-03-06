﻿app.factory('playerFactory', ['$http', function($http) {
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
                player['ownedGames'] = data.owned_games;
                player['recentlyPlayedGames'] = data.recently_played_games;
                player['wishlistGames'] = data.wishlist_games;
                return data;
            }).error(function (err) {
                return err;
            });
    }
    
    dataFactory.postSteamId = function (steamId) {
        return $http.post(urlBase + 'steamId', JSON.stringify(steamId))
            .success(function (data) {
                return data;
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