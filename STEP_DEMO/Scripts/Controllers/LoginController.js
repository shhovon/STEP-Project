angular.module('MyApp') 
.controller('LoginController', function ($scope, LoginService) {
       
    //alert(angular.toJson($scope.data));

    //Default Variable
    $scope.submitText = "Sign In";
    $scope.submitted = false;
    $scope.message = '';   
    $scope.isFormValid = false;

    $scope.CompID = '';

    
   
    $scope.User = {
        CompID: '',
        EmployeeCode: '',
        RegId: ''
    };

  

    // Populate Unit
    LoginService.GetCompany().then(function (d) {
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
    $scope.Login_check = function (data) {
       
        if ($scope.submitText == 'Sign In') {
           
            $scope.submitted = true;
           
            $scope.message = '';           
            if ($scope.isFormValid) {
                
                $scope.User = data;
               // $scope.User.EmployeeCode = 'MGT-' + $scope.User.EmployeeCode;

                //Registration Check
                LoginService.Login_check($scope.User).then(function (d) {

                    if (d.split(',')[0] == 'Success') {
                        LoginService.SaveLoginInfo($scope.User).then(function (d) {
                            //alert(d);
                        });


                        //window.location.pathname = 'Home/Dashboard';
                        // window.location.pathname = 'Home/UserProfile';

                        window.location.pathname = d.split(',')[1];

                        //alert('Success');
                        ClearForm();
                    }
                    else
                    {
                        alert(d);
                    }

                    $scope.submitText = "Sign In";
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




.factory('LoginService', function ($http, $q) { 


    var fac = {};

    fac.GetCompany = function () {
        return $http.get('/Data/GetCompany')
    }


    fac.GetEmployeeDetailByEmpCode = function (data) {
        return $http.get('/Data/GetEmployeeDetailByEmpCode', {
            params: { CompID: data.CompID, EmpCode: data.EmployeeCode }
        });
    }

    

   
    fac.Login_check = function (data) {


        var defer = $q.defer();

       

        var data_emp = fac.GetEmployeeDetailByEmpCode(data);

        data_emp.then(function (result) {
            if (result.data.length < 1) {
                defer.resolve('Login ID Not avaiable');
                return defer.promise;
            }
            else {
                data.RegId = result.data[0].RegId;

                $http({
                    url: '/User/Login_check',
                    method: 'POST',
                    data: JSON.stringify(data),
                    headers: { 'content-type': 'application/json' }
                }).success(function (d) {
                    // Success callback
                    defer.resolve(d);
                }).error(function (e) {
                    //Failed Callback
                    alert('Error Login_check!');
                    defer.reject(e);
                });
            }
        });





        fac.SaveLoginInfo = function (data) {
            //alert(data);
            var defer = $q.defer();
            $http({
                url: '/User/SaveLoginInfo',
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

      

       
        return defer.promise;
    }
    return fac;
});
