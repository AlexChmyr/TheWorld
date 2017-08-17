(function () {

    angular.module("app-trips")
        .controller("tripEditorController", tripEditorController)

    function tripEditorController($routeParams, $http) {
        
        var vm = this;

        vm.tripName = $routeParams.tripName;
        vm.stops = [];
        vm.errorMessage = "";
        vm.isBusy = true;
        vm.newStop = {};

        $http.get("/api/trips/" + vm.tripName + "/stops")
            .then(
                function (response) {
                    angular.copy(response.data, vm.stops);
                    showMap(vm.stops)
                },
                function (error) {
                    vm.errorMessage = "Data loading error ocured: " + error.statusText;
                }
            )
            .finally(function () {
                vm.isBusy = false;
            });

        vm.addStop = function () {
            vm.isBusy = true;

            $http.post("/api/trips/" + vm.tripName + "/stops",vm.newStop)
                .then(
                    function (response) {
                        vm.stops.push(response.data);
                        showMap(vm.stops);
                        vm.newStop = {};
                    },
                    function (error) {
                        vm.errorMessage = "Can't add new stop: " + error.statusText;
                    }
                )
                .finally(function () {
                    vm.isBusy = false;
                });

        }
    }

    function showMap(stops) {

        console.log(stops);

        if(stops && stops.length > 0) {

            var mapedStops = _.map(stops, function (item) {
                return {
                    lat: item.latitude,
                    long: item.longitude,
                    info: item.name,
                }
            })

            console.log(mapedStops);

            travelMap.createMap({
                stops: mapedStops,
                selector: "#map",
                currentStop: 1,
                initialZoom: 3
            });
        }
    }

})()