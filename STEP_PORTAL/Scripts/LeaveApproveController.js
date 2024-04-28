angular.module('MyApp')
.controller('LeaveApproveController', function ($scope, $http, $timeout, $filter, $window, LeaveApproveService) {



    //alert(angular.toJson($scope.data));

    $scope.UpdateText = 'Update';

    //Default Variable
    $scope.submitted = false;
    $scope.message = '';
    $scope.CompID = '';
    $scope.isFormValid = false;


    $scope.data = {
        Month: '',
        Year: ''
    };


    $scope.months = [
           { name: 'January', id: 1 },
           { name: 'February', id: 2 },
           { name: 'March', id: 3 },
           { name: 'April', id: 4 },
           { name: 'May', id: 5 },
           { name: 'June', id: 6 },
           { name: 'July', id: 7 },
           { name: 'August', id: 8 },
           { name: 'September', id: 9 },
           { name: 'October', id: 10 },
           { name: 'November', id: 11 },
           { name: 'December', id: 12 }]

    ;



    $scope.data.Year = new Date().getFullYear();




    $scope.StatusList = ['APPROVED', 'PENDING', 'CANCELED', 'DECLINED'];



    $scope.editLeaveStatus = function (r, type) {

        if (type == 0) {
            $scope.LeaveStatus = [
          "CANCELED"
            ];
        }
        else {
            $scope.LeaveStatus = [
                       //"APPROVED",
                        "DECLINED"
            ];

        }


        $scope.LeaveRowBak = r;

        $scope.LeaveRow = angular.copy(r);

    }


    $scope.search_by = function () {
        Load_Data();
    };



    function Load_Data() {


        //// Populate GetEmployeeLeaveApproved

        LeaveApproveService.GetEmployeeLeaveApproved($scope.data).then(function (d) {

            // alert(angular.toJson(d.data));

            $scope.EmpList = $filter('filter')(d.data, { ComID: $scope.Company.ID }, true);
            // alert(angular.toJson($scope.EmpList));

        }, function (error) {
            alert('Error GetEmployeeLeaveApproved!');
        });

    }




    // Populate Unit
    LeaveApproveService.GetCompany().then(function (d) {
        $scope.CompanyList = d.data;


        // Populate DeptSec AND  Company Filter
        LeaveApproveService.GetEmpDeptSec().then(function (d) {

            $scope.DepartmentList = d.data;
            $scope.SectionList = d.data;
            $scope.ComID = extractColumn($scope.DepartmentList, 'ComID')

            // alert(angular.toJson(ComID));

        }, function (error) {
            alert('Error!' + error);
        });



    }, function (error) {
        alert('Error!' + error);
    });



    //Update Data
    $scope.LeaveApplicationUpdate = function (data) {

        if ($scope.UpdateText == 'Update') {
            $scope.submitted = true;
            $scope.message = '';


            $scope.LeaveRow = data;

            $scope.LeaveRow.AppliedDate = new Date(parseInt($scope.LeaveRow.AppliedDate.substr(6)));
            $scope.LeaveRow.FrDate = new Date(parseInt($scope.LeaveRow.FrDate.substr(6)));
            $scope.LeaveRow.ToDate = new Date(parseInt($scope.LeaveRow.ToDate.substr(6)));

            $scope.LeaveRow.StatusReasons = 'HR : ' + $scope.LeaveRow.StatusReasons;

            //LeaveInsertMainDatabase
            LeaveApproveService.LeaveDeleteMainDatabase($scope.LeaveRow).then(function (d) {


                if (d == 'Success') {

                    //LeaveApplicationUpdate

                    LeaveApproveService.LeaveApplicationUpdate($scope.LeaveRow).then(function (d) {
                        angular.element('#ModalUpdateStatus').modal('hide');


                        alert('Leave Updated Successfully');

                        // Populate Leave Apply Status
                        Load_Data();
                        //ClearForm();
                    });


                }
                else {

                    alert(d);
                }


            });





        }
    }




    $scope.containsComparator = function (expected, actual) {
        // alert(expected + '_' + actual);
        return actual.indexOf(expected) > -1;
    };



    function extractColumn(arr, column) {
        function reduction(previousValue, currentValue) {
            previousValue.push(currentValue[column]);
            return previousValue;
        }

        return arr.reduce(reduction, []);
    }


    //Clear Form 
    function ClearForm() {
        $scope.User = {};
        $scope.f1.$setPristine(); //here f1 is form name
        $scope.submitted = false;
    }

})




.factory('LeaveApproveService', function ($http, $q) {


    var fac = {};


    fac.GetCompany = function () {
        return $http.get('/Data/GetCompany')
    }


    fac.GetEmpDeptSec = function () {
        return $http.get('/Data/GetEmpDeptSec', {
            params: { Type: 'HRApprove' }
        });
    }



    fac.GetEmployeeLeaveApproved = function (data) {
        //alert(angular.toJson(data));
        return $http.get('/Leave/GetEmployeeLeaveApproved', {
            params: { Month: data.Month, Year: data.Year }
        });
    }



    fac.LeaveApplicationUpdate = function (data) {
        //alert(data);
        var defer = $q.defer();
        $http({
            url: '/Leave/LeaveApplicationUpdate',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        }).success(function (d) {
            // Success callback
            defer.resolve(d);
        }).error(function (e) {
            //Failed Callback
            alert('Error!');
            defer.reject(e);
        });
        return defer.promise;
    }



    fac.LeaveDeleteMainDatabase = function (data) {
        //alert(data);
        var defer = $q.defer();
        $http({
            url: '/Leave/LeaveDeleteMainDatabase',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        }).success(function (d) {
            // Success callback
            defer.resolve(d);
        }).error(function (e) {
            //Failed Callback
            alert('Error!');
            defer.reject(e);
        });
        return defer.promise;
    }


    return fac;
});
