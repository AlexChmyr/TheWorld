(function () {

    angular.module("app-trips")
        .controller("tripsController", tripsController)

    function tripsController($http) {
        
        var vm = this;
        vm.trips = [];
        vm.errorMessage = "";
        vm.isBusy = true;


        // Show trips list
        $http.get("/api/trips")
            .then(
            function (response) {
                angular.copy(response.data, vm.trips);
            },
            function (error) {
                vm.errorMessage = "Data loading error ocured: " + error.statusText;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        vm.newTrip = {};

        // Add new trip
        vm.addTrip = function () {

            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip)
            .then(
            function (response) {
                vm.trips.push(response.data)
                vm.newTrip = {};
            },
            function (error) {
                vm.errorMessage = "Can't create new trip: " + error.statusText;
            })
            .finally(function () {
                vm.isBusy = false;
            });

        }
    }

})()