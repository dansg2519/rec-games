app.factory('gameFactory', ['$http', function ($http) {
    var urlBase = '/api/game/';
    var dataFactory = {};

    dataFactory.postRecommendedGames = function (player) {
        var playerData = {
                             owned_games: player.ownedGames.response.games,
                             player_portrait: angular.fromJson(player.portrait),
                             wishlist_games: player.wishlistGames
        };
        return $http.post(urlBase + 'recommendedGames', playerData)
            .success(function (data) {
                return data;
            })
            .error(function (err) {
                return err;
            });
    }

    return dataFactory;
}]);