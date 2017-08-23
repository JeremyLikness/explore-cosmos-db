const apiRoot = "http://localhost:5000/api/";

const makeApi = (path, pathArgs, query) => {
    var result = `${apiRoot}${path}/`;
    if (pathArgs && pathArgs.length) {
        result = `${result}${pathArgs.join('/')}/`;
    }
    if (query && query.length) {
        var queries = [];
        for (var idx = 0; idx < query.length; idx++) {
            queries.push(query[idx].join('='));
        }
        result = `${result}?${queries.join('&')}`;
    }
    return result;
}

var app = new Vue({
    el: '#app',
    created: function () {
        this.selectedGroup = this.foodGroups[0];
        this.fetchGroups();
        this.fetchNutrients();
    },
    data: {
        weightSequence: 0,
        weight: null,
        amount: 1,
        amounts: [0.5, 1, 1.5, 2, 2.5, 3, 3.5, 4, 4.5, 5, 5.5, 6, 6.5, 7, 7.5, 8, 8.5, 9, 9.5, 10],
        busyCount: 0,
        busy: false,
        showAlert: false,
        alertText: null,
        foodGroups: [{ code: '', description: 'All' }],
        foodItems: [],
        selectedGroupCode: '',
        selectedGroupDescription: 'All',
        searchText: '',
        selectedNutrient: 'None',
        nutrients: [],
        foodItem: null
    },
    watch: {
        'selectedGroupCode': function () {
            var group = this.foodGroups.find(fg => fg.code === this.selectedGroupCode);
            if (group) {
                this.selectedGroupDescription = group.description;
            }
        },
        'busyCount': function () {
            this.busy = this.busyCount > 0;
        },
        'weightSequence': function () {
            this.weight = this.foodItem.weights.find(w => w.sequence === this.weightSequence);
        }
    },
    methods: {
        fetchGroups: function () {
            var _this = this;
            this.busyCount++;
            $.get(makeApi('foodGroups')).done(function (data) {
                _this.foodGroups = _this.foodGroups.concat(data);
            }).fail(function (err) {
                _this.showAlert = true;
                _this.alertText = err;
            }).always(function () { _this.busyCount--; });
        },
        fetchNutrients: function () {
            var _this = this;
            this.busyCount++;
            $.get(makeApi('nutrients')).done(function (data) {
                _this.nutrients = data;
                _this.selectedNutrient = _this.nutrients[0].tagName;
            }).fail(function (err) {
                _this.showAlert = true;
                _this.alertText = err;
            }).always(function () { _this.busyCount--; });
        },
        searchFoods: function () {
            var _this = this;
            this.busyCount++;
            var query = [];
            if (this.selectedGroupCode !== '') {
                query.push(['groupId', this.selectedGroupCode]);
            }
            if (this.searchText !== '') {
                query.push(['search', this.searchText]);
            }
            $.get(makeApi('foods', '', query)).done(function (data) {
                _this.foodItems = data;
            }).fail(function (err) {
                _this.showAlert = true;
                _this.alertText = err;

            }).always(function () { _this.busyCount--; });
        },
        getTopFoods: function () {
            var _this = this;
            this.busyCount++;
            var query = [];
            if (this.selectedGroupCode !== '') {
                query.push(['groupId', this.selectedGroupCode]);
            }
            $.get(makeApi('nutrients', ['top', this.selectedNutrient], query)).done(function (data) {
                _this.foodItems = data.foodItems;
            }).fail(function (err) {
                _this.showAlert = true;
                _this.alertText = err;
            }).always(function () { _this.busyCount--; });
        },
        getFoodItem: function (foodId) {
            var _this = this;
            this.busyCount++;
            $.get(makeApi('foods', [foodId])).done(function (data) {
                _this.foodItem = data;
                _this.weightSequence = _this.foodItem.weights[0].sequence;
                _this.amount = 1;
            }).fail(function (err) {
                _this.showAlert = true;
                _this.alertText = err;
            }).always(function () { _this.busyCount--; });
        }
    }

});