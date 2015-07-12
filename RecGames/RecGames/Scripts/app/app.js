﻿var app = angular.module('RecGamesApp', ['ui.router']);

app.config(['$stateProvider', '$urlRouterProvider', '$locationProvider', function ($stateProvider, $urlRouterProvider, $locationProvider) {
    $stateProvider.state('start', {
        url: '/',
        templateUrl: 'steamid.html'
    }).state('player', {
        url: '/player',
        templateUrl: 'player.html'
    });

    $urlRouterProvider.otherwise('/');

    $locationProvider.html5Mode(true);
}]);