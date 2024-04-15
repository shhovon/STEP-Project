(function () {
    //Create a Module 
    var app = angular.module('MyApp', ['ngRoute', 'chart.js']);  // Will use ['ng-Route'] when we will implement routing
    //Create a Controller
    //app.controller('HomeController', function ($scope) {  // here $scope is used for share data between view and controller
    //    $scope.Message = "";
    //});

    app.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.when('/User', {
            controller: "UserController"
        })
         .when('/Attendence', {
             controller: "AttendenceController"
             })
         .when('/Leave', {
             controller: "LeaveController"
         })
          .when('/Album', {
              controller: "AlbumController"
          })
        .when('/BusSchedule', {
            controller: "BusScheduleController"
        })
        .when('/EmpOTRequisition', {
            controller: "EmpOTRequisitionController"
        })
        .when('/Fabric', {
            controller: "FabricController"
         })
        .when('/Data', {
                controller: "DataController"
            }).otherwise({
            //templateUrl: "/views/main.html",
            controller: "HomeController"
        })
    }]);

     
    //app.config([
    //"$stateProvider", "$urlRouterProvider", "$locationProvider",
    //function ($stateProvider, $urlRouterProvider, $locationProvider) {

    //    $stateProvider
    //        .state('/', {
    //            url: "/",
    //            templateUrl: "/Home/index"
    //            ,controller: 'HomeController'
    //        })
    //        .state('/About', {
    //            url: "/Home/About",
    //            templateUrl: "/Home/About"
    //        })
    //        .state('/Contact', {
    //            url: "/Home/Contact",
    //            templateUrl: "/Home/Contact"
    //        })

    //    $urlRouterProvider.otherwise("/");
    //    $locationProvider.html5Mode({
    //        enabled: true,
    //        requireBase: false
    //    });
    //}
    //]);

    //app.config(function ($routeProvider) {
    //    $routeProvider.
    //    when('/firstPage', {
    //        templateUrl: 'routedemo/first',
    //        controller: 'routeDemoFirstController'
    //    }).
    //    when('/secondPage', {
    //        templateUrl: 'routedemo/second',
    //        controller: 'routeDemoSecondController'
    //    })
    //})
   
    //app.controller('UserController', function ($scope) {  // here $scope is used for share data between view and controller
    //    $scope.Message = "";
    //});

    //app.controller('DataController', function ($scope) {  // here $scope is used for share data between view and controller
    //    $scope.Message = "";
    //});



    app.filter('fetchas', function ($filter) {
        return function (collection) {
            var today = $filter('date')(new Date(), 'MMM dd yyyy');
            var newCollection = [];

            angular.forEach(collection, function (input) {
                var formatted = $filter('date')(input, 'MMM dd yyyy');
                if (formatted == today) {
                    newCollection.push(input);
                }
            });
            console.log(newCollection);
            return newCollection;
        };
    });



    // int  range Filter
    app.filter('range', function () {
        return function (input, min, max) {
            min = parseInt(min);
            max = parseInt(max);
            for (var i = min; i <= max; i++)
                input.push(i);
            return input;
        };
    });


    app.filter('dateRange', function () {
        return function( items, fromDate, toDate ) {
            var filtered = [];
            //here you will have your desired input
           // console.log(fromDate, toDate);
           
            var from_date = Date.parse(fromDate);
            var to_date = Date.parse(toDate);
            console.log(from_date, to_date);
           
            angular.forEach(items, function (item) {
                console.log(item.Created_date.slice(6, -2));
                if (item.Created_date.slice(6, -2) > from_date && item.Created_date.slice(6, -2) < to_date) {
                    filtered.push(item);
                }
            });
            return filtered;
        };
    });


    // tooltip
    app.directive('tooltip', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                $(element).hover(function () {
                    // on mouseenter
                    $(element).tooltip('show');
                }, function () {
                    // on mouseleave
                    $(element).tooltip('hide');
                });
            }
        };
    });


    var BloodGroupName = [

        {
            id: 'UN',
            name: 'Unknown'
        },

           {
               id: 'A+',
               name: 'A Positive'
           },
           {
               id: 'A-',
               name: 'A Negative'
           },
           {
               id: 'B+',
               name: 'B Positive'
           },
           {
               id: 'B-',
               name: 'B Negative'
           },
           {
               id: 'O+',
               name: 'O Positive'
           },
           {
               id: 'O-',
               name: 'O Negative'
           },
           {
               id: 'AB+',
               name: 'AB Positive'
           },
           {
               id: 'AB-',
               name: 'AB Negative'
           }
    ];

    // int  Blood Group Filter
    app.filter('BloodGroupList', function () {
        return function (input) {
            input = BloodGroupName;
            return input;
        };
    });


    app.filter('groupBy', function ($timeout) {
        return function (data, key) {
            if (!key) return data;
            var outputPropertyName = '__groupBy__' + key;
            if (!data[outputPropertyName]) {
                var result = {};
                for (var i = 0; i < data.length; i++) {
                    if (!result[data[i][key]])
                        result[data[i][key]] = [];
                    result[data[i][key]].push(data[i]);
                }
                Object.defineProperty(result, 'length', { enumerable: false, value: Object.keys(result).length });
                Object.defineProperty(data, outputPropertyName, { enumerable: false, configurable: true, writable: false, value: result });
                $timeout(function () { delete data[outputPropertyName]; }, 0, false);
            }
            return data[outputPropertyName];
        };
    });




    app.filter('unique', function () {

      
        // we will return a function which will take in a collection
        // and a keyname
        return function (collection, keyname) {
            // we define our output and keys array;
            var output = [],
                keys = [];

            // we utilize angular's foreach function
            // this takes in our original collection and an iterator function
            angular.forEach(collection, function (item) {
                // we check to see whether our object exists
                var key = item[keyname];
                // if it's not already part of our keys array
                if (keys.indexOf(key) === -1) {
                    // add it to our keys array
                    keys.push(key);
                    // push this item to our final output array
                    output.push(item);
                }
            });
            // return our array which should be devoid of
            // any duplicates
            return output;
        };
    });





    // limitChar

    app.filter('limitChar', function () {
        return function (content, length, tail) {
            if (isNaN(length))
                length = 50;

            if (tail === undefined)
                tail = "...";

            if (content.length <= length || content.length - tail.length <= length) {
                return content;
            }
            else {
                return String(content).substring(0, length - tail.length) + tail;
            }
        };
    });

    // password confirmation 
    app.directive("compareTo", function () {
        return {
            require: "ngModel",
            scope:
            {
                confirmPassword: "=compareTo"
            },
            link: function (scope, element, attributes, modelVal) {
                modelVal.$validators.compareTo = function (val) {
                    return val == scope.confirmPassword;
                };
                scope.$watch("confirmPassword", function () {
                    modelVal.$validate();
                });
            }
        };
    });



    // DatePicker -> NgModel
    app.directive('datePicker', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                $(element).datetimepicker({
                    //locale: 'DE',
                    //format: 'hh:mm',
                    //viewMode: 'years',
                    format: 'DD-MMM-YYYY',
                    ignoreReadonly: false,
                    parseInputDate: function (data) {
                        if (data instanceof Date) {
                            return moment(data);
                        } else {
                            return moment(new Date(data));
                        }
                    },
                    //maxDate: new Date()
                });

                $(element).on("dp.change", function (e) {
                    ngModel.$viewValue = e.date;
                    ngModel.$commitViewValue();
                });
            }
        };
    });



        // DatePicker Input NgModel->DatePicker
        app.directive('datePickerInput', function () {
            return {
                require: 'ngModel',
                link: function (scope, element, attr, ngModel) {
                    // Trigger the Input Change Event, so the Datepicker gets refreshed
                    scope.$watch(attr.ngModel, function (value) {
                        if (value) {
                            element.trigger("change");
                        }
                    });

                }
            };
        });



    // TimePicker -> NgModel
    app.directive('timePicker', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                $(element).datetimepicker({
                    //locale: 'DE',
                    format: 'HH:mm',
                    ignoreReadonly: false,
                    parseInputDate: function (data) {
                        if (data instanceof Date) {
                            return moment(data);
                        } else {
                            return moment(new Date(data));
                        }
                    },
                    //maxDate: new Date()
                });

                $(element).on("dp.change", function (e) {                   
                    ngModel.$viewValue = e.date;
                    ngModel.$commitViewValue();
                });
            }
        };
    });


    // time Input NgModel->DatePicker
    app.directive('timePickerInput', function () {
        
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                // Trigger the Input Change Event, so the Datepicker gets refreshed
                scope.$watch(attr.ngModel, function (value) {
                    if (value) {
                        element.trigger("change");
                    }
                });
            }
        };
    });




    // datetimePicker -> NgModel
    app.directive('datetimePicker', function () {
        return {
            require: 'ngModel',
            link: function (scope, element, attr, ngModel) {
                $(element).datetimepicker({
                    //locale: 'DE',
                    format: 'DD-MM-YYYY hh:mm',
                    ignoreReadonly: false,
                    parseInputDate: function (data) {
                        if (data instanceof Date) {
                            return moment(data);
                        } else {
                            return moment(new Date(data));
                        }
                    },
                    //maxDate: new Date()
                });

                $(element).on("dp.change", function (e) {
                    //var time = $filter('date')(e.date, 'DD-MM-YYYY HH:mm:ss')
                    ngModel.$viewValue = e.date;
                    ngModel.$commitViewValue();
                });
            }
        };
    });



    app.directive('preventRightClick', [

            function () {
                return {
                    restrict: 'A',
                    link: function ($scope, $ele) {
                        $ele.bind("contextmenu", function (e) {
                            e.preventDefault();
                        });
                    }
                };
            }
    ])


    app.directive("bindImageResults", function($compile) {
      return {
          restrict: 'A',
          link: function(scope, ele) {
              scope.$watch('results', function(newVal, oldVal) {
                  console.log(newVal,oldVal);
                  if (!newVal) return;
                  ele.append(newVal)
              })
          }
      }
    })

    // TODO: Move to polyfill?
    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/g, '');
        };
    }



    ////  toggle
    //app.directive('toggle', function(){
    //    return {
    //        restrict: 'A',
    //        link: function(scope, element, attrs){
    //            if (attrs.toggle=="tooltip"){
    //                $(element).tooltip();
    //            }
    //            if (attrs.toggle=="popover"){
    //                $(element).popover();
    //            }
    //        }
    //    };
    //})

    // File Upload
    app.directive("ngFileSelect", function (fileReader, $timeout) {
        return {
            scope: {
                ngModel: '='
            },
            link: function ($scope, el) {
                function getFile(file) {
                    fileReader.readAsDataUrl(file, $scope)
                      .then(function (result) {
                          $timeout(function () {
                              $scope.ngModel = result;
                          });
                      });
                }

                el.bind("change", function (e) {
                    var file = (e.srcElement || e.target).files[0];
                    getFile(file);
                });
            }
        };
    });



    app.directive('file', function () {
        return {
            scope: {
                file: '='
            },
            link: function (scope, el, attrs) {
                el.bind('change', function (event) {
                    var file = event.target.files[0];
                    scope.file = file ? file : undefined;
                    scope.$apply();
                });
            }
        };
    });

    app.factory("fileReader", function ($q, $log) {
        var onLoad = function (reader, deferred, scope) {
            return function () {
                scope.$apply(function () {
                    deferred.resolve(reader.result);
                });
            };
        };

        var onError = function (reader, deferred, scope) {
            return function () {
                scope.$apply(function () {
                    deferred.reject(reader.result);
                });
            };
        };

        var onProgress = function (reader, scope) {
            return function (event) {
                scope.$broadcast("fileProgress", {
                    total: event.total,
                    loaded: event.loaded
                });
            };
        };

        var getReader = function (deferred, scope) {
            var reader = new FileReader();
            reader.onload = onLoad(reader, deferred, scope);
            reader.onerror = onError(reader, deferred, scope);
            reader.onprogress = onProgress(reader, scope);
            return reader;
        };

        var readAsDataURL = function (file, scope) {
            var deferred = $q.defer();

            var reader = getReader(deferred, scope);
            reader.readAsDataURL(file);

            return deferred.promise;
        };

        return {
            readAsDataUrl: readAsDataURL
        };
    });


    app.directive('customOnChange', function() {
  return {
    restrict: 'A',
    link: function (scope, element, attrs) {
      var onChangeHandler = scope.$eval(attrs.customOnChange);
      element.bind('change', onChangeHandler);
    }
  };
});



    app.directive('checkImage', function ($http) {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                attrs.$observe('ngSrc', function (ngSrc) {
                    $http.get(ngSrc).success(function () {
                        //alert('image exist');
                    }).error(function () {
                        //alert('image not exist');
                        element.attr('src', '../images/users/blank-profile-picture.png'); // set default image
                    });
                });
            }
        };
    });




 

    //directive infinityscroll
    app.directive('infinityscroll', function () {
        return {
            restrict: 'A',
            link: function (scope, element, attrs) {
                element.bind('scroll', function () {
                    if ((element[0].scrollTop + element[0].offsetHeight) == element[0].scrollHeight) {
                        //scroll reach to end
                        scope.$apply(attrs.infinityscroll)
                    }
                });
            }
        }
    });


    /**
	 * A replacement utility for internationalization very similar to sprintf.
	 *
	 * @param replace {mixed} The tokens to replace depends on type
	 *  string: all instances of $0 will be replaced
	 *  array: each instance of $0, $1, $2 etc. will be placed with each array item in corresponding order
	 *  object: all attributes will be iterated through, with :key being replaced with its corresponding value
	 * @return string
	 *
	 * @example: 'Hello :name, how are you :day'.format({ name:'John', day:'Today' })
	 * @example: 'Records $0 to $1 out of $2 total'.format(['10', '20', '3000'])
	 * @example: '$0 agrees to all mentions $0 makes in the event that $0 hits a tree while $0 is driving drunk'.format('Bob')
	 */
    function format(value, replace) {
        if (!value) {
            return value;
        }
        var target = value.toString();
        if (replace === undefined) {
            return target;
        }
        if (!angular.isArray(replace) && !angular.isObject(replace)) {
            return target.split('$0').join(replace);
        }
        var token = angular.isArray(replace) && '$' || ':';

        angular.forEach(replace, function (value, key) {
            target = target.split(token + key).join(value);
        });
        return target;
    }



    app.value('customSelectDefaults', {
        displayText: 'Select...',
        emptyListText: 'There are no items to display',
        emptySearchResultText: 'No results match "$0"',
        addText: 'Add',
        searchDelay: 300
    });

    app.directive('customSelect', ['$parse', '$compile', '$timeout', '$q', 'customSelectDefaults', function ($parse, $compile, $timeout, $q, baseOptions) {
        var CS_OPTIONS_REGEXP = /^\s*(.*?)(?:\s+as\s+(.*?))?\s+for\s+(?:([\$\w][\$\w\d]*))\s+in\s+([\s\S]+?)(?:\s+track\s+by\s+([\s\S]+?))?$/;
        var VALUES_REGEXP = /^.+?(?=\||$)/;

        return {
            restrict: 'A',
            require: 'ngModel',
            link: function (scope, elem, attrs, controller) {
                var customSelect = attrs.customSelect;
                if (!customSelect) {
                    throw new Error('Expected custom-select attribute value.');
                }

                var match = customSelect.match(CS_OPTIONS_REGEXP);

                if (!match) {
                    throw new Error("Expected expression in form of " +
						"'_select_ (as _label_)? for _value_ in _collection_[ track by _id_]'" +
						" but got '" + customSelect + "'.");
                }

                elem.addClass('dropdown custom-select');

                // Ng-Options break down
                var displayFn = $parse(match[2] || match[1]),
					valueName = match[3],
					valueFn = $parse(match[2] ? match[1] : valueName),
					values = match[4],
					valuesFn = $parse(values),
					track = match[5],
					trackByExpr = track ? " track by " + track : "",
					dependsOn = attrs.csDependsOn;

                var options = getOptions(),
					timeoutHandle,
					lastSearch = '',
					focusedIndex = -1,
						matchMap = {};

                var itemTemplate = elem.html().trim() || '{{' + (match[2] || match[1]) + '}}',

					dropdownTemplate =
					'<a class="dropdown-toggle" data-toggle="dropdown" href ng-class="{ disabled: disabled }">' +
						'<span> &ensp;{{displayText}}</span>' +
						'<b></b>' +
					'</a>' +
					'<div class="dropdown-menu">' +
						'<div stop-propagation="click" class="custom-select-search">' +
							'<input class="' + attrs.selectClass + '" type="text" autocomplete="off" ng-model="searchTerm" />' +
						'</div>' +
						'<ul role="menu">' +
							'<li role="presentation" ng-repeat="' + valueName + ' in matches' + trackByExpr + '">' +
								'<a role="menuitem" tabindex="-1" href ng-click="select(' + valueName + ')">' +
									itemTemplate +
								'</a>' +
							'</li>' +
							'<li ng-hide="matches.length" class="empty-result" stop-propagation="click">' +
								'<em class="muted">' +
									'<span ng-hide="searchTerm">{{emptyListText}}</span>' +
									'<span class="word-break" ng-show="searchTerm">{{ format(emptySearchResultText, searchTerm) }}</span>' +
								'</em>' +
							'</li>' +
						'</ul>' +
						'<div class="custom-select-action">' +
							(typeof options.onAdd === "function" ?
							'<button type="button" class="btn btn-warning btn-block add-button" ng-click="add()">{{addText}}</button>' : '') +
						'</div>' +
					'</div>';

                // Clear element contents
                elem.empty();

                // Create dropdown element
                var dropdownElement = angular.element(dropdownTemplate),
					anchorElement = dropdownElement.eq(0).dropdown(),
					inputElement = dropdownElement.eq(1).find(':text'),
					ulElement = dropdownElement.eq(1).find('ul');

                // Create child scope for input and dropdown
                var childScope = scope.$new(true);
                configChildScope();

                // Click event handler to set initial values and focus when the dropdown is shown
                anchorElement.on('click', function (event) {
                    if (childScope.disabled) {
                        return;
                    }
                    childScope.$apply(function () {
                        lastSearch = '';
                        childScope.searchTerm = '';
                    });

                    focusedIndex = -1;
                    inputElement.focus();

                    // If filter is not async, perform search in case model changed
                    //if (!options.async) {
                    //	getMatches();
                    //}
                });

                if (dependsOn) {
                    scope.$watch(dependsOn, function (newVal, oldVal) {
                        if (newVal !== oldVal) {
                            childScope.matches = [];
                            childScope.select(undefined);
                        }
                    });
                }

                // Event handler for key press (when the user types a character while focus is on the anchor element)
                anchorElement.on('keypress', function (event) {
                    if (!(event.altKey || event.ctrlKey)) {
                        anchorElement.click();
                    }
                });

                // Event handler for Esc, Enter, Tab and Down keys on input search
                inputElement.on('keydown', function (event) {
                    if (!/(13|27|40|^9$)/.test(event.keyCode)) return;
                    event.preventDefault();
                    event.stopPropagation();

                    switch (event.keyCode) {
                        case 27: // Esc
                            anchorElement.dropdown('toggle');
                            break;
                        case 9: // Tab
                        case 13: // Enter
                            selectFromInput();
                            childScope.searchTerm = undefined;
                            break;
                        case 40: // Down
                            focusFirst();
                            break;
                    }
                });

                // Event handler for Up and Down keys on dropdown menu
                ulElement.on('keydown', function (event) {
                    if (!/(38|40)/.test(event.keyCode)) return;
                    event.preventDefault();
                    event.stopPropagation();

                    var items = ulElement.find('li > a');

                    if (!items.length) return;
                    if (event.keyCode == 38) focusedIndex--;                                    // up
                    if (event.keyCode == 40 && focusedIndex < items.length - 1) focusedIndex++; // down
                    //if (!~focusedIndex) focusedIndex = 0;

                    if (focusedIndex >= 0) {
                        items.eq(focusedIndex)
							.focus();
                    } else {
                        focusedIndex = -1;
                        inputElement.focus();
                    }
                });

                resetMatches();

                // Compile template against child scope
                $compile(dropdownElement)(childScope);
                elem.append(dropdownElement);

                // When model changes outside of the control, update the display text
                controller.$render = function () {
                    setDisplayText();
                };

                // Watch for changes in the default display text
                childScope.$watch(getDisplayText, setDisplayText);

                childScope.$watch(function () { return elem.attr('disabled'); }, function (value) {
                    childScope.disabled = value;
                });

                childScope.$watch('searchTerm', function (newValue) {
                    if (timeoutHandle) {
                        $timeout.cancel(timeoutHandle);
                    }

                    var term = (newValue || '').trim();
                    timeoutHandle = $timeout(function () {
                        getMatches(term);
                    },
					// If empty string, do not delay
					(term && options.searchDelay) || 0);
                });

                if (!options.async) {
                    var m = values.match(VALUES_REGEXP);
                    if (m) {
                        var originalValues = m[0];
                        scope.$watchCollection(originalValues, function (value) {
                            if (angular.isArray(value)) {

                                getMatches();
                            }
                        });
                    }
                }

                // Support for autofocus
                if ('autofocus' in attrs) {
                    anchorElement.focus();
                }

                var needsDisplayText;
                function setDisplayText() {
                    var locals = {};
                    locals[valueName] = controller.$modelValue;
                    var text = displayFn(scope, locals);

                    if (text === undefined) {
                        var map = matchMap[hashKey(controller.$modelValue)];
                        if (map) {
                            text = map.label;
                        }
                    }

                    needsDisplayText = !text;
                    childScope.displayText = text || options.displayText;
                }

                function getOptions() {
                    return angular.extend({}, baseOptions, scope.$eval(attrs.customSelectOptions));
                }

                function getDisplayText() {
                    options = getOptions();
                    return options.displayText;
                }

                function focusFirst() {
                    var opts = ulElement.find('li > a');
                    if (opts.length > 0) {
                        focusedIndex = 0;
                        opts.eq(0).focus();
                    }
                }

                // Selects the first element on the list when the user presses Enter inside the search input
                function selectFromInput() {
                    var opts = ulElement.find('li > a');
                    if (opts.length > 0) {
                        var ngRepeatItem = opts.eq(0).scope();
                        var item = ngRepeatItem[valueName];
                        childScope.$apply(function () {
                            childScope.select(item);
                        });
                        anchorElement.dropdown('toggle');
                    }
                }

                function getMatches(searchTerm) {
                    if (searchTerm === undefined) {
                        searchTerm = (childScope.searchTerm || "").trim();
                    }
                    var locals = { $searchTerm: searchTerm }
                    $q.when(valuesFn(scope, locals)).then(function (matches) {
                        if (!matches) return;

                        if (searchTerm === inputElement.val().trim()/* && hasFocus*/) {
                            matchMap = {};
                            childScope.matches.length = 0;
                            for (var i = 0; i < matches.length; i++) {
                                locals[valueName] = matches[i];
                                var value = valueFn(scope, locals),
									label = displayFn(scope, locals);

                                matchMap[hashKey(value)] = {
                                    value: value,
                                    label: label/*,
									model: matches[i]*/
                                };

                                childScope.matches.push(matches[i]);
                            }
                            //childScope.matches = matches;
                        }

                        if (needsDisplayText) setDisplayText();
                    }, function () {
                        resetMatches();
                    });
                }

                function resetMatches() {
                    childScope.matches = [];
                    focusedIndex = -1;
                };

                function configChildScope() {
                    childScope.addText = options.addText;
                    childScope.emptySearchResultText = options.emptySearchResultText;
                    childScope.emptyListText = options.emptyListText;

                    childScope.select = function (item) {
                        var locals = {};
                        locals[valueName] = item;
                        var value = valueFn(childScope, locals);
                        //setDisplayText(displayFn(scope, locals));
                        childScope.displayText = displayFn(childScope, locals) || options.displayText;
                        controller.$setViewValue(value);

                        anchorElement.focus();

                        typeof options.onSelect === "function" && options.onSelect(item);
                    };

                    childScope.add = function () {
                        $q.when(options.onAdd(childScope.searchTerm), function (item) {
                            if (!item) return;

                            var locals = {};
                            locals[valueName] = item;
                            var value = valueFn(scope, locals),
								label = displayFn(scope, locals);

                            matchMap[hashKey(value)] = {
                                value: value,
                                label: label/*,
									model: matches[i]*/
                            };

                            childScope.matches.push(item);
                            childScope.select(item);
                        });
                    };

                    childScope.format = format;

                    setDisplayText();
                }

                var current = 0;
                function hashKey(obj) {
                    if (obj === undefined) return 'undefined';

                    var objType = typeof obj,
						key;

                    if (objType == 'object' && obj !== null) {
                        if (typeof (key = obj.$$hashKey) == 'function') {
                            // must invoke on object to keep the right this
                            key = obj.$$hashKey();
                        } else if (key === undefined) {
                            key = obj.$$hashKey = 'cs-' + (current++);
                        }
                    } else {
                        key = obj;
                    }

                    return objType + ':' + key;
                }
            }
        };
    }]);

    app.directive('stopPropagation', function () {
        return {
            restrict: 'A',
            link: function (scope, elem, attrs, ctrl) {
                var events = attrs['stopPropagation'];
                elem.bind(events, function (event) {
                    event.stopPropagation();
                });
            }
        };
    });

})();