
angular.module('MyApp')
.controller('UserProfileController', function ($scope, UserProfileService, $window, $filter, $http) {


    //alert(angular.toJson($scope.data));

    $scope.submitText = "Save";


    $scope.submitted = false;
    $scope.message = '';
    $scope.isFormValid = false;

    $scope.CompID = '';

    $scope.User = {
        RegId: '',
        Password: '',
        EmailID: '',
        MobileNoPerson: '',
        PhoneExt: '',
        Password: '',
        DeptHead: '',
        ReportSuper: '',
        DateofBirth: new Date(),
        BloodGroup: ''

    };



    $scope.EmployeeDetail = {
        RegId: '',
        EmployeeCode: '',
        Name: '',
        Department: '',
        Section: '',
        Designation: ''
    };


    $scope.BloodGroupList = $filter('BloodGroupList')();


    $scope.$on("fileProgress", function (e, progress) {
        $scope.progress = progress.loaded / progress.total;

    });

    //validates form on client side
    $scope.$watch('f1.$valid', function (newValue) {
        $scope.isFormValid = newValue;
    });




    //// Populate EmployeeDetail
    var DataEmployeeDetail = UserProfileService.GetEmployeeDetail().then(function (d) {
        $scope.EmployeeDetail = d.data;

        $scope.UserCode = '';
        $scope.ReportSuper = '';
        $scope.DeptHead = ''


    }, function (error) {
        alert('Error GetEmployeeDetail!');
    });





    // Populate GetUserDetailBySession

    UserProfileService.GetUserDetailBySession().then(function (d) {
        $scope.UserDetail = d.data[0];
        $scope.ProfilePic = '../images/users/' + d.data[0].RegId + '.png';


        $http.get($scope.ProfilePic).then(function () {
            $scope.ProfilePic = '../images/users/' + d.data[0].RegId + '.png';
        }, function (error) {
            //alert(error);
            $scope.ProfilePic = '../images/users/blank-profile-picture.png';
        }
                  );


    }, function (error) {
        alert('Error GetUserDetailBySession!');
    });


  
   

    DataEmployeeDetail.then(function (result) {
    Load_userInfo();
    });



    //Load_userInfo
    function Load_userInfo() {


        // Populate GetRegistrationDetail


        $scope.UserDetail_old = [];


            UserProfileService.GetRegistrationDetail().then(function (d) {
                $scope.User = d.data[0];

                //get report super
                $scope.UserDetail_old = angular.copy($scope.User);
                // alert(angular.toJson($scope.UserDetail_old));

                var EmployeeDetail = $filter('filter')($scope.EmployeeDetail, { RegId: $scope.User.ReportSuper }, true);
                $scope.ReportSuper = EmployeeDetail[0];


                EmployeeDetail = $filter('filter')($scope.EmployeeDetail, { RegId: $scope.User.DeptHead }, true);
                $scope.DeptHead = EmployeeDetail[0];


                var UserBloodGroup = $filter('filter')($scope.BloodGroupList, { id: $scope.User.BloodGroup.trim() }, true);

                $scope.BloodGroup = UserBloodGroup[0];

                $scope.User.DateofBirth = new Date(parseInt($scope.User.DateofBirth.substr(6)));

                $scope.uCPassword = $scope.User.Password;

                $scope.User.last_login_date = new Date(parseInt($scope.User.last_login_date.substr(6)));
                $scope.User.Created_date = new Date(parseInt($scope.User.Created_date.substr(6)));
                $scope.User.Updated_date = new Date(parseInt($scope.User.Updated_date.substr(6)));



            }, function (error) {
                alert('Error GetRegistrationDetail!');
            });


       

    }
    


    


    $scope.advstatus = false;


    $scope.uploadFile = function (event) {


        $scope.advstatus = true;
        $scope.$apply();


        var files = event.target.files;
        files2 = files;


        angular.forEach(files, function (flowFile, i) {

            var flowFile_size = flowFile.size / (1024 * 1024);

            if (flowFile_size > 2) {
                //alert('Please select File size < 2 MB');               
                //return;
            }

            var fileReader = new FileReader();
            var image = new Image();



            fileReader.onload = function (event) {

                var uri = event.target.result;
                image.src = uri;
                image.onload = function () {
                  

                    $scope.$apply();
                };


                fileReader.onloadend = function (e) {
                 

                    $scope.$apply();
                    //alert($scope.progress);
                }


                $scope.images[i].uri = uri;
            };
            fileReader.readAsDataURL(flowFile);
        });



        var Form_Data = new FormData();


        angular.forEach(files, function (file) {
            Form_Data.append('file', file);
        });


        UserProfileService.UpdateProfilePic(Form_Data).then(function (d) {
          
            $scope.advstatus = false;
            $scope.$apply();

            alert(d);

        });

    }

    



    $scope.UpdateProfile = function (data) {


        $scope.submitted = true;
        $scope.message = '';

        if ($scope.isFormValid) {


            $scope.User = data;

            var email = $scope.User.EmailID;
            $scope.User.EmailID = $scope.User.EmailID ;


            $scope.User.ReportSuper = $scope.ReportSuper.RegId;
            $scope.User.DeptHead = $scope.DeptHead.RegId;

            $scope.User.BloodGroup = $scope.BloodGroup.id;


            UserProfileService.UpdateProfile($scope.User).then(function (d) {

                if (d == 'Success') {
                    alert('You have successfully Updated');
                    $scope.User.EmailID = $scope.User.EmailID.toString().replace($scope.group_email, '');

                    //send email to report Super
                    if ($scope.UserDetail_old.ReportSuper != $scope.User.ReportSuper)
                    {                        
                      
                        $scope.UserDetail_old.Updated_by = angular.copy($scope.User.ReportSuper);

                        UserProfileService.ReportSupperChangeMail($scope.UserDetail_old).then(function (d) {                           
                            Load_userInfo();
                            // alert('Sent Email');
                        });


                    }

                }
                $scope.submitText = "Save";
            });
        }
    }




    // Populate Unit
    UserProfileService.GetCompany().then(function (d) {
        $scope.CompanyList = d.data;
    }, function (error) {
        alert('Error GetCompany!');
    });


    $scope.GetCompID = function () {
        $scope.User.COMID = $scope.Company.ID;       
    };




    //Get EmployeeName from Employee Code
    $scope.GetEmployeeID = function (ID) {
        UserProfileService.GetEmployeeName(ID).then(function (d) {           
            $scope.User.Employeename = d.data[0].Name;

        }, function (error) {
            alert('Error GetEmployeeID!');
        });

    }




    //Clear Form 
    function ClearForm() {
        $scope.User = {};
        $scope.f1.$setPristine(); 
        $scope.submitted = false;
    }

})


