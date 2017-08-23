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
        busyCount: 0,
        busy: false,
        foodGroups: [{code: '', description: 'All'}],
        foodItems: [],
        selectedGroupCode: '',
        selectedGroupDescription: 'All',
        searchText: '',
        selectedNutrient: 'None',
        nutrients: []
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
        }
    },
    methods: {
        fetchGroups: function() {
            var _this = this;
            this.busyCount++;
            $.get(makeApi('foodGroups'), function(data) {
                _this.foodGroups = _this.foodGroups.concat(data);
                _this.busyCount--;
            });
        },
        fetchNutrients: function() {
            var _this = this;
            this.busyCount++;
            $.get(makeApi('nutrients'), function(data) {
                _this.nutrients = data;
                _this.selectedNutrient = _this.nutrients[0].tagName;
                _this.busyCount--;
            });
        },
        searchFoods: function() {
            var _this = this;
            this.busyCount++;
            var query = [];
            if (this.selectedGroupCode !== '') {
                query.push(['groupId', this.selectedGroupCode]);
            }
            if (this.searchText !== '') {
                query.push(['search', this.searchText]);
            }
            $.get(makeApi('foods','',query), function(data) {
                _this.foodItems = data;
                _this.busyCount--;
            });
        },
        getTopFoods: function () {
            var _this = this;
            this.busyCount++;
            var query = [];
            if (this.selectedGroupCode !== '') {
                query.push(['groupId', this.selectedGroupCode]);
            }
            $.get(makeApi('nutrients', ['top', this.selectedNutrient], query), function (data) {
                _this.foodItems = data.foodItems;
                _this.busyCount--;
            });
        }
    }

});