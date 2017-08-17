(function () {

    angular.module("app-trips", ["ngRoute"])
        .config(function ($routeProvider) {
            $routeProvider.when("/", {
                controller: "tripsController",
                controllerAs: "vm",
                templateUrl: "/views/tripsView.html"
            });

            $routeProvider.when("/editor/:tripName", {
                controller: "tripEditorController",
                controllerAs: "vm",
                templateUrl: "/views/tripsEditorView.html"
            });

            $routeProvider.otherwise({
                redirectTo: "/"
            });
        })

})()