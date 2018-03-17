const apiRoot = "https://pathtoyourapi";

// helper method to make it easier to build url paths
var app = new Vue({

    el: '#app',

    created: function () {
        this.fetchGroups();
    },

    data: {
        busyCount: 0, // used to show loading 
        busy: false, // updated as busyCount changes
        showAlert: false, // true when an error occurred
        alertText: null, // json of the error (not very informative as is)
        foodGroups: [], // list of food groups
    },

    watch: {

        // set busy flag when count of pending asynchronous operations changes
        // could change busy to a computed field too 
        'busyCount': function () {
            this.busy = this.busyCount > 0;
        }
    },

    methods: {

        // get the group list 
        fetchGroups: function () {
            var _this = this;
            this.busyCount++;
            $.get(apiRoot).done(function (data) {
                _this.foodGroups = _this.foodGroups.concat(data);
            }).fail(function (err) {
                _this.showAlert = true;
                _this.alertText = err;
            }).always(function () { _this.busyCount--; });
        }
    }

});