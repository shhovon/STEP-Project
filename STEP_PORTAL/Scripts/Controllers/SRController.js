angular.module('MyApp')
    .controller('SRController', function ($scope, $location, $filter, $timeout, SRService) {


        //Default Variable
        $scope.submitted = false;
        $scope.message = '';
        $scope.CompID = '';
        $scope.isFormValid = false;

        $scope.MailShow = false;

        $scope.data = {
            ID: null,
            FromDate: new Date(),
            ToDate: new Date()
        };

        $scope.dataSearch = {
            ID: null,
            FromDate: new Date(),
            ToDate: new Date()
        };




        // Recive Parameter
        var popupParameter = $location.search();

        // alert(angular.toJson(popupParameter));


        $scope.data = popupParameter;

        // alert($scope.data.ID);

        if ($scope.data.ID != null) {

            Load_SRDetail();
        }
        else {

            Load_SR();
        }

        $scope.FromDate = new Date(new Date().setDate(new Date().getDate() - 30));
        $scope.ToDate = new Date(new Date().setDate(new Date().getDate() + 1));




        $scope.dataSearch.FromDate = new Date($scope.FromDate);
        $scope.dataSearch.ToDate = new Date($scope.ToDate);




        $scope.SRPriorityList = ['Low', 'Medium', 'High'];


        $scope.searchSRStatusList = ['Pending', 'Close', 'Cancel', 'In_Progress'];

        $scope.SRStatus = ['Close', 'Cancel'];






        var availableClasses = [
            "Pending",
            "Close",
            "Cancel",
            "In_Progress"        ];

        $scope.getClasses = function (get_value) {
            var classes = [];
            //alert(get_value);
            angular.forEach(availableClasses, function (value) {
                if (get_value.indexOf(value) != -1)
                    classes.push(value);
            });
            //alert(classes);
            //classes.push("BUDDHIST");
            return classes;
        };




        //$timeout(function () {
        //    //  alert(angular.toJson($scope.dataSRType[1]));
        //    //  $scope.SRType = $scope.dataSRType[1];
        //}, 5000);                           






        // Populate DeptSec AND  Company Filter
        SRService.GetEmpDeptSec().then(function (d) {
            $scope.DepartmentList = d.data;


        }, function (error) {
            alert('Error!' + error);
        });



        SRService.GetSRCategory($scope.data).then(function (d) {

            $scope.dataSRCategory = d.data;
           // alert(angular.toJson($scope.dataSRType));

            $scope.SRDetail.SRType = $scope.dataSRType[1];

            // alert(angular.toJson($scope.dataList));

        }, function (error) {
            alert('Error GetSRCategory!');
        });


        SRService.GetSRSubCategory($scope.data).then(function (d) {


            $scope.dataSRSubCategory = d.data;

            // alert(angular.toJson($scope.dataList));

        }, function (error) {
            alert('Error GetSRSubCategory!');
        });



        SRService.GetSRType($scope.data).then(function (d) {


            $scope.dataSRType = d.data;
            // $scope.SRDetail.SRType = $scope.dataSRType[1];

            // alert(angular.toJson($scope.dataSRType));

        }, function (error) {
            alert('Error GetSRType!');
        });


        SRService.GetEmployeeDetail($scope.data).then(function (d) {


            $scope.EmployeeDetail = d.data;

            //$scope.EmployeeDetail.push({
            //    EmployeeCode: '000001',
            //    Name: 'PWC',
            //    Designation: '',
            //    Department: ''
            //});


            //$scope.EmployeeDetail.push({
            //    EmployeeCode: '000002',
            //    Name: 'Oracle SR',
            //    Designation: '',
            //    Department: ''
            //});


            ////modified by sayem
            //$scope.EmployeeDetail_Filtered = $scope.EmployeeDetail.filter(function (item) { return item.Department == 'ICT' || item.Department == 'ICT (CORPORATE)' || item.Department == 'MANAGEMENT INFORMATION SYSTEM' });;

            $scope.EmployeeDetail_Filtered = $scope.EmployeeDetail.filter(function (item) {
                //return item.Designation != 'CHAIRMAN' || item.Designation != 'MANAGING DIRECTOR' || item.Designation != 'DIRECTOR' 

                // this code works for to filter out the data from the employees when adding for add person
                var designation = item.Designation.toUpperCase(); // Convert to uppercase for case-insensitive comparison
                return designation !== 'CHAIRMAN' && designation !== 'MANAGING DIRECTOR' && designation !== 'DIRECTOR';
            });



            SRService.GetUserDetailBySession().then(function (d) {

                $scope.UserDetail = d.data[0];


                if ($scope.UserDetail.Department == 'ICT (CORPORATE)' || $scope.UserDetail.Department == 'ICT') {

                    $scope.EmployeeDetail_Request = $scope.EmployeeDetail;

                    //$scope.SRAdd.RequestBy = $scope.UserDetail;

                }
                else {

                    $scope.EmployeeDetail_Request = $filter('filter')($scope.EmployeeDetail, { RegId: $scope.UserDetail.RegId }, true);

                }
            });







        }, function (error) {
            alert('Error GetEmployeeDetail!');
        });






        //// Populate EmployeeDetail
        SRService.GetEmployeeDetail().then(function (d) {


            $scope.EmployeeDetail = d.data;



        }, function (error) {
            alert('Error GetEmployeeDetail!');
        });





        $scope.dataList_all = null;



        function Load_SR() {


            /*debugger;*/
            SRService.GetSR($scope.dataSearch).then(function (d) {

                $scope.dataList_all = d.data;
                // alert(angular.toJson($scope.dataList_all));
                $scope.dataList = angular.copy($scope.dataList_all);

                //added test code 13/06/23

                //debugger;
                //var dataTable = $('#Report_table').DataTable();

                //$scope.$watch('searchKeyword', function (newVal) {
                //    dataTable.search(newVal).draw();
                //});




            }, function (error) {
                alert('Error GetSR!' + angular.toJson(error));
            });


        }

        $scope.search_Load_by = function () {



            $scope.dataSearch.FromDate = new Date($scope.FromDate);
            $scope.dataSearch.ToDate = new Date($scope.ToDate);


            Load_SR();

        }

        $scope.search_by = function () {


            //alert(angular.toJson($scope.searchAssignTo));


            $scope.dataList = angular.copy($scope.dataList_all);


            if ($scope.searchAssignTo != null) {

                $scope.dataList = $filter('filter')($scope.dataList, { AssignTo_Name: $scope.searchAssignTo.AssignTo_Name }, true);
            }


            if ($scope.searchSupportedBy != null) {

                $scope.dataList = $filter('filter')($scope.dataList, { SupportedBy_Name: $scope.searchSupportedBy.SupportedBy_Name }, true);
            }


            if ($scope.searchRequestBy != null) {

                $scope.dataList = $filter('filter')($scope.dataList, { RequestBy_Name: $scope.searchRequestBy.RequestBy_Name }, true);
            }




            if ($scope.searchSRCategory != null) {

                $scope.dataList = $filter('filter')($scope.dataList, { Category: $scope.searchSRCategory.Category }, true);
            }

            if ($scope.searchSRSubCategory != null) {

                $scope.dataList = $filter('filter')($scope.dataList, { SubCategory: $scope.searchSRSubCategory.SubCategory }, true);
            }

            if ($scope.searchSRPriority != null) {

                $scope.dataList = $filter('filter')($scope.dataList, { Priority: $scope.searchSRPriority }, true);
            }

            if ($scope.searchSRStatus != null) {

                $scope.dataList = $filter('filter')($scope.dataList, { Status: $scope.searchSRStatus }, true);
            }

            //alert($scope.fromDate);

            // $scope.dataList = $filter('dateRange')($scope.dataList, $scope.fromDate, $scope.toDate);




        };









        function Load_SRDetail() {
            /*debugger;*/
            $scope.data.FromDate = new Date();
            $scope.data.ToDate = new Date();
            //  alert(angular.toJson($scope.data));

            SRService.GetSR($scope.data).then(function (d) {

                $scope.dataList_all = d.data;

                $scope.dataList = angular.copy($scope.dataList_all);

                //$scope.dataList = $filter('dateRange')($scope.dataList, $scope.fromDate, $scope.toDate);



                // alert(angular.toJson($scope.dataList));

            }, function (error) {
                alert('Error GetSR!' + angular.toJson(error));
            });



            SRService.GetSRDetail($scope.data).then(function (d) {


                $scope.SRDetaildataList = d.data;


                //// Populate GetSRSupported_by
                SRService.GetSRSupported_by($scope.data).then(function (d) {

                   

                    $scope.EmployeeDetailSupportedBy = d.data;



                }, function (error) {
                    alert('Error GetSRSupported_by!');
                });



                //angular.forEach($scope.SRDetaildataList, function (value, key) {

                //   $scope.Emp = $filter('filter')($scope.EmployeeDetail, { RegId: parseInt(value.Created_By) }, true);
                //    // alert(angular.toJson($scope.Emp));

                //   var isIn = false;

                //   for (var i = 0; i < $scope.EmployeeDetailSupportedBy.length; i++) {
                //       if ($scope.Emp[0].RegId == value.Created_By) {
                //           isIn = true;
                //           break;
                //       }
                //   }
                //   if (!isIn) {
                //       $scope.EmployeeDetailSupportedBy.push($scope.Emp[0]);
                //   }

                //});

            }, function (error) {
                alert('Error GetSRDetail!');
            });



        }


        $scope.GetSRDetailBot = function () { 
            //debugger;

            //console.log($scope.searchTextSupportBot);

            SRService.prc_GetSR_DetailBot_By_Search_Text($scope.searchTextSupportBot).then(function (d) {
                $scope.SRDetailBotResults = d.data;
                /*console.log($scope.SRDetailBotResults);*/



            }, function (error) {
                alert('Error Get Detail Bot Result!');
            });
        }





        //Create SR
        $scope.CreateSR = function (data) {



            $scope.SR = {
                Title: '',
                Priority: '',
                SubCategory: '',
                RequestBy: '',
                Created_By: ''
            };

            $scope.SR.Title = data.Title;
            $scope.SR.Priority = data.SRPriority;
            $scope.SR.SubCategory = data.SRSubCategory.ID;
            $scope.SR.RequestBy = data.RequestBy.RegId;



            if ($scope.isFormValid) {

                SRService.CreateSR($scope.SR).then(function (d) {

                    angular.element('#ModalSR').modal('hide');

                    Load_SR();

                    alert(d.split(',')[1]);


                    // Insert data in detail

                    $scope.SRDetail = {
                        SRID: '',
                        SRType: { ID: 2 },
                        Description: ''
                    };



                    $scope.SRDetail.SRID = d.split(',')[0];
                    $scope.SRDetail.Description = data.Description;


                    $scope.data.ID = null;



                    $scope.UpdateSRDetail($scope.SRDetail);



                });



                Load_SR();

            }

        }





        //Update SR 
        $scope.UpdateSR = function (data) {

            //debugger;

            $scope.SR = {
                ID: '',
                SupportedBy: '',
                Status: '',
                Remarks: ''
            };

            $scope.SR.ID = $scope.data.ID;

            if (data.Status != 'Cancel') {

                $scope.SR.SupportedBy = data.SupportedBy.RegId;
            }

            $scope.SR.Status = data.Status;
            $scope.SR.Remarks = data.Remarks;

            // update SR detail line

            // alert(angular.toJson($scope.SR.ID));

            var Form_Data = new FormData();

            Form_Data.append("SRID", $scope.SR.ID);
            Form_Data.append("Type", 2);
            Form_Data.append("Description", data.Description);


            if (data.Description != '') {

                SRService.UpdateSRDetail(Form_Data).then(function (d) {


                });

            }

            // alert(angular.toJson(Form_Data));

            //return;


            // Update SR header
            SRService.UpdateSR($scope.SR).then(function (d) {


                angular.element('#ModalSRClose').modal('hide');

                Load_SRDetail();

                alert(d);



            });


        }

        //Update SR Detail
        $scope.UpdateSRDetail = function (data) {

            var SRID = null;

            if ($scope.data.ID != null) {

                SRID = $scope.data.ID;
            }
            else {
                SRID = data.SRID;
            }


            var Form_Data = new FormData();

            Form_Data.append("SRID", SRID);
            Form_Data.append("Type", data.SRType.ID);

            if (data.SRType.ID == 1) {
                Form_Data.append("Description", data.AddEmp.RegId);
            }
            else {
                Form_Data.append("Description", data.Description);
            }


            angular.forEach(files, function (file, i) {

                Form_Data.append('files', file);

            });







            if ($scope.isFormValid) {

                SRService.UpdateSRDetail(Form_Data).then(function (d) {

                    //

                    files = null;
                    $scope.imagesList = [];

                    angular.element('#ModalSRDetail').modal('hide');



                    if ($scope.data.ID != null) {


                        alert(d);

                        Load_SRDetail();
                    }


                    $scope.data.ID = SRID;




                    if (d == 'SR Detail Updated') {

                        $scope.MailShow = true;

                        SRService.SRMail($scope.data).then(function (d) {

                            //  alert(d);

                            $scope.MailShow = false
                        });
                    }


                });



            }

        }



        $scope.exportData = function () {

            //debugger;
            var uri = 'data:application/vnd.ms-excel;base64,'
                , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{CouponDetails}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>'
                , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
                , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

            var table = document.getElementById("Report_table");
            var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML };
            var url = uri + base64(format(template, ctx));
            var a = document.createElement('a');
            a.href = url;
            a.download = 'SR_data.xls';
            a.click();
        };


        angular.element('#ModalShowImageSlide').on('shown.bs.modal', function () {

            $scope.advstatus = true;

            preLoad().then(function () {
                $scope.nextLoad = "Starting the next load activity..."
            });

        });








        var files = null;


        $scope.imagesList = [];


        $scope.uploadFile = function (event) {


            files = event.target.files;

            angular.forEach(files, function (flowFile, i) {


                $scope.imagesList[i] = {};


                $scope.imagesList[i].Name = flowFile.name;
                $scope.imagesList[i].size = flowFile.size;
                $scope.imagesList[i].Status = 'Uploading';

            });




        }


        $scope.uploadFile2 = function (event) {


            $scope.advstatus = true;
            $scope.$apply();


            var files = event.target.files;



            files2 = files;

            angular.forEach(files, function (flowFile, i) {


                $scope.imagesList[i] = {};


                $scope.imagesList[i].Name = flowFile.name;
                $scope.imagesList[i].size = flowFile.size;
                $scope.imagesList[i].Status = 'Uploading';

                //alert(flowFile.size/(1024*1024));

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

                        //alert(this.width + " " + this.height);    


                        //filesLoaded++;
                        //if (filesLoaded === files.length) {
                        //    console.log('Files already loaded');
                        //    $scope.progress = 100;
                        //    // Do some stuff when all files are loaded...
                        //}
                        //else {
                        //    console.log((100 * filesLoaded / files.length) + '% loaded');
                        //    $scope.progress = 100 * (filesLoaded / files.length);
                        //    $scope.$apply(); // Is it mandatory?
                        //}


                        $scope.$apply();
                    };


                    fileReader.onloadend = function (e) {


                        $scope.$apply();
                        //alert($scope.progress);
                    }


                    $scope.imagesList[i].uri = uri;



                };
                fileReader.readAsDataURL(flowFile);
            });






            angular.forEach(files, function (file, i) {

                var Form_Data = new FormData();


                // alert($scope.SRDetaildataList[0].SRID);

                Form_Data.append("ID", $scope.SRDetaildataList[0].ID);
                Form_Data.append("SRID", $scope.SRDetaildataList[0].SRID);

                Form_Data.append('file', file);


                SRService.UpdateAlbumPic(Form_Data).then(function (d) {

                    $scope.advstatus = false;
                    $scope.$apply();
                    $scope.imagesList[i].Status = d;
                    //alert(d);

                }, function (error) {
                    alert('Error ' + error);
                });

            });




        }


        //ZoomImage
        $scope.ZoomImage = function (data) {

            // alert(angular.toJson(data));     
            var ImageURL = "http://localhost:36224/Home/GetImg?imageURL=" + data;
            $window.open(ImageURL);
        }



        //Update Albun
        $scope.UpdateAlbum = function () {

            SRService.Album_update($scope.Album).then(function (d) {
                // alert(d);
            });

        }



        //TemplateImage
        $scope.TemplateImage = function (data) {

            $scope.Album.TemplateImage = data;

            if (!confirm('Are you sure you want to save this as Template Image?')) {
                return;
            }

            //  alert($scope.Album.TemplateImage);

            // Save it!
            SRService.Album_update($scope.Album).then(function (d) {
                alert(d);
            });
        }


        //Save Data
        $scope.DeleteImage = function (data) {


            $scope.Album.TemplateImage = data;

            if (confirm('Are you sure you want to Delete this Image ? -- ' + data)) {
                // DeleteImage it!

                SRService.DeleteImage($scope.Album).then(function (d) {
                    alert(d);
                });

            } else {
                // Do nothing!
            }
        }

        //validates form on client side
        $scope.$watch('f1.$valid', function (newValue) {
            $scope.isFormValid = newValue;
        });

        //Save Data
        $scope.CreateNewAlbum = function (data) {

            //alert(angular.toJson($scope.isFormValid));


            $scope.submitted = true;
            $scope.message = '';


            if ($scope.isFormValid) {

                SRService.CreateNewAlbum(data).then(function (d) {

                    alert(d);

                    var ms = new Date().getTime() + 86400000;
                    $scope.LastDate = new Date(ms);
                    GetAlbumDetail();

                    angular.element('#ModalAlbumNew').modal('hide');



                });
            }


        }





        angular.element('#ModalShowImageSlide').on('shown.bs.modal', function () {

            $scope.advstatus = true;

            preLoad().then(function () {
                $scope.nextLoad = "Starting the next load activity..."
            });

        });





        function preLoad() {

            $scope.log = [];
            $scope.nextLoad = "Waiting for preLoad...";
            var promises = [];

            function loadImage(src) {
                return $q(function (resolve, reject) {
                    var image = new Image();

                    //  alert(src);
                    // var  src = 'https://upload.wikimedia.org/wikipedia/commons/thumb/3/3d/LARGE_elevation.jpg/800px-LARGE_elevation.jpg';

                    image.src = src

                    image.onload = function () {
                        // $scope.advstatus = true;
                    };

                    image.onloadend = function () {
                        $scope.log.push("loaded image: " + src);
                        // $scope.advstatus = false;
                        angular.element('#carousel-custom').carousel();
                        resolve(image);
                    };

                    image.onerror = function (e) {
                        reject(e);
                    };
                })
            }

            //$scope.ImagesList = angular.element('#carousel-custom').find('img').map(function () { return this.src; }).get();
            //alert($scope.ImagesList.length);
            //$scope.ImagesList.forEach(function (src) {
            //   // alert(src);
            //    promises.push(loadImage(src));
            //})



            $scope.AlbumImagesList.forEach(function (image_urls) {


                var host = $location.host();
                // alert(host);

                // var src = "http://localhost:36221/Home/GetImg?imageURL=" + image_urls;

                var src = 'http://' + host + "/Home/GetImg?imageURL=" + image_urls;
                promises.push(loadImage(src));
            })

            return $q.all(promises).then(function (results) {
                console.log('promises array all resolved');
                $scope.results = results;
            });
        }



        //Clear Form 
        function ClearForm() {
            $scope.User = {};
            $scope.f1.$setPristine(); //here f1 is form name
            $scope.submitted = false;
        }

        //$timeout(function () {
        //    $('#Report_table').DataTable();
        //}, 0);


    })

    .factory('SRService', function ($http, $q) {


        var fac = {};


        fac.GetCompany = function () {
            return $http.get('/Data/GetCompany')
        }


        fac.GetEmpDeptSec = function () {
            return $http.get('/Data/GetEmpDeptSec', {
                params: { Type: 'ALL' }
            });
        }



        fac.GetUserDetailBySession = function () {
            return $http.get('/Data/GetUserDetailBySession')
        }


        fac.GetSR = function (data) {

            return $http.get('/SR/GetSR', {
                params: { ID: data.ID, FromDate: data.FromDate, ToDate: data.ToDate }
            });
        }

        fac.GetSRDetail = function (data) {

            return $http.get('/SR/GetSRDetail', {
                params: { ID: data.ID }
            });
        }


        fac.GetSRSupported_by = function (data) {

            return $http.get('/SR/GetSRSupported_by', {
                params: { ID: data.ID }
            });
        }

        fac.prc_GetSR_EmpWise = function (data) {
            return $http.get('/SR/prc_GetSR_EmpWise', {
                params: { Date_from: data.Date_from, ID: Date_to.Date_to }
            });
        }

        fac.prc_GetSR_DetailBot_By_Search_Text = function (data) {
            return $http.get('/SR/prc_GetSR_DetailBot_By_Search_Text', {
                params: { SearchText: data }
            });
        }


        fac.GetSRCategory = function (data) {

            return $http.get('/SR/GetSRCategory');
        }

        fac.GetSRSubCategory = function (data) {

            return $http.get('/SR/GetSRSubCategory');
        }


        fac.GetSRType = function () {
            return $http.get('/SR/GetSRType');
        }


        fac.GetEmployeeDetail = function () {
            return $http.get('/Data/GetEmployeeDetail');
        }



        fac.UpdateSR = function (data) {
            //alert(data);
            var defer = $q.defer();
            $http({
                url: '/SR/UpdateSR',
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

        fac.UpdateSRDetail = function (Form_Data) {
            //alert(Form_Data);

            var defer = $q.defer();

            var url = '/SR/UpdateSRDetail';

            $http.post(url, Form_Data, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
                .success(function (d) {
                    //success
                    //alert(d);
                    defer.resolve(d);
                })
                .error(function (e) {
                    //failed
                    //alert('FileUpload');
                    // alert('Error!');
                    defer.reject(e);
                });

            return defer.promise;
        }

        fac.CreateSR = function (data) {
            //alert(data);
            var defer = $q.defer();
            $http({
                url: '/SR/CreateSR',
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


        fac.UpdateAlbumPic = function (Form_Data) {
            //alert(Form_Data);

            var defer = $q.defer();

            var url = '/SR/FileUpload';

            $http.post(url, Form_Data, {
                transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            })
                .success(function (d) {
                    //success
                    //alert(d);
                    defer.resolve(d);
                })
                .error(function (e) {
                    //failed
                    //alert('FileUpload');
                    // alert('Error!');
                    defer.reject(e);
                });

            return defer.promise;
        }


        fac.SRMail = function (data) {
            //alert(data);
            var defer = $q.defer();
            $http({
                url: '/SR/SRMail',
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


