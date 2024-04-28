angular.module('MyApp')


.controller('DashboardController', function ($scope, $location, $filter, $timeout, DashboardService) {


    $scope.fromDate = new Date(new Date().setDate(new Date().getDate() - 365));
    $scope.toDate = new Date();

    $scope.data_EmpWise = {
        Date_from: null,
        Date_to: null
    };



    Load_SR();


    $scope.search_by = function () {

        Load_SR();


    }



    function Load_SR() {
      

        //alert($scope.dataList_SubCategoryWise);


        $scope.data_EmpWise.Date_from = new Date($scope.fromDate);
        $scope.data_EmpWise.Date_to = new Date($scope.toDate);

       // alert($scope.data_EmpWise.Date_from);



      
        DashboardService.prc_GetSR_SubCategoryWise($scope.data_EmpWise).then(function (d) {

            $scope.dataList_SubCategoryWise = d.data;

            // alert($scope.dataList_SubCategoryWise);



            $scope.colors_SubCategoryWise = [
          { backgroundColor: '#444593', pointBackgroundColor: '#444593', pointHoverBackgroundColor: '#444593', borderColor: '#444593', pointBorderColor: '#444593', pointHoverBorderColor: '#444593' },
          { backgroundColor: '#1AA920', pointBackgroundColor: '#1AA920', pointHoverBackgroundColor: '#1AA920', borderColor: '#1AA920', pointBorderColor: '#1AA920', pointHoverBorderColor: '#1AA920' },
          { backgroundColor: '#CC9C08', pointBackgroundColor: '#CC9C08', pointHoverBackgroundColor: '#CC9C08', borderColor: '#CC9C08', pointBorderColor: '#CC9C08', pointHoverBorderColor: '#CC9C08' },
          { backgroundColor: '#DAF7A6', pointBackgroundColor: '#DAF7A6', pointHoverBackgroundColor: '#DAF7A6', borderColor: '#DAF7A6', pointBorderColor: '#DAF7A6', pointHoverBorderColor: '#DAF7A6' }

            ];


            $scope.options_SubCategoryWise = { legend: { display: true, position: 'top' } };


            $scope.series_SubCategoryWise = ['Total_SR', 'Total_Close', 'Total_Pending'];



            $scope.labels_SubCategoryWise = [];


            angular.forEach($scope.dataList_SubCategoryWise, function (value, key) {
                $scope.labels_SubCategoryWise.push(value.SubCategory);
            })





            ///////////////////////////////////

            $scope.data_SubCategoryWise = [];


            $scope.data_Total_SR = [];
            angular.forEach($scope.dataList_SubCategoryWise, function (value, key) {
                $scope.data_Total_SR.push(value.Total_SR);
            })



            $scope.data_Total_Close = [];
            angular.forEach($scope.dataList_SubCategoryWise, function (value, key) {
                $scope.data_Total_Close.push(value.Total_Close);
            })



            $scope.data_Total_Pending = [];
            angular.forEach($scope.dataList_SubCategoryWise, function (value, key) {
                $scope.data_Total_Pending.push(value.Total_Pending);
            })



            $scope.data_SubCategoryWise = [$scope.data_Total_SR, $scope.data_Total_Close, $scope.data_Total_Pending];




        //DashboardService.prc_GetSR_EmpWise($scope.data_EmpWise).then(function (d) {

        //    $scope.SRDetaildataList = d.data;


        //}, function (error) {


        //    alert('Error prc_GetSR_EmpWise!' + angular.toJson(error));
        //});






        }, function (error) {


            alert('Error prc_GetSR_EmpWise!' + angular.toJson(error));
        });





    }

  





    



    $scope.UserDetail = {
        RegId: '',
        Dept: '',
        Month: '',
        Year: ''
    };



    var curr_date = new Date();




    $scope.colors_EmpJobCard = [
  { backgroundColor: '#1AA920', pointBackgroundColor: '#1AA920', pointHoverBackgroundColor: '#1AA920', borderColor: '#1AA920', pointBorderColor: '#1AA920', pointHoverBorderColor: '#1AA920' },
  { backgroundColor: '#444593', pointBackgroundColor: '#444593', pointHoverBackgroundColor: '#444593', borderColor: '#444593', pointBorderColor: '#444593', pointHoverBorderColor: '#444593' },
  { backgroundColor: '#CC9C08', pointBackgroundColor: '#CC9C08', pointHoverBackgroundColor: '#CC9C08', borderColor: '#CC9C08', pointBorderColor: '#CC9C08', pointHoverBorderColor: '#CC9C08' },
  { backgroundColor: '#DAF7A6', pointBackgroundColor: '#DAF7A6', pointHoverBackgroundColor: '#DAF7A6', borderColor: '#DAF7A6', pointBorderColor: '#DAF7A6', pointHoverBorderColor: '#DAF7A6' }

    ];





    $scope.options_EmpAtten = {
        events: false,
        tooltips: {
            enabled: false
        },
        labels: ['Red', 'Blue', 'Purple', 'Yellow'],
        hover: {
            animationDuration: 0
        },
        scales: {
            xAxes: [{
                gridLines: {
                    display: false
                }
            }],
            yAxes: [{
                gridLines: {
                    display: false
                }
            }]
        },
        animation: {
            duration: 1000,
            onComplete: function () {
                var chartInstance = this.chart,
                    ctx = chartInstance.ctx;
                ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                ctx.textAlign = 'center';
                ctx.textBaseline = 'bottom';

                this.data.datasets.forEach(function (dataset, i) {
                    var meta = chartInstance.controller.getDatasetMeta(i);
                    meta.data.forEach(function (bar, index) {
                        var data = dataset.data[index];
                        ctx.fillText(data, bar._model.x, bar._model.y - 5);
                    });
                });
            }
        }
    };


    $scope.colors_EmpAtten = [{ backgroundColor: '#444593', pointBackgroundColor: '#444593', pointHoverBackgroundColor: '#444593', borderColor: '#444593', pointBorderColor: '#444593', pointHoverBorderColor: '#444593' },
  { backgroundColor: '#3F8AC5', pointBackgroundColor: '#3F8AC5', pointHoverBackgroundColor: '#3F8AC5', borderColor: '#3F8AC5', pointBorderColor: '#3F8AC5', pointHoverBorderColor: '#3F8AC5' },
  { backgroundColor: '#4DD146', pointBackgroundColor: '#4DD146', pointHoverBackgroundColor: '#4DD146', borderColor: '#4DD146', pointBorderColor: '#4DD146', pointHoverBorderColor: '#4DD146' }];



    //$scope.options_EmpAtten = { legend: { display: true, position: 'top' } };

    $scope.options_EmpLeave = {
        events: false,
        tooltips: {
            enabled: false
        },
        legend: {
            display: true
        },
        labels: ['Red', 'Blue', 'Purple', 'Yellow'],
        hover: {
            animationDuration: 0
        },
        scales: {
            xAxes: [{
                gridLines: {
                    display: false
                }
            }],
            yAxes: [{
                gridLines: {
                    display: false
                }
            }]
        },
        animation: {
            duration: 1000,
            onComplete: function () {
                var chartInstance = this.chart,
                    ctx = chartInstance.ctx;
                ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                ctx.textAlign = 'center';
                ctx.textBaseline = 'bottom';

                this.data.datasets.forEach(function (dataset, i) {
                    var meta = chartInstance.controller.getDatasetMeta(i);
                    meta.data.forEach(function (bar, index) {
                        var data = dataset.data[index];
                        ctx.fillText(data, bar._model.x, bar._model.y - 5);
                    });
                });
            }
        }
    };



    $scope.colors_EmpLeave = [{ backgroundColor: '#57B0F6', pointBackgroundColor: '#57B0F6', pointHoverBackgroundColor: '#57B0F6', borderColor: '#57B0F6', pointBorderColor: '#57B0F6', pointHoverBorderColor: '#57B0F6' },
    { backgroundColor: '#127DD1', pointBackgroundColor: '#127DD1', pointHoverBackgroundColor: '#127DD1', borderColor: '#127DD1', pointBorderColor: '#127DD1', pointHoverBorderColor: '#127DD1' },
    { backgroundColor: '#DAF7A6 ', pointBackgroundColor: '#DAF7A6 ', pointHoverBackgroundColor: '#DAF7A6 ', borderColor: '#DAF7A6 ', pointBorderColor: '#DAF7A6 ', pointHoverBorderColor: '#DAF7A6 ' }];


    $scope.options_ManPower = {
        events: false,
        tooltips: {
            enabled: false
        },
        legend: {
            display: true
        },
        labels: ['Red', 'Blue', 'Purple', 'Yellow'],
        hover: {
            animationDuration: 0
        },
        scales: {
            xAxes: [{
                gridLines: {
                    display: false
                }
            }],
            yAxes: [{
                gridLines: {
                    display: false
                }
            }]
        },
        zoom: {
            enabled: true,
            mode: 'x',
            sensitivity: 0.25,
        },
        animation: {
            duration: 1000,
            onComplete: function () {
                var chartInstance = this.chart,
                    ctx = chartInstance.ctx;
                ctx.font = Chart.helpers.fontString(Chart.defaults.global.defaultFontSize, Chart.defaults.global.defaultFontStyle, Chart.defaults.global.defaultFontFamily);
                ctx.textAlign = 'center';
                ctx.textBaseline = 'bottom';

                this.data.datasets.forEach(function (dataset, i) {
                    var meta = chartInstance.controller.getDatasetMeta(i);
                    meta.data.forEach(function (bar, index) {
                        var data = dataset.data[index];
                        ctx.fillText(data, bar._model.x, bar._model.y - 5);
                    });
                });
            }
        }
    };







    DashboardService.GetEmpLastAtten().then(function (d) {

        $scope.dataList_EmpLastAtten = d.data;

    }, function (error) {
        alert('Error GetEmpLastAtten!');
    });

    DashboardService.GetUserDetailBySession().then(function (d) {


        $scope.UserDetail.RegId = d.data[0].RegId;
        $scope.UserDetail.Year = curr_date.getFullYear();


        DashboardService.GetEmpAttenInfo($scope.UserDetail).then(function (d) {

            $scope.dataList_Atten = d.data;




            ///////////// Current Month's Statistics 

            //MonthName


            $scope.options_EmpJobCard = { legend: { display: true, position: 'top' } };

            var curr_month = curr_date.toLocaleString('default', { month: 'long' }).toUpperCase();
            // var curr_month = 'DECEMBER';
            $scope.dataList_curr_month = $filter('filter')($scope.dataList_Atten, { MonthName: curr_month }, true);



            $scope.labels_EmpJobCard = ['Pres.', 'Late', 'Leave', 'Absent'];



            $scope.data_EmpJobCard = [];



            angular.forEach($scope.dataList_curr_month, function (value, key) {
                $scope.data_EmpJobCard.push(value.Present_Days - value.Late_Days);
            })


            angular.forEach($scope.dataList_curr_month, function (value, key) {
                $scope.data_EmpJobCard.push(value.Late_Days);
            })


            angular.forEach($scope.dataList_curr_month, function (value, key) {
                $scope.data_EmpJobCard.push(value.Leave_Days);
            })


            angular.forEach($scope.dataList_curr_month, function (value, key) {
                $scope.data_EmpJobCard.push(value.Office_Day - value.Present_Days - value.Leave_Days);
            })







            /////////////  Late Detail 

            // $scope.series_EmpAtten = ['Office_Day', 'Present_Days', 'Late_Days', 'Leave_Days'];

            $scope.series_EmpAtten = ['Late_Days'];

            $scope.labels_EmpAtten = [];


            angular.forEach($scope.dataList_Atten, function (value, key) {
                $scope.labels_EmpAtten.push(value.MonthName.substring(0, 3) + '-' + value.YearName.toString().substring(2, 4));
            })


            $scope.data_EmpAtten = [];

            //$scope.data_EmpAtten_Office_Day = [];

            //angular.forEach($scope.dataList_Atten, function (value, key) {
            //    $scope.data_EmpAtten_Office_Day.push(value.Office_Day);
            //})

            //$scope.data_EmpAtten_Present_Days = [];

            //angular.forEach($scope.dataList_Atten, function (value, key) {
            //    $scope.data_EmpAtten_Present_Days.push(value.Present_Days);
            //})


            //$scope.data_EmpAtten_Leave_Days = [];

            //angular.forEach($scope.dataList_Atten, function (value, key) {
            //    $scope.data_EmpAtten_Leave_Days.push(value.Leave_Days);
            //})


            $scope.data_EmpAtten_Late_Days = [];

            angular.forEach($scope.dataList_Atten, function (value, key) {
                $scope.data_EmpAtten_Late_Days.push(value.Late_Days);
            })


            // $scope.data_EmpAtten = [$scope.data_EmpAtten_Office_Day, $scope.data_EmpAtten_Present_Days, $scope.data_EmpAtten_Late_Days, $scope.data_EmpAtten_Leave_Days];

            $scope.data_EmpAtten = [$scope.data_EmpAtten_Late_Days];

            //alert(angular.toJson($scope.labels_EmpAtten));

        }, function (error) {
            alert('Error GetEmpAttenInfo!');
        });



        /////////////  Leave Summary

        DashboardService.GetLeaveSummary($scope.UserDetail).then(function (d) {

            $scope.dataList_LeaveSummary = d.data;

            // alert(angular.toJson($scope.dataList_LeaveSummary));




            //$scope.series_EmpLeave = ['Total', 'Enjoy', 'Balance'];

            $scope.series_EmpLeave = ['Total', 'Balance'];

            $scope.labels_EmpLeave = [];


            angular.forEach($scope.dataList_LeaveSummary, function (value, key) {
                $scope.labels_EmpLeave.push(value.LeaveType);
            })




            $scope.data_EmpLeave = [];



            $scope.data_EmpLeave_Total = [];

            angular.forEach($scope.dataList_LeaveSummary, function (value, key) {
                $scope.data_EmpLeave_Total.push(value.Total);
            })



            $scope.data_EmpLeave_Enjoy = [];

            angular.forEach($scope.dataList_LeaveSummary, function (value, key) {
                $scope.data_EmpLeave_Enjoy.push(value.Enjoy);
            })


            $scope.data_EmpLeave_Balance = [];

            angular.forEach($scope.dataList_LeaveSummary, function (value, key) {
                $scope.data_EmpLeave_Balance.push(value.Balance);
            })


            // $scope.data_EmpLeave = [$scope.data_EmpLeave_Total, $scope.data_EmpLeave_Enjoy, $scope.data_EmpLeave_Balance];
            $scope.data_EmpLeave = [$scope.data_EmpLeave_Total, $scope.data_EmpLeave_Balance];

            // alert(angular.toJson($scope.data_EmpLeave));


        }, function (error) {
            alert('Error GetLeaveSummary!');
        });





        $timeout(function () {
            $scope.displayErrorMsg = false;
        }, 2000);



        ///////////////////////////////

        // Populate Unit
        DashboardService.GetCompany().then(function (d) {


            $scope.CompanyList = d.data;




            $scope.ParaManPower = {
                RegId: 1,
                Date: ''
            };

            $scope.ParaManPower.Date = curr_date;

            //alert(angular.toJson($scope.ParaManPower));



            DashboardService.Get_ManPower($scope.ParaManPower).then(function (d) {

                $scope.dataList_ManPowerOrg = d.data;





                $scope.DepartmentList = $scope.dataList_ManPowerOrg;
                $scope.SectionList = $scope.dataList_ManPowerOrg;

                //$scope.ComID = extractColumn($scope.DepartmentList, 'ComID');



                //alert(angular.toJson($scope.dataList_ManPowerer));

                Get_ManPower();


            }, function (error) {
                alert('Error Get_ManPower!');
            });

            DashboardService.GetEmployeeDetailByRegId($scope.UserDetail.RegId).then(function (d) {


                $scope.EmployeeDetail = d.data;



                //$scope.Company_user = $filter('filter')($scope.CompanyList, { ID: $scope.EmployeeDetail[0].ComID }, true);
                //$scope.Company = $scope.Company_user[0];


                //$scope.Department_user = $filter('filter')($scope.DepartmentList, { DeptName: $scope.EmployeeDetail[0].Department }, true);
                //$scope.Department = $scope.Department_user[0];


                //$scope.Section_user = $filter('filter')($scope.Department_user, { SubDeptName: $scope.EmployeeDetail[0].Section }, true);
                //$scope.Section = $scope.Section_user[0];



            }, function (error) {
                alert('Error GetEmployeeDetail!' + error);
            });






        }, function (error) {
            alert('Error GetCompany!' + error);
        });




    }, function (error) {
        alert('Error !' + error);
    });







    $scope.Get_ManPower = function () {


        Get_ManPower();


    };




    function Get_ManPower() {


        //$scope.dataList_ManPowerer




        $scope.dataList_ManPower = angular.copy($scope.dataList_ManPowerOrg);


        // alert(angular.toJson($scope.dataList_ManPower));
        // alert(angular.toJson($scope.Company));
        // alert(angular.toJson($scope.Department));
        //alert(angular.toJson($scope.Section));

        if ($scope.Company != undefined) {

            $scope.dataList_ManPower = $filter('filter')($scope.dataList_ManPower, { ComID: $scope.Company.ID }, true);
        }

        if ($scope.Department != undefined) {

            $scope.dataList_ManPower = $filter('filter')($scope.dataList_ManPower, { Department: $scope.Department.Department }, true);
        }

        if ($scope.Section != undefined) {

            $scope.dataList_ManPower = $filter('filter')($scope.dataList_ManPower, { Section: $scope.Section.Section }, true);
        }







        $scope.options_ManPower = { legend: { display: true, position: 'top' } };


        $scope.series_ManPower = ['Total_Emp', 'Total_Present', 'Total_Absent'];



        $scope.labels_ManPower = [];


        angular.forEach($scope.dataList_ManPower, function (value, key) {
            $scope.labels_ManPower.push(value.Section);
        })



        ///////////////////////////////////

        $scope.data_ManPower = [];


        $scope.data_Total_Emp = [];
        angular.forEach($scope.dataList_ManPower, function (value, key) {
            $scope.data_Total_Emp.push(value.Total_Emp);
        })


        $scope.data_Total_Present = [];
        angular.forEach($scope.dataList_ManPower, function (value, key) {
            $scope.data_Total_Present.push(value.Total_Present);
        })

        $scope.data_Total_Absent = [];
        angular.forEach($scope.dataList_ManPower, function (value, key) {
            $scope.data_Total_Absent.push(value.Total_Absent);
        })

        $scope.data_ManPower = [$scope.data_Total_Emp, $scope.data_Total_Present, $scope.data_Total_Absent];


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




})
.factory('DashboardService', function ($http, $q) {

    var fac = {};



    fac.prc_GetSR_EmpWise = function (data) {
       
        return $http.get('/SR/prc_GetSR_EmpWise', {
            params: { Date_from: data.Date_from, Date_to: data.Date_to }
        });
    }

    fac.prc_GetSR_SubCategoryWise = function (data) {

        return $http.get('/SR/prc_GetSR_SubCategoryWise', {
            params: { Date_from: data.Date_from, Date_to: data.Date_to }
        });
    }

    


    fac.GetUserDetailBySession = function () {
        return $http.get('/Data/GetUserDetailBySession')
    }


    fac.GetEmployeeDetail = function () {
        return $http.get('/Data/GetEmployeeDetail');
    }

    fac.GetEmployeeDetailByRegId = function (RegId) {
        return $http.get('/Data/GetEmployeeDetailByRegId', {
            params: { RegId: RegId }
        });
    }



    fac.GetEmpLastAtten = function () {
        return $http.get('/Data/GetEmpLastAtten')
    }



    fac.GetEmpAttenInfo = function (data) {
        return $http.get('/Data/GetEmpAttenInfo', {
            params: { RegId: data.RegId }
        });
    }

    fac.GetLeaveSummary = function (data) {
        return $http.get('/Data/GetLeaveSummary', {
            params: { RegId: data.RegId, Year: data.Year }
        });
    }



    fac.GetEmpDeptSec = function () {
        return $http.get('/Data/GetEmpDeptSec', {
            params: { Type: 'OTReq' }
        });
    }


    fac.GetCompany = function () {
        return $http.get('/Data/GetCompany')
    }



    fac.Get_ManPower = function (data) {
        return $http.get('/Data/Prc_Get_ManPower', {
            params: { RegId: data.RegId, Date: data.Date }
        });
    }



    return fac;
});

