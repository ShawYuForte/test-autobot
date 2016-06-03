var app = angular
  .module('automation', ['xml'])
  .config(function ($httpProvider) {
    $httpProvider.interceptors.push('xmlHttpInterceptor');
  });