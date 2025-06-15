//( () => {
//    var e = document.querySelector("#flatpickr-date")
//      , t = document.querySelector("#flatpickr-time")
//      , a = document.querySelector("#flatpickr-datetime")
//      , r = document.querySelector("#flatpickr-multi")
//      , i = document.querySelector("#flatpickr-range")
//      , n = document.querySelector("#flatpickr-inline")
//      , o = document.querySelector("#flatpickr-human-friendly")
//      , c = document.querySelector("#flatpickr-disabled-range");
//    e && e.flatpickr({
//        monthSelectorType: "static",
//        static: !0
//    }),
//    t && t.flatpickr({
//        enableTime: !0,
//        noCalendar: !0,
//        static: !0
//    }),
//    a && a.flatpickr({
//        enableTime: !0,
//        dateFormat: "Y-m-d H:i",
//        static: !0
//    }),
//    r && r.flatpickr({
//        weekNumbers: !0,
//        enableTime: !0,
//        mode: "multiple",
//        minDate: "today",
//        static: !0
//    }),
//    null != typeof i && i.flatpickr({
//        mode: "range",
//        static: !0
//    }),
//    n && n.flatpickr({
//        inline: !0,
//        allowInput: !1,
//        monthSelectorType: "static"
//    }),
//    o && o.flatpickr({
//        altInput: !0,
//        altFormat: "F j, Y",
//        dateFormat: "Y-m-d",
//        static: !0
//    }),
//    c && (e = new Date(Date.now() - 1728e5),
//    t = new Date(Date.now() + 1728e5),
//    c.flatpickr({
//        dateFormat: "Y-m-d",
//        disable: [{
//            from: e.toISOString().split("T")[0],
//            to: t.toISOString().split("T")[0]
//        }],
//        static: !0
//    }))
//}
//)(),
$(function() {
    var e = $("#bs-rangepicker-basic")
      , t = $("#bs-rangepicker-single")
      , a = $("#bs-rangepicker-time")
      , r = $("#bs-rangepicker-range")
      , i = $("#bs-rangepicker-week-num")
      , n = $("#bs-rangepicker-dropdown")
      // dành cho filter date picker
      , h = $(".bs-rangepicker-filter")
      , e = (e.length && e.daterangepicker({
        opens: isRtl ? "left" : "right"
    }),
    t.length && t.daterangepicker({
        singleDatePicker: !0,
        opens: isRtl ? "left" : "right"
    }),
    a.length && a.daterangepicker({
        timePicker: !0,
        timePickerIncrement: 30,
        locale: {
            format: "MM/DD/YYYY h:mm A"
        },
        opens: isRtl ? "left" : "right"
    }),
    r.length && r.daterangepicker({
        ranges: {
            Today: [moment(), moment()],
            Yesterday: [moment().subtract(1, "days"), moment().subtract(1, "days")],
            "Last 7 Days": [moment().subtract(6, "days"), moment()],
            "Last 30 Days": [moment().subtract(29, "days"), moment()],
            "This Month": [moment().startOf("month"), moment().endOf("month")],
            "Last Month": [moment().subtract(1, "month").startOf("month"), moment().subtract(1, "month").endOf("month")]
        },
        opens: isRtl ? "left" : "right"
    }),
    i.length && i.daterangepicker({
        showWeekNumbers: !0,
        opens: isRtl ? "left" : "right"
    }),
    n.length && n.daterangepicker({
        showDropdowns: !0,
        opens: isRtl ? "left" : "right"
    }),
    // nếu độ rộng desktop thì hiển thị range, nếu độ rộng mobile thì ẩn range
    h.length && window.innerWidth > 768 && h.daterangepicker({
        showDropdowns: !0,
        opens: isRtl ? "left" : "right",
        startDate: moment().subtract(7, 'days'),
        endDate: moment(),
        minDate: '1980-01-01',
        timePicker: false,
        timePicker24Hour: false,
        autoApply: true,
        showISOWeekNumbers: true,
        locale: {
            format: 'DD/MM/YYYY',
            applyLabel: 'Chọn',
            cancelLabel: 'Hủy',
            customRangeLabel: 'Tùy chọn',
        },
        ranges: {
            'Hôm nay': [moment(), moment()],
            'Hôm qua': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            '7 ngày qua': [moment().subtract(6, 'days'), moment()],
            '30 ngày qua': [moment().subtract(29, 'days'), moment()]
        }
    }),
    h.length && window.innerWidth < 768 && h.daterangepicker({
        showDropdowns: !0,
        opens: isRtl ? "left" : "right",
        startDate: moment().subtract(7, 'days'),
        endDate: moment(),
        minDate: '1980-01-01',
        timePicker: false,
        timePicker24Hour: false,
        autoApply: true,
        locale: {
            format: 'DD/MM/YYYY',
            applyLabel: 'Chọn',
            cancelLabel: 'Hủy',
            customRangeLabel: 'Tùy chọn',
        }
    }),
    document.getElementsByClassName("cancelBtn"))
      , t = (Array.from(e).forEach(e => {
        e.classList.remove("btn-default"),
        e.classList.add("btn-label-primary")
    }
    ),
    $("#timepicker-basic"))
      , a = $("#timepicker-min-max")
      , r = $("#timepicker-disabled-times")
      , i = $("#timepicker-format")
      , n = $("#timepicker-step")
      , e = $("#timepicker-24hours");
    t.length && t.timepicker({
        orientation: isRtl ? "r" : "l"
    }),
    a.length && a.timepicker({
        minTime: "2:00pm",
        maxTime: "7:00pm",
        showDuration: !0,
        orientation: isRtl ? "r" : "l"
    }),
    r.length && r.timepicker({
        disableTimeRanges: [["12am", "3am"], ["4am", "4:30am"]],
        orientation: isRtl ? "r" : "l"
    }),
    i.length && i.timepicker({
        timeFormat: "H:i:s",
        orientation: isRtl ? "r" : "l"
    }),
    n.length && n.timepicker({
        step: 15,
        orientation: isRtl ? "r" : "l"
    }),
    e.length && e.timepicker({
        show: "24:00",
        timeFormat: "H:i:s",
        orientation: isRtl ? "r" : "l"
    })
}),
( () => {
    var e = document.querySelector("#color-picker-classic")
      , t = document.querySelector("#color-picker-monolith")
      , a = document.querySelector("#color-picker-nano");
    e && new Pickr({
        el: e,
        theme: "classic",
        default: "rgba(144, 85, 253, 1)",
        swatches: ["rgba(144, 85, 253, 1)", "rgba(86, 202, 0, 1)", "rgba(255, 76, 81, 1)", "rgba(255, 180, 0, 1)", "rgba(22, 177, 255, 1)"],
        components: {
            preview: !0,
            opacity: !0,
            hue: !0,
            interaction: {
                hex: !0,
                rgba: !0,
                hsla: !0,
                hsva: !0,
                cmyk: !0,
                input: !0,
                clear: !0,
                save: !0
            }
        }
    }),
    t && new Pickr({
        el: t,
        theme: "monolith",
        default: "rgba(86, 202, 0, 1)",
        swatches: ["rgba(144, 85, 253, 1)", "rgba(86, 202, 0, 1)", "rgba(255, 76, 81, 1)", "rgba(255, 180, 0, 1)", "rgba(22, 177, 255, 1)"],
        components: {
            preview: !0,
            opacity: !0,
            hue: !0,
            interaction: {
                hex: !0,
                rgba: !0,
                hsla: !0,
                hsva: !0,
                cmyk: !0,
                input: !0,
                clear: !0,
                save: !0
            }
        }
    }),
    a && new Pickr({
        el: a,
        theme: "nano",
        default: "rgba(255, 76, 81, 1)",
        swatches: ["rgba(144, 85, 253, 1)", "rgba(86, 202, 0, 1)", "rgba(255, 76, 81, 1)", "rgba(255, 180, 0, 1)", "rgba(22, 177, 255, 1)"],
        components: {
            preview: !0,
            opacity: !0,
            hue: !0,
            interaction: {
                hex: !0,
                rgba: !0,
                hsla: !0,
                hsva: !0,
                cmyk: !0,
                input: !0,
                clear: !0,
                save: !0
            }
        }
    })
}
)();
