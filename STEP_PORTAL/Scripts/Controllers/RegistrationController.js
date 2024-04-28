angular.module('MyApp') 
.controller('RegistationController', function ($scope, $window, $filter,RegistrationService) {
    
    
    //alert(angular.toJson($scope.data));


    $scope.submitText = "Save";

    $scope.submitted = false;
    $scope.message = '';
    $scope.isFormValid = false;
    $scope.User = {
        RegId: '',      
        Password: '',
        EmailID: '',
        MobileNoPerson: '',
        PhoneExt: '',
        Password: '',
        DeptHead: '',
        ReportSuper: '',
        DateofBirth: new Date("1985-01-01"),
        BloodGroup: '',
        Role: '1',
        Registerd: true,
        YsnActive: true,
        Created_date: new Date(),
        last_login_date: new Date()
    
 
    };
 
 

    // Populate Unit
    RegistrationService.GetCompany().then(function (d) {
        $scope.CompanyList = d.data;
    }, function (error) {
        alert('Error GetCompany!');
    });


    $scope.GetCompID = function () {
        $scope.User.COMID = $scope.Company.ID;
        $scope.EmployeeDetail_Reg = $filter('filter')($scope.EmployeeDetail, { ComID: $scope.Company.ID }, true);
    };
   
    
   
    angular.element(document).ready(function () {
    var ax = new ActiveXObject("WScript.Network");
    alert('User: ' + ax.UserName);
    alert('Computer: ' + ax.ComputerName);
    });


    //// Populate EmployeeDetail
    RegistrationService.GetEmployeeDetail().then(function (d) {
        $scope.EmployeeDetail = d.data;
        $scope.UserCode = '';
        $scope.ReportSuper = '';
        $scope.DeptHead = ''
        
    }, function (error) {
        alert('Error GetEmployeeDetail!');
    });



    //Get EmployeeName from Employee Code

        $scope.GetEmployeeID = function (ID) {
        RegistrationService.GetEmployeeName(ID).then(function (d) {
            $scope.User.Employeename = d.data[0].Name;

        }, function (error) {
            alert('Error GetEmployeeID!');
        });

    }
    

    //validates form on client side
    $scope.$watch('f1.$valid', function (newValue) {
        $scope.isFormValid = newValue;
    });


    //Save Data
    $scope.SaveData = function (data) {

        if ($scope.submitText == 'Save') {
            $scope.submitted = true;
            $scope.message = '';
         
          
            if ($scope.isFormValid) {
               
                $scope.User = data;


                $scope.User.RegId = $scope.UserCode.RegId;   

                $scope.User.ReportSuper = $scope.ReportSuper.RegId;
                $scope.User.DeptHead = $scope.DeptHead.RegId;
                $scope.User.BloodGroup = $scope.BloodGroup.id; 
                
                RegistrationService.SaveFormData($scope.User).then(function (d) {                 
                    if (d == 'Success') {
                        alert('You have successfully registered');
                        $window.location.href = "/Home";
                        
                        ClearForm();
                    }
                    else {
                        alert(d);
                    }
                    $scope.submitText = "Save";
                });

                $scope.User.EmailID = email;
            }
            else {
                $scope.message = '';
            }
        }
    }

    //Clear Form 
    function ClearForm() {
        $scope.User = {};
        $scope.f1.$setPristine(); 
        $scope.submitted = false;
    }

})


.factory('RegistrationService', function ($http, $q) {

    var fac = {};

    fac.GetCompany = function () {
        return $http.get('/Data/GetCompany')
    }


    fac.GetEmployeeDetail = function () {
        return $http.get('/Data/GetEmployeeDetail')
    }


    fac.GetReportingBoss = function () {
        return $http.get('/Data/GetReportingBoss')
    }

    fac.GetDepartmentHead = function () {
        return $http.get('/Data/GetDepartmentHead')
    }


    fac.SaveFormData = function (data) {
        var defer = $q.defer();
        $http({
            url: '/User/Register_insert',
            method: 'POST',
            data: JSON.stringify(data),
            headers: { 'content-type': 'application/json' }
        }).success(function (d) {
            // Success callback
            defer.resolve(d);
        }).error(function (e) {
            //Failed Callback
            alert('Error SaveFormData!');
            defer.reject(e);
        });
        return defer.promise;
    }
    return fac;
});



