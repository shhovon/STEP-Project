angular.module('MyApp') 
.controller('RecoveryPasswordController', function ($scope, RecoveryPasswordService) {
       

    //alert(angular.toJson($scope.data));

    //Default Variable
    $scope.submitText = "Recover";
    $scope.submitted = false;
    $scope.message = '';
    $scope.CompID = '';
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
    RecoveryPasswordService.GetCompany().then(function (d) {
        $scope.CompanyList = d.data;
    }, function (error) {
        alert('Error!' + error);
    });


    $scope.GetCompID = function () {
        $scope.User.CompID = $scope.Company.ID;
        $scope.User.CompName = $scope.Company.Name;
    };



    //validates form on client side
    $scope.$watch('f1.$valid', function (newValue) {
        $scope.isFormValid = newValue;
    });
    //Save Data
    $scope.RecoveryPassword = function (data) {


        if ($scope.submitText == 'Recover') {
            $scope.submitted = true;
            $scope.message = '';           
            if ($scope.isFormValid) {
               
                $scope.User = data;

                //Registration Check
                RecoveryPasswordService.RecoveryPassword($scope.User).then(function (d) {
                   
                    if (d == 'Success') {
                        alert("Temporary Password sent to your email ID");
                        window.location.pathname = 'Home/Dashboard';
                        //alert('Success');
                        ClearForm();
                    }
                    else
                    {
                        alert(d);
                    }

                    $scope.submitText = "Recover";
                });
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




.factory('RecoveryPasswordService', function ($http, $q) { 


    var fac = {};

    fac.GetCompany = function () {
        return $http.get('/Data/GetCompany')
    }
    

    fac.GetEmployeeDetailByEmpCode = function (data) {
        return $http.get('/Data/GetEmployeeDetailByEmpCode', {
            params: { CompID: data.CompID, EmpCode: data.EmployeeCode }
        });
    }

    
   
    fac.RecoveryPassword = function (data) {
        
        var defer = $q.defer();

        var data_emp = fac.GetEmployeeDetailByEmpCode(data);

        data_emp.then(function (result) {
            //alert(angular.toJson(result));
            if (result.data.length < 1) {
                defer.resolve('Login ID Not avaiable');
                return defer.promise;
            }
            else {
                data.RegId = result.data[0].RegId;
                data.Updated_by = result.data[0].Name;
                
                //alert($scope.User.RegId)
                $http({
                    url: '/User/RecoveryPassword',
                    method: 'POST',
                    data: JSON.stringify(data),
                    headers: { 'content-type': 'application/json' }
                }).success(function (d) {
                    // Success callback
                    defer.resolve(d);
                }).error(function (e) {
                    //Failed Callback
                    alert('Error RecoveryPassword! -- '+e);
                    defer.reject(e);
                });
            }
        });




       
        return defer.promise;
    }
    return fac;
});
