/**
 * Dashboard Analytics
 */

'use strict';

(function () {
  let cardColor, labelColor, borderColor, chartBgColor, bodyColor;

  cardColor = config.colors.cardColor;
  labelColor = config.colors.textMuted;
  borderColor = config.colors.borderColor;
  chartBgColor = config.colors.chartBgColor;
  bodyColor = config.colors.bodyColor;

  // Weekly Overview Line Chart
  // --------------------------------------------------------------------
  const weeklyOverviewChartEl = document.querySelector('#weeklyOverviewChart'),
    weeklyOverviewChartConfig = {
      chart: {
        type: 'bar',
        height: 200,
        offsetY: -9,
        offsetX: -16,
        parentHeightOffset: 0,
        toolbar: {
          show: false
        }
      },
      series: [
        {
          name: 'Sales',
          data: [32, 55, 45, 75, 55, 35, 70]
        }
      ],
      colors: [chartBgColor],
      plotOptions: {
        bar: {
          borderRadius: 8,
          columnWidth: '30%',
          endingShape: 'rounded',
          startingShape: 'rounded',
          colors: {
            ranges: [
              {
                from: 75,
                to: 80,
                color: config.colors.primary
              },
              {
                from: 0,
                to: 73,
                color: chartBgColor
              }
            ]
          }
        }
      },
      dataLabels: {
        enabled: false
      },
      legend: {
        show: false
      },
      grid: {
        strokeDashArray: 8,
        borderColor,
        padding: {
          bottom: -10
        }
      },
      xaxis: {
        axisTicks: { show: false },
        crosshairs: { opacity: 0 },
        axisBorder: { show: false },
        categories: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
        tickPlacement: 'on',
        labels: {
          show: false
        },
        axisBorder: {
          show: false
        },
        axisTicks: {
          show: false
        }
      },
      yaxis: {
        min: 0,
        max: 90,
        show: true,
        tickAmount: 3,
        labels: {
          formatter: function (val) {
            return parseInt(val) + 'K';
          },
          style: {
            fontSize: '13px',
            fontFamily: 'Inter',
            colors: labelColor
          }
        }
      },
      states: {
        hover: {
          filter: {
            type: 'none'
          }
        },
        active: {
          filter: {
            type: 'none'
          }
        }
      },
      responsive: [
        {
          breakpoint: 1500,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '40%'
              }
            }
          }
        },
        {
          breakpoint: 1200,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '30%'
              }
            }
          }
        },
        {
          breakpoint: 815,
          options: {
            plotOptions: {
              bar: {
                borderRadius: 5
              }
            }
          }
        },
        {
          breakpoint: 768,
          options: {
            plotOptions: {
              bar: {
                borderRadius: 10,
                columnWidth: '20%'
              }
            }
          }
        },
        {
          breakpoint: 568,
          options: {
            plotOptions: {
              bar: {
                borderRadius: 8,
                columnWidth: '30%'
              }
            }
          }
        },
        {
          breakpoint: 410,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '50%'
              }
            }
          }
        }
      ]
    };
  if (typeof weeklyOverviewChartEl !== undefined && weeklyOverviewChartEl !== null) {
    const weeklyOverviewChart = new ApexCharts(weeklyOverviewChartEl, weeklyOverviewChartConfig);
    weeklyOverviewChart.render();
  }

  // Lấy dữ liệu từ phần tử HTML
  const chartDoanhThuTuanElement = document.getElementById("chartDoanhThuTuan");
  const chartDoanhThuTuanDataString = chartDoanhThuTuanElement.getAttribute("data-values");
  const chartDoanhThuTuanDataArray = chartDoanhThuTuanDataString.split(",").map(Number); // Chuyển chuỗi thành mảng số
  const chartDoanhThuTuanSeriesString = chartDoanhThuTuanElement.getAttribute("data-series");
  const chartDoanhThuTuanSeriesArray = chartDoanhThuTuanSeriesString.split(",").map(String); // Chuyển chuỗi thành mảng string

  // Weekly Overview Line Chart
  // --------------------------------------------------------------------
  const chartDoanhThuTuanEl = document.querySelector('#chartDoanhThuTuan'),
    chartDoanhThuTuanConfig = {
      chart: {
        type: 'bar',
        height: 200,
        offsetY: -9,
        offsetX: -16,
        parentHeightOffset: 0,
        toolbar: {
          show: false
        }
      },
      series: [
        {
          name: 'Doanh thu',
          data: chartDoanhThuTuanDataArray
        }
      ],
      colors: [chartBgColor],
      plotOptions: {
        bar: {
          borderRadius: 8,
          columnWidth: '30%',
          endingShape: 'rounded',
          startingShape: 'rounded',
          colors: {
            ranges: [
              {
                from: 0,
                to: 1500,
                color: config.colors.danger
              },
              {
                from: 1501,
                to: 3000,
                color: config.colors.primary
              },
              {
                from: 3000,
                to: 4000,
                color: config.colors.info
              },
              {
                from: 4000,
                to: 6000,
                color: config.colors.success
              }
            ]
          }
        }
      },
      dataLabels: {
        enabled: false
      },
      legend: {
        show: false
      },
      grid: {
        strokeDashArray: 8,
        borderColor,
        padding: {
          bottom: -10
        }
      },
      xaxis: {
        axisTicks: { show: false },
        crosshairs: { opacity: 0 },
        axisBorder: { show: false },
        categories: chartDoanhThuTuanSeriesArray,
        tickPlacement: 'on',
        labels: {
          show: false
        },
        axisBorder: {
          show: false
        },
        axisTicks: {
          show: false
        }
      },
      yaxis: {
        min: 0,
        max: 6000,
        show: true,
        tickAmount: 3,
        labels: {
          formatter: function (val) {
            return new Intl.NumberFormat('vi-VN').format(parseInt(val)) + 'K';
          },
          style: {
            fontSize: '13px',
            fontFamily: 'Inter',
            colors: labelColor
          }
        }
      },
      states: {
        hover: {
          filter: {
            type: 'none'
          }
        },
        active: {
          filter: {
            type: 'none'
          }
        }
      },
      responsive: [
        {
          breakpoint: 1500,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '40%'
              }
            }
          }
        },
        {
          breakpoint: 1200,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '30%'
              }
            }
          }
        },
        {
          breakpoint: 815,
          options: {
            plotOptions: {
              bar: {
                borderRadius: 5
              }
            }
          }
        },
        {
          breakpoint: 768,
          options: {
            plotOptions: {
              bar: {
                borderRadius: 10,
                columnWidth: '20%'
              }
            }
          }
        },
        {
          breakpoint: 568,
          options: {
            plotOptions: {
              bar: {
                borderRadius: 8,
                columnWidth: '30%'
              }
            }
          }
        },
        {
          breakpoint: 410,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '50%'
              }
            }
          }
        }
      ]
    };
  if (typeof chartDoanhThuTuanEl !== undefined && chartDoanhThuTuanEl !== null) {
    const chartDoanhThuTuan = new ApexCharts(chartDoanhThuTuanEl, chartDoanhThuTuanConfig);
    chartDoanhThuTuan.render();
  }

  // Total Profit line chart
  // --------------------------------------------------------------------
  const totalProfitLineChartEl = document.querySelector('#totalProfitLineChart'),
    totalProfitLineChartConfig = {
      chart: {
        height: 90,
        type: 'line',
        parentHeightOffset: 0,
        toolbar: {
          show: false
        }
      },
      grid: {
        borderColor: labelColor,
        strokeDashArray: 6,
        xaxis: {
          lines: {
            show: true
          }
        },
        yaxis: {
          lines: {
            show: false
          }
        },
        padding: {
          top: -15,
          left: -7,
          right: 9,
          bottom: -15
        }
      },
      colors: [config.colors.primary],
      stroke: {
        width: 3
      },
      series: [
        {
          data: [0, 20, 5, 30, 15, 45]
        }
      ],
      tooltip: {
        shared: false,
        intersect: true,
        x: {
          show: false
        }
      },
      xaxis: {
        labels: {
          show: false
        },
        axisTicks: {
          show: false
        },
        axisBorder: {
          show: false
        }
      },
      yaxis: {
        labels: {
          show: false
        }
      },
      tooltip: {
        enabled: false
      },
      markers: {
        size: 6,
        strokeWidth: 3,
        strokeColors: 'transparent',
        strokeWidth: 3,
        colors: ['transparent'],
        discrete: [
          {
            seriesIndex: 0,
            dataPointIndex: 5,
            fillColor: cardColor,
            strokeColor: config.colors.primary,
            size: 6,
            shape: 'circle'
          }
        ],
        hover: {
          size: 7
        }
      },
      responsive: [
        {
          breakpoint: 1350,
          options: {
            chart: {
              height: 80
            }
          }
        },
        {
          breakpoint: 1200,
          options: {
            chart: {
              height: 100
            }
          }
        },
        {
          breakpoint: 768,
          options: {
            chart: {
              height: 110
            }
          }
        }
      ]
    };
  if (typeof totalProfitLineChartEl !== undefined && totalProfitLineChartEl !== null) {
    const totalProfitLineChart = new ApexCharts(totalProfitLineChartEl, totalProfitLineChartConfig);
    totalProfitLineChart.render();
  }

  // Lấy dữ liệu từ phần tử HTML
  const dtThangLineChartElement = document.getElementById("dtThangLineChart");
  const dtThangLineChartDataString = dtThangLineChartElement.getAttribute("data-values");
  const dtThangLineChartDataArray = dtThangLineChartDataString.split(",").map(Number); // Chuyển chuỗi thành mảng số

  // Total Profit line chart
  // --------------------------------------------------------------------
  const dtThangLineChartEl = document.querySelector('#dtThangLineChart'),
    dtThangLineChartConfig = {
      chart: {
        height: 90,
        type: 'line',
        parentHeightOffset: 0,
        toolbar: {
          show: false
        }
      },
      grid: {
        borderColor: labelColor,
        strokeDashArray: 6,
        xaxis: {
          lines: {
            show: true
          }
        },
        yaxis: {
          lines: {
            show: false
          }
        },
        padding: {
          top: -15,
          left: -7,
          right: 9,
          bottom: -15
        }
      },
      colors: [config.colors.primary],
      stroke: {
        width: 3
      },
      series: [
        {
          data: dtThangLineChartDataArray
        }
      ],
      tooltip: {
        shared: false,
        intersect: true,
        x: {
          show: false
        }
      },
      xaxis: {
        labels: {
          show: false
        },
        axisTicks: {
          show: false
        },
        axisBorder: {
          show: false
        }
      },
      yaxis: {
        labels: {
          show: false
        }
      },
      tooltip: {
        enabled: false
      },
      markers: {
        size: 6,
        strokeWidth: 3,
        strokeColors: 'transparent',
        strokeWidth: 3,
        colors: ['transparent'],
        discrete: [
          {
            seriesIndex: 0,
            dataPointIndex: 5,
            fillColor: cardColor,
            strokeColor: config.colors.primary,
            size: 6,
            shape: 'circle'
          }
        ],
        hover: {
          size: 7
        }
      },
      responsive: [
        {
          breakpoint: 1350,
          options: {
            chart: {
              height: 80
            }
          }
        },
        {
          breakpoint: 1200,
          options: {
            chart: {
              height: 100
            }
          }
        },
        {
          breakpoint: 768,
          options: {
            chart: {
              height: 110
            }
          }
        }
      ]
    };
  if (typeof dtThangLineChartEl !== undefined && dtThangLineChartEl !== null) {
    const dtThangLineChart = new ApexCharts(dtThangLineChartEl, dtThangLineChartConfig);
    dtThangLineChart.render();
  }


  // Sessions Column Chart
  // --------------------------------------------------------------------
  const sessionsColumnChartEl = document.querySelector('#sessionsColumnChart'),
    sessionsColumnChartConfig = {
      chart: {
        height: 90,
        parentHeightOffset: 0,
        type: 'bar',
        toolbar: {
          show: false
        }
      },
      tooltip: {
        enabled: false
      },
      plotOptions: {
        bar: {
          barHeight: '100%',
          columnWidth: '20px',
          startingShape: 'rounded',
          endingShape: 'rounded',
          borderRadius: 4,
          colors: {
            ranges: [
              {
                from: 25,
                to: 32,
                color: config.colors.danger
              },
              {
                from: 60,
                to: 75,
                color: config.colors.primary
              },
              {
                from: 45,
                to: 50,
                color: config.colors.danger
              },
              {
                from: 35,
                to: 42,
                color: config.colors.primary
              }
            ],
            backgroundBarColors: [chartBgColor, chartBgColor, chartBgColor, chartBgColor, chartBgColor],
            backgroundBarRadius: 4
          }
        }
      },
      grid: {
        show: false,
        padding: {
          top: -10,
          left: -10,
          bottom: -15
        }
      },
      dataLabels: {
        enabled: false
      },
      legend: {
        show: false
      },
      xaxis: {
        labels: {
          show: false
        },
        axisTicks: {
          show: false
        },
        axisBorder: {
          show: false
        }
      },
      yaxis: {
        labels: {
          show: false
        }
      },
      series: [
        {
          data: [30, 70, 50, 40, 60]
        }
      ],
      responsive: [
        {
          breakpoint: 1350,
          options: {
            chart: {
              height: 80
            },
            plotOptions: {
              bar: {
                columnWidth: '40%'
              }
            }
          }
        },
        {
          breakpoint: 1200,
          options: {
            chart: {
              height: 100
            },
            plotOptions: {
              bar: {
                columnWidth: '20%'
              }
            }
          }
        },
        {
          breakpoint: 768,
          options: {
            chart: {
              height: 110
            },
            plotOptions: {
              bar: {
                columnWidth: '10%'
              }
            }
          }
        },
        {
          breakpoint: 480,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '20%'
              }
            }
          }
        }
      ]
    };
  if (typeof sessionsColumnChartEl !== undefined && sessionsColumnChartEl !== null) {
    const sessionsColumnChart = new ApexCharts(sessionsColumnChartEl, sessionsColumnChartConfig);
    sessionsColumnChart.render();
  }

  // Lấy dữ liệu từ phần tử HTML
  const cpThangColumnChartElement = document.getElementById("cpThangColumnChart");
  const cpThangColumnChartDataString = cpThangColumnChartElement.getAttribute("data-values");
  const cpThangColumnChartDataArray = cpThangColumnChartDataString.split(",").map(Number); // Chuyển chuỗi thành mảng số
  // Sessions Column Chart
  // --------------------------------------------------------------------
  const cpThangColumnChartEl = document.querySelector('#cpThangColumnChart'),
    cpThangColumnChartConfig = {
      chart: {
        height: 90,
        parentHeightOffset: 0,
        type: 'bar',
        toolbar: {
          show: false
        }
      },
      tooltip: {
        enabled: false
      },
      plotOptions: {
        bar: {
          barHeight: '100%',
          columnWidth: '20px',
          startingShape: 'rounded',
          endingShape: 'rounded',
          borderRadius: 4,
          colors: {
            ranges: [
              {
                from: 1,
                to: 5000,
                color: config.colors.danger
              },
              {
                from: 5001,
                to: 10000,
                color: config.colors.primary
              },
              {
                from: 10001,
                to: 15000,
                color: config.colors.info
              },
              {
                from: 15001,
                to: 30000,
                color: config.colors.success
              }
            ],
            backgroundBarColors: [chartBgColor, chartBgColor, chartBgColor, chartBgColor, chartBgColor],
            backgroundBarRadius: 4
          }
        }
      },
      grid: {
        show: false,
        padding: {
          top: -10,
          left: -10,
          bottom: -15
        }
      },
      dataLabels: {
        enabled: false
      },
      legend: {
        show: false
      },
      xaxis: {
        labels: {
          show: false
        },
        axisTicks: {
          show: false
        },
        axisBorder: {
          show: false
        }
      },
      yaxis: {
        labels: {
          show: false
        }
      },
      series: [
        {
          data: cpThangColumnChartDataArray
        }
      ],
      responsive: [
        {
          breakpoint: 1350,
          options: {
            chart: {
              height: 80
            },
            plotOptions: {
              bar: {
                columnWidth: '40%'
              }
            }
          }
        },
        {
          breakpoint: 1200,
          options: {
            chart: {
              height: 100
            },
            plotOptions: {
              bar: {
                columnWidth: '20%'
              }
            }
          }
        },
        {
          breakpoint: 768,
          options: {
            chart: {
              height: 110
            },
            plotOptions: {
              bar: {
                columnWidth: '10%'
              }
            }
          }
        },
        {
          breakpoint: 480,
          options: {
            plotOptions: {
              bar: {
                columnWidth: '20%'
              }
            }
          }
        }
      ]
    };
  if (typeof cpThangColumnChartEl !== undefined && cpThangColumnChartEl !== null) {
    const cpThangColumnChart = new ApexCharts(cpThangColumnChartEl, cpThangColumnChartConfig);
    cpThangColumnChart.render();
  }

  // Lấy dữ liệu từ phần tử HTML
  const tyLeCacPhongThangRoundChartElement = document.getElementById("tyLeCacPhongThangRoundChart");
  const tyLeCacPhongThangRoundChartDataString = tyLeCacPhongThangRoundChartElement.getAttribute("data-values");
  const tyLeCacPhongThangRoundChartDataArray = tyLeCacPhongThangRoundChartDataString.split(",").map(Number); // Chuyển chuỗi thành mảng số
  const tyLeCacPhongThangRoundChartSeriesString = tyLeCacPhongThangRoundChartElement.getAttribute("data-series");
  const tyLeCacPhongThangRoundChartSeriesArray = tyLeCacPhongThangRoundChartSeriesString.split(",").map(String); // Chuyển chuỗi thành mảng string
  const tyLeCacPhongThangRoundChartTotalString = tyLeCacPhongThangRoundChartElement.getAttribute("data-total").toString();

  // Pie chart ty le cac phong trong thang
  const tyLeCacPhongThangRoundChartEl = document.querySelector('#tyLeCacPhongThangRoundChart'),
    tyLeCacPhongThangRoundChartConfig = {
      chart: {
        height: 250,
        parentHeightOffset: 0,
        type: "donut"
      },
      labels: tyLeCacPhongThangRoundChartSeriesArray,
      series: tyLeCacPhongThangRoundChartDataArray,
      colors: [config.colors.success, config.colors.secondary, config.colors.danger, config.colors.warning],
      stroke: {
        width: 0
      },
      dataLabels: {
        enabled: !0
      },
      legend: {
        show: !0,
        position: "right",
        fontSize: "13px",
        labels: {
          colors: config.colors.black,
          useSeriesColors: !1
        },
        markers: {
          offsetY: 0,
          offsetX: -3,
          height: 10,
          width: 10
        },
        itemMargin: {
          vertical: 3,
          horizontal: 10
        }
      },
      tooltip: {
        style: {
          color: config.colors.white
        }
      },
      grid: {
        padding: {
          top: 0
        }
      },
      plotOptions: {
        pie: {
          donut: {
            size: "70%",
            labels: {
              show: !0,
              value: {
                fontSize: "22px",
                fontFamily: "Inter",
                color: config.colors.headingColor,
                fontWeight: 500,
                offsetY: -20,
                formatter: function (val) {
                  return new Intl.NumberFormat('vi-VN').format(parseInt(val)) + 'K';
                }
              },
              name: {
                offsetY: 20,
                fontFamily: "Inter",
                fontWeight: "500",
                formatter: function (val) {
                  return val
                }
              },
              total: {
                show: !0,
                fontSize: "15px",
                fontFamily: "Inter",
                label: "Tổng",
                formatter: function (e) {
                  return tyLeCacPhongThangRoundChartTotalString + 'K';
                }
              }
            }
          }
        }
      },
      responsive: [{
        breakpoint: 1399,
        options: {
          chart: {
            height: 200
          }
        }
      }, {
        breakpoint: 420,
        options: {
          chart: {
            height: 300
          }
        }
      }]
    };

  if (typeof tyLeCacPhongThangRoundChartEl !== undefined && tyLeCacPhongThangRoundChartEl !== null) {
    const tyLeCacPhongThangRoundChart = new ApexCharts(tyLeCacPhongThangRoundChartEl, tyLeCacPhongThangRoundChartConfig);
    tyLeCacPhongThangRoundChart.render();
  };
})();