.factory('UserProfileService', function ($http, $q) {

    var fac = {};



    fac.GetCompany = function () {
        return $http.get('/Data/GetCompany')
    }


    fac.GetEmployeeDetail = function () {
        return $http.get('/Data/GetEmployeeDetail')
    }

    
    fac.GetRegistrationDetail = function () {
        return $http.get('/Data/GetRegistrationDetail')

    }

    fac.GetReportingBoss = function () {
        return $http.get('/Data/GetReportingBoss')
    }

    fac.GetDepartmentHead = function () {
        return $http.get('/Data/GetDepartmentHead')
    }


    fac.GetUserDetailBySession = function () {
        return $http.get('/Data/GetUserDetailBySession')
    }


    var getModelAsFormData = function (data) {
        var dataAsFormData = new FormData();
        angular.forEach(data, function (value, key) {
            dataAsFormData.append(key, value);
        });
        return dataAsFormData;
    };

    


    fac.UpdateProfilePic = function (Form_Data) {

        var defer = $q.defer();
        
        var url = '/User/FileUpload'; 

        $http.post(url, Form_Data, {
            transformRequest: angular.identity,
            headers: { 'Content-Type': undefined }
        })
        .success(function (d) {
            defer.resolve(d);
        })
        .error(function (e) {
            alert('Error!');
            defer.reject(e);
        });

        return defer.promise;
    }



    fac.UpdateProfile = function (data) {
        var defer = $q.defer();
        $http({
            url: '/User/Register_update',
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

    fac.UpdateProfile = function (data) {
        var defer = $q.defer();
        $http({
            url: '/User/Register_update',
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


    fac.ReportSupperChangeMail = function (data) {
        var defer = $q.defer();
        $http({
            url: '/User/ReportSupperChangeMail',
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



